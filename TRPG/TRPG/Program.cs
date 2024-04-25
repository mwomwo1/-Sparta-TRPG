using System;
using System.Collections.Generic;

namespace TRPG
{
    class Program
    {
        // 시작화면에서 선택할 수 있는 옵션
        public enum STARTSELECT
        {
            SELECTSTATUS,
            SELECTINVENTORY,
            SELECTSHOP,
            ENTERDUNGEON,
            REST,
            END
        }

        // 상점에서 선택할 수 있는 옵션
        public enum SHOPSELECT
        {
            BUYITEM,
            EXITSHOP
        }

        // 아이템 클래스에 ItemType 추가
        public enum ItemType
        {
            Armor,
            Weapon
        }

        // 던전 난이도
        public enum DungeonDifficulty
        {
            Easy,
            Normal,
            Hard
        }

        // 던전 결과
        public enum DungeonResult
        {
            Success,
            Failure
        }

        // 시작
        public static STARTSELECT StartSelect()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("게임 시작 화면");
                Console.WriteLine();
                Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
                Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.");
                Console.WriteLine();
                Console.WriteLine("1. 상태 보기");
                Console.WriteLine("2. 인벤토리");
                Console.WriteLine("3. 상점");
                Console.WriteLine("4. 던전 입장");
                Console.WriteLine("5. 휴식");
                Console.WriteLine("6. 종료");
                Console.WriteLine();
                Console.WriteLine("원하시는 행동을 입력해주세요");

                string input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        return STARTSELECT.SELECTSTATUS;
                    case "2":
                        return STARTSELECT.SELECTINVENTORY;
                    case "3":
                        return STARTSELECT.SELECTSHOP;
                    case "4":
                        return STARTSELECT.ENTERDUNGEON;
                    case "5":
                        return STARTSELECT.REST;
                    case "6": // 추가된 옵션
                        return STARTSELECT.END; // 종료 옵션
                    default:
                        Console.WriteLine("잘못된 입력입니다, 다시 입력해주세요.");
                        break;
                }
            }
        }

        // 아이템 클래스
        class Item
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public int Attack { get; set; }
            public int Defense { get; set; }
            public int Price { get; set; }
            public bool Purchased { get; set; } // 구매 여부 추가
            public ItemType Type { get; set; } // 아이템의 유형 (방어구 or 무기)

            // 생성자에 ItemType 추가
            public Item(string name, string description, int attack, int defense, int price, ItemType type)
            {
                Name = name;
                Description = description;
                Attack = attack;
                Defense = defense;
                Price = price;
                Purchased = false; // 초기값은 false로 설정
                Type = type;
            }
        }

        

        // 던전 아이템 목록
        static List<Item> dungeonInventory = new List<Item>
        {
            // 각 던전별 아이템 목록을 추가할 수 있습니다.
        };

        // 던전 입장
        static void EnterDungeon(Player player)
        {
            Console.Clear();
            Console.WriteLine("**던전입장**");
            Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.");
            Console.WriteLine();
            Console.WriteLine("1. 쉬운 던전     | 방어력 5 이상 권장");
            Console.WriteLine("2. 일반 던전     | 방어력 11 이상 권장");
            Console.WriteLine("3. 어려운 던전    | 방어력 17 이상 권장");
            Console.WriteLine("0. 나가기");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");

            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    StartDungeon(player, DungeonDifficulty.Easy);
                    break;
                case "2":
                    StartDungeon(player, DungeonDifficulty.Normal);
                    break;
                case "3":
                    StartDungeon(player, DungeonDifficulty.Hard);
                    break;
                case "0":
                    Console.WriteLine("던전입장을 취소합니다.");
                    break;
                default:
                    Console.WriteLine("잘못된 입력입니다, 다시 입력해주세요.");
                    break;
            }
        }

        // 던전 시작 기능
        static void StartDungeon(Player player, DungeonDifficulty difficulty)
        {
            int playerDefense = player.CalculateTotalDefense();
            int recommendedDefense = RecommendedDefense[difficulty];

           
            Console.WriteLine($"**{difficulty.ToString()} 던전**");
            Console.WriteLine($"권장 방어력: {recommendedDefense}");
            Console.WriteLine();

            if (playerDefense < recommendedDefense)
            {
                Console.WriteLine("<<현재 방어력이 권장 방어력보다 낮습니다>>");
                Console.WriteLine(" 던전을 수행할 수 있는 충분한 능력이 없습니다");
                Console.WriteLine(".");
                Console.WriteLine(".");
                Console.WriteLine(".");
                Console.WriteLine("던전 입장에 실패했습니다.");
                Console.WriteLine();
                Console.WriteLine("다시 플레이하시려면 아무 곳이나 누르세요.");
                player.Health -= 50; // 체력 감소
                Console.ReadLine();
                return;
            }
            Console.WriteLine("던전에 입장합니다. ");
            Console.WriteLine(".");
            Console.WriteLine(".");
            Console.WriteLine(".");
            Console.WriteLine(".");
            Console.WriteLine(".");
            Console.WriteLine("아무 곳이나 클릭하세");
            Console.ReadLine();

            Random rand = new Random();
            int baseReward = DungeonInfo[difficulty].BaseReward;
            int additionalRewardPercentage = DungeonInfo[difficulty].AdditionalRewardPercentage;
            int additionalReward = (int)(baseReward * ((float)rand.Next(1, additionalRewardPercentage + 1) / 100));
            int totalReward = baseReward + additionalReward;

            Console.WriteLine($"던전을 클리어하였습니다! 보상으로 {totalReward}G를 획득하였습니다.");
            Console.WriteLine(".");
            Console.WriteLine(".");
            Console.WriteLine(".");
            Console.WriteLine("돌아가려면 아무 것이나 입력");
            Console.ReadLine();
            int damage = rand.Next(20, 36) + (playerDefense - recommendedDefense); // 기본 체력 감소량 계산
            player.Health -= damage;
            return;
        }

        // 던전 난이도에 따른 권장 방어력
        public static Dictionary<DungeonDifficulty, int> RecommendedDefense = new Dictionary<DungeonDifficulty, int>
        {
            { DungeonDifficulty.Easy, 5 },
            { DungeonDifficulty.Normal, 11 },
            { DungeonDifficulty.Hard, 17 }
        };

        // 던전에 대한 정보
        public static Dictionary<DungeonDifficulty, DungeonReward> DungeonInfo = new Dictionary<DungeonDifficulty, DungeonReward>
        {
            { DungeonDifficulty.Easy, new DungeonReward { BaseReward = 1000, AdditionalRewardPercentage = 20 } },
            { DungeonDifficulty.Normal, new DungeonReward { BaseReward = 1700, AdditionalRewardPercentage = 30 } },
            { DungeonDifficulty.Hard, new DungeonReward { BaseReward = 2500, AdditionalRewardPercentage = 40 } }
        };


        // 던전 클리어 보상
        public struct DungeonReward
        {
            public int BaseReward;
            public int AdditionalRewardPercentage;
        }

        // 휴식 기능
        static void Rest(Player player)
        {
            Console.Clear();
            Console.WriteLine("**휴식**");
            Console.WriteLine("휴식을 선택하면 500G으로 체력을 회복합니다.");
            Console.WriteLine();
            Console.WriteLine($"현재 체력: {player.Health} / {Player.MaxHealth}");
            Console.WriteLine($"보유 골드: {player.Gold}G");
            Console.WriteLine();
            Console.WriteLine("휴식을 선택하시겠습니까? (Y/N)");

            string input = Console.ReadLine();
            if (input.ToLower() == "y")
            {
                if (player.Gold >= 500)
                {
                    player.Gold -= 500;
                    player.Health = Math.Min(Player.MaxHealth, player.Health + 100);
                    Console.WriteLine("휴식을 완료했습니다.");
                }
                else
                {
                    Console.WriteLine("Gold가 부족합니다.");
                }
            }
        }




        // 상점 아이템 목록
        static List<Item> shopInventory = new List<Item>
        {
            new Item("수련자 갑옷", "방어력 +5 | 수련에 도움을 주는 갑옷입니다.", 0, 5, 1000, ItemType.Armor),
            new Item("무쇠갑옷", "방어력 +9 | 무쇠로 만들어져 튼튼한 갑옷입니다.", 0, 9, 2000, ItemType.Armor),
            new Item("스파르타의 갑옷", "방어력 +15 | 스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", 0, 15, 3500, ItemType.Armor),
            new Item("낡은 검", "공격력 +2 | 쉽게 볼 수 있는 낡은 검 입니다.", 2, 0, 600, ItemType.Weapon),
            new Item("청동 도끼", "공격력 +5 | 어디선가 사용됐던거 같은 도끼입니다.", 5, 0, 1500, ItemType.Weapon),
            new Item("스파르타의 창", "공격력 +7 | 스파르타의 전사들이 사용했다는 전설의 창입니다.", 7, 0, 2000, ItemType.Weapon)
        };

        // 아이템 구매 화면
        static SHOPSELECT ShopSelect(Player player)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("**상점 - 아이템 구매**");
                Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
                Console.WriteLine();
                Console.WriteLine("[보유 골드]");
                Console.WriteLine($"{player.Gold}G");
                Console.WriteLine();
                Console.WriteLine("[아이템 목록]");
                // 아이템 목록 출력 및 구매 여부에 따라 상태 표시
                for (int i = 0; i < shopInventory.Count; i++)
                {
                    string purchasedStatus = shopInventory[i].Purchased ? "구매완료" : $"{shopInventory[i].Price} G";
                    Console.WriteLine($"{i + 1}. {shopInventory[i].Name} | {shopInventory[i].Description} | {purchasedStatus}");
                }
                Console.WriteLine();
                Console.WriteLine("1. 물건 구매하기 ");
                Console.WriteLine("0. 나가기");
                Console.WriteLine();
                Console.WriteLine("원하시는 행동을 입력해주세요.");

                string input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        return SHOPSELECT.BUYITEM;
                    case "0":
                        return SHOPSELECT.EXITSHOP;
                    default:
                        Console.WriteLine("잘못된 입력입니다 , 다시 입력해주세요 .");
                        break;
                }
            }
        }

        // 아이템 구매 기능
        static void BuyItem(Player player)
        {
            Console.WriteLine("구매할 아이템 번호를 입력하세요.");
            int input;
            while (true)
            {
                if (!int.TryParse(Console.ReadLine(), out input))
                {
                    Console.WriteLine("잘못된 입력입니다.");
                    continue;
                }

                if (input == 0)
                {
                    return;
                }
                else if (input <= shopInventory.Count)
                {

                    //thread readLine
                    int selectedIndex = input - 1;
                    // 이미 구매한 아이템인지 확인
                    if (shopInventory[selectedIndex].Purchased)
                    {
                        Console.WriteLine("이미 구매한 아이템입니다.");
                        continue;

                    }
                    // 보유 골드와 아이템 가격 비교
                    if (player.Gold >= shopInventory[selectedIndex].Price)
                    {
                        // 구매 가능한 경우
                        player.Gold -= shopInventory[selectedIndex].Price; // 보유 골드에서 가격 차감
                        player.Inventory.Add(new Item(shopInventory[selectedIndex].Name, shopInventory[selectedIndex].Description, shopInventory[selectedIndex].Attack, shopInventory[selectedIndex].Defense, shopInventory[selectedIndex].Price, shopInventory[selectedIndex].Type));
                        shopInventory[selectedIndex].Purchased = true; // 상점에서 구매 완료로 변경
                        Console.WriteLine("구매를 완료했습니다.");
                        continue;
                    }
                    else
                    {
                        Console.WriteLine("Gold가 부족합니다.");
                        continue;

                    }
                    Thread.Sleep(3000);
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                    continue;
                }
            }
        }

        // 메인 메서드 수정
        static void Main(string[] args)
        {
            Player player = new Player(); // 플레이어 생성
            while (true)
            {
                Console.Clear();
                STARTSELECT selectedAction = StartSelect(); // 시작화면 호출
                if (selectedAction == STARTSELECT.END) // 추가된 조건문
                {
                    Console.WriteLine("게임을 종료합니다.");
                    break; // 루프 종료
                }
                switch (selectedAction)
                {
                    case STARTSELECT.SELECTSTATUS:
                        player.DisplayStatus(); // 상태 보기
                        break;
                    case STARTSELECT.SELECTINVENTORY:
                        player.DisplayInventory(); // 인벤토리 표시
                        break;
                    case STARTSELECT.SELECTSHOP:
                        SHOPSELECT shopAction = ShopSelect(player); // 상점 선택 화면 호출
                        switch (shopAction)
                        {
                            case SHOPSELECT.BUYITEM:
                                BuyItem(player); // 아이템 구매
                                break;
                            case SHOPSELECT.EXITSHOP:
                                Console.WriteLine("상점을 나갑니다.");
                                break;
                        }
                        break;
                    case STARTSELECT.ENTERDUNGEON:
                        EnterDungeon(player); // 던전 입장
                        break;
                    case STARTSELECT.REST:
                        Rest(player); // 휴식
                        break;
                }
            }
            
        }

        // 플레이어 클래스
        class Player
        {
            public static int MaxHealth = 100;
            public int Health { get; set; }
            public List<Item> Inventory { get; set; }
            public int Gold { get; set; }

            public Player()
            {
                Health = MaxHealth;
                Inventory = new List<Item>();
                Gold = 1000;
            }

            // 상태를 출력하는 메서드 내부의 StartSelect 메서드 수정
            public void DisplayStatus()
            {
                Console.Clear();
                Console.WriteLine("상태 보기");
                Console.WriteLine("캐릭터의 정보가 표시됩니다.");
                Console.WriteLine();
                Console.WriteLine($"체력: {Health} / {MaxHealth}");
                Console.WriteLine($"Gold : {Gold} G");
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("뒤로 돌아가시려면 아무버튼이나 누르세요.");
                string input = Console.ReadLine();
                
                
            }

            // 인벤토리 표시 메서드
            public void DisplayInventory()
            {
                Console.Clear();
                Console.WriteLine("인벤토리 - 장착 관리");
                Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
                Console.WriteLine();
                Console.WriteLine("[아이템 목록]");
                for (int i = 0; i < Inventory.Count; i++)
                {
                    Console.WriteLine($"{i + 1} {Inventory[i].Name} | {Inventory[i].Description}");
                }
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("뒤로 돌아가시려면 아무버튼이나 누르세요.");
                string input = Console.ReadLine();
            }

            // 모든 장비의 방어력을 합산하는 메서드
            public int CalculateTotalDefense()
            {
                int totalDefense = 0;
                foreach (var item in Inventory)
                {
                    totalDefense += item.Defense;
                }
                return totalDefense;
            }

        }
    }
}

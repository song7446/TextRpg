using System.Threading;

namespace TextRpg
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"스파르타 던전에 오신걸 환영합니다!");
            Player player = new Player();
            Stage stage = new Stage();
            Shops shops = new Shops();
            InitShop(shops);

            SettingNameJob(player);

            while (true)
            {
                int beforeStage = StartScene();

                switch (beforeStage)
                {
                    case 1:
                        ViewState(player);
                        break;
                    case 2:
                        Inventory(player);
                        break;
                    case 3:
                        ViewShop(player, shops);
                        break;
                }
            }
            stage.Start();
        }

        // 초기 이름 설정 
        public static void SettingNameJob(Player player)
        {
            Console.WriteLine("이름을 입력해주세요");
            string name = Console.ReadLine();
            player.name = name;
            while (true)
            {
                Console.WriteLine("원하시는 직업을 입력해주세요");
                Console.WriteLine("1. 전사");
                Console.WriteLine("2. 도적");
                Console.WriteLine("3. 궁수");
                Console.WriteLine("4. 마법사");

                string job = Console.ReadLine();
                if (job == "1" || job == "전사")
                {
                    player.job = JOBENUM.WARRIOR;
                    break;
                }
                else if (job == "2" || job == "도적")
                {
                    player.job = JOBENUM.THIEF;
                    break;
                }
                else if (job == "3" || job == "궁수")
                {
                    player.job = JOBENUM.ARCHER;
                    break;
                }
                else if (job == "4" || job == "마법사")
                {
                    player.job = JOBENUM.WIZARD;
                    break;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("1부터 4중에서 입력해주세요");
                }
            }
            Console.Clear();
        }

        // 시작 씬
        public static int StartScene()
        {
            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
            Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.");
            string input;
            int intInput;
            while (true)
            {
                Console.WriteLine("1. 상태 보기");
                Console.WriteLine("2. 인벤토리");
                Console.WriteLine("3. 상점");
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                input = Console.ReadLine();
                if (input == "1" || input == "2" || input == "3")
                {
                    intInput = Convert.ToInt32(input);
                    break;
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다");
                    Console.Clear();
                }
            }
            Console.Clear();
            return intInput;
        }

        public static void ViewState(Player warrior)
        {
            string input = "";
            while (input != "0")
            {
                Console.WriteLine("상태 보기");
                Console.WriteLine("캐릭터의 정보가 표시됩니다.");
                Console.WriteLine($"Lv. {warrior.level}");
                Console.WriteLine($"{warrior.name} ( {warrior.job.ToString()} )");
                Console.WriteLine($"공격력 : {warrior.attack}");
                Console.WriteLine($"방어력 : {warrior.defense}");
                Console.WriteLine($"체  력 : {warrior.hp}");
                Console.WriteLine($"Gold   : {warrior.gold}");
                Console.WriteLine("");
                Console.WriteLine("0. 나가기");
                Console.WriteLine("");
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                input = Console.ReadLine();
                Console.Clear();
            }
        }

        public static void Inventory(Player player)
        {
            string input = "";
            bool isPutOnSetting = false;
            while (true)
            {
                Console.Write("인벤토리");
                if (isPutOnSetting)
                {
                    Console.Write(" - 장착 관리");
                }
                Console.WriteLine();
                Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
                Console.WriteLine("");
                Console.WriteLine("[아이템 목록]");

                if (player.equipments != null)
                {
                    for (int i = 0; i < player.equipments.Count; i++)
                    {
                        if (isPutOnSetting)
                        {
                            Console.Write($"- {i + 1} {(player.equipments[i].isOn ? "[E]" : "")}{player.equipments[i].name} | {player.equipments[i].statName} +{player.equipments[i].stats} | {player.equipments[i].explains}");
                        }
                        else
                        {
                            Console.Write($"- {(player.equipments[i].isOn ? "[E]" : "")}{player.equipments[i].name} | {player.equipments[i].statName} +{player.equipments[i].stats} | {player.equipments[i].explains}");
                        }
                    }
                }

                if (!isPutOnSetting)
                {
                    Console.WriteLine("1. 장착 관리");
                }
                else
                {
                    Console.WriteLine();
                }
                Console.WriteLine("0. 나가기");
                Console.WriteLine("");
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                input = Console.ReadLine();
                Console.Clear();
                if (input == "1" || input == "장착 관리")
                {
                    isPutOnSetting = true;
                }
                else if (input == "0" || input == "나가기")
                {
                    if (isPutOnSetting)
                    {
                        isPutOnSetting = false;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }


        public static void ViewShop(Player player, Shops shops)
        {
            string input = "";
            bool isBuyItem = false;
            while (true)
            {
                Console.Write("상점");
                if (isBuyItem)
                {
                    Console.Write(" - 아이템 구매");
                }
                Console.WriteLine();
                Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
                Console.WriteLine("");
                Console.WriteLine("[보유 골드]");
                Console.WriteLine($"{player.gold} G");
                Console.WriteLine("");
                Console.WriteLine("[아이템 목록]");
                for (int i = 0; i < shops.equips.Count; i++)
                {
                    if (isBuyItem)
                    {
                        Console.WriteLine($"- {i + 1} {shops.equips[i].name} | {shops.equips[i].statName} +{shops.equips[i].stats} | {shops.equips[i].explains} " +
                           $"| {(shops.equips[i].isBuyed ? "구매완료" : shops.equips[i].gold)}");
                    }
                    else
                    {

                        Console.WriteLine($"- {shops.equips[i].name} | {shops.equips[i].statName} +{shops.equips[i].stats} | {shops.equips[i].explains} " +
                            $"| {(shops.equips[i].isBuyed ? "구매완료" : shops.equips[i].gold)}");
                    }
                }
                Console.WriteLine();
                if (!isBuyItem)
                {
                    Console.WriteLine("1. 아이템 구매");
                }
                else
                {
                    Console.WriteLine();
                }
                Console.WriteLine("0. 나가기");
                Console.WriteLine();
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                input = Console.ReadLine();
                Console.Clear();
                if (input == "1" || input == "아이템 구매")
                {
                    isBuyItem = true;
                }
                else if (input == "0" || input == "나가기")
                {
                    if (isBuyItem)
                    {
                        isBuyItem = false;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        public static void InitShop(Shops shops)
        {
            string[] name = { "수련자 갑옷", "무쇠갑옷", "스파르타의 갑옷", "낡은 검", "청동 도끼 ", "스파르타의 창" };
            string[] statName = { "방어력", "방어력", "방어력", "공격력", "공격력", "공격력" };
            int[] stat = { 5, 9, 15, 2, 5, 7 };
            string[] explain = { "수련에 도움을 주는 갑옷입니다.", "무쇠로 만들어져 튼튼한 갑옷입니다.", "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", "쉽게 볼 수 있는 낡은 검 입니다.", "어디선가 사용됐던거 같은 도끼입니다.", "스파르타의 전사들이 사용했다는 전설의 창입니다." };
            int[] gold = { 1000, 2000, 3500, 600, 1500, 2000 };
            for (int i = 0; i < name.Length; i++)
            {
                Equipment equipment = new Equipment();
                equipment.name = name[i];
                equipment.statName = statName[i];
                equipment.stats = stat[i];
                equipment.explains = explain[i];
                equipment.gold = gold[i];
                shops.equips.Add(equipment);
            }
        }

        // 캐릭터 인터페이스
        public interface ICharacter
        {
            string name { get; set; }
            float hp { get; set; }
            int attack { get; set; }
            int defense { get; set; }
            bool isDead { get; set; }

            public void TakeDamage(int damage);
        }

        // 플레이어 클래스
        public class Player : ICharacter
        {
            public string name { get; set; }
            public int level { get; set; } = 1;
            public JOBENUM job { get; set; }
            public float hp { get; set; } = 100;
            public int attack { get; set; } = 10;
            public int defense { get; set; } = 5;
            public int gold { get; set; } = 1500;
            public bool isDead { get; set; }
            = false;

            public List<Equipment> equipments { get; set; }

            public void TakeDamage(int damage)
            {

            }
        }

        // 몬스터 클래스
        public class Monster : ICharacter
        {
            public string name { get; set; }
            public float hp { get; set; }
            public int attack { get; set; } = 0;

            public int defense { get; set; }
            public bool isDead { get; set; }
            = false;

            public void TakeDamage(int damage)
            {

            }
        }

        // 고블린 클래스
        public class Goblin : Monster
        {
        }

        // 드래곤 클래스
        public class Dragon : Monster
        {
        }

        // 아이템 인터페이스
        public interface IItem
        {
            string name { get; set; }
            public void Use(Player player);
        }

        // HP 포션 클래스
        public class HealthPotion : IItem
        {
            public string name { get; set; }
            public void Use(Player player)
            {
            }
        }

        // 강화 포션 클래스
        public class StrengthPotion : IItem
        {
            public string name { get; set; }
            public void Use(Player player)
            {
            }
        }

        public class Equipment : IItem
        {
            public string name { get; set; }

            public string statName { get; set; }
            public int stats { get; set; }
            public string explains { get; set; }

            public int gold { get; set; }

            public bool isOn { get; set; } = false;

            public bool isBuyed { get; set; } = false;
            public void Use(Player player)
            {

            }
        }

        public class Shops
        {
            public List<Equipment> equips { get; set; } = new List<Equipment>();
            public List<HealthPotion> hpPotions { get; set; }
            public List<StrengthPotion> strengthPotions { get; set; }
        }

        // 스테이지 클래스
        public class Stage
        {
            Player player;
            Monster monster;
            HealthPotion healthPotion;
            StrengthPotion strengthPotion;

            public void Start()
            {

            }
        }

        public enum JOBENUM
        {
            WARRIOR,
            THIEF,
            ARCHER,
            WIZARD,
        }
    }
}

using System.Threading;

namespace TextRpg
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"스파르타 던전에 오신걸 환영합니다!");
            // 플레이어 생성
            Player player = new Player();
            // 스테이지 생성
            Stage stage = new Stage();
            // 상점 생성
            Shops shops = new Shops();
            // 상점 물품 추가 
            InitShop(shops);
            // 플레이어 이름 직업 세팅
            SettingNameJob(player);

            // 게임 로직 반복문
            while (true)
            {
                // 던전 이전 활동 선택
                int beforeStage = StartScene();

                switch (beforeStage)
                {
                    case 1:
                        // 상태 보기
                        ViewState(player);
                        break;
                    case 2:
                        // 인벤토리
                        Inventory(player);
                        break;
                    case 3:
                        // 상점
                        ViewShop(player, shops);
                        break;
                }
            }
        }

        // 초기 이름 설정 
        public static void SettingNameJob(Player player)
        {
            Console.WriteLine("이름을 입력해주세요");
            // 이름 입력 받고 추가
            string name = Console.ReadLine();
            player.name = name;

            // 직업 추가 반복문
            while (true)
            {
                Console.WriteLine("원하시는 직업을 입력해주세요");
                Console.WriteLine("1. 전사");
                Console.WriteLine("2. 도적");
                Console.WriteLine("3. 궁수");
                Console.WriteLine("4. 마법사");

                string job = Console.ReadLine();
                // 1 - 전사
                if (job == "1" || job == "전사")
                {
                    player.job = JOBENUM.WARRIOR;
                    break;
                }
                // 2 - 도적
                else if (job == "2" || job == "도적")
                {
                    player.job = JOBENUM.THIEF;
                    break;
                }
                // 3 - 궁수
                else if (job == "3" || job == "궁수")
                {
                    player.job = JOBENUM.ARCHER;
                    break;
                }
                // 4 - 마법사
                else if (job == "4" || job == "마법사")
                {
                    player.job = JOBENUM.WIZARD;
                    break;
                }
                // 이 외 입력시 1-4 입력할 때까지 반복
                else
                {
                    Console.Clear();
                    Console.WriteLine("1부터 4중에서 입력해주세요");
                }
            }
            // 콘솔 초기화
            Console.Clear();
        }

        // 시작 씬
        public static int StartScene()
        {            
            string input;
            int intInput;

            // 던전 이전 활동 선택
            while (true)
            {
                Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
                Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.");
                Console.WriteLine("");
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

        // 상태 보기 
        public static void ViewState(Player player)
        {
            string input = "";
            int plusAttack = 0;
            int plusDefense = 0;

            if (player.isOnEquip != null) 
            {
                foreach (var equip in player.isOnEquip)
                {
                    if (equip.statName == "공격력")
                    {
                        plusAttack = equip.stats;
                    }
                    else
                    {
                        plusDefense = equip.stats;
                    }
                }
            }

            while (input != "0")
            {
                Console.WriteLine("상태 보기");
                Console.WriteLine("캐릭터의 정보가 표시됩니다.");
                Console.WriteLine($"Lv. {player.level}");
                Console.WriteLine($"{player.name} ( {player.job.ToString()} )");
                Console.WriteLine($"공격력 : {player.attack} {(plusAttack == 0 ? "" : $"+ {plusAttack}")}");
                Console.WriteLine($"방어력 : {player.defense} {(plusDefense == 0 ? "" : $"+ {plusDefense}")}");
                Console.WriteLine($"체  력 : {player.hp}");
                Console.WriteLine($"Gold   : {player.gold}");
                Console.WriteLine("");
                Console.WriteLine("0. 나가기");
                Console.WriteLine("");
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                input = Console.ReadLine();
                Console.Clear();
            }
        }

        // 인벤토리
        public static void Inventory(Player player)
        {
            string input = "";
            int intInput = 0;
            bool isPutOnSetting = false;
            while (true)
            {
                Console.Write("인벤토리");

                // 장착 관리 중  
                if (isPutOnSetting)
                {
                    Console.Write(" - 장착 관리");
                }
                Console.WriteLine();
                Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
                Console.WriteLine("");
                Console.WriteLine("[아이템 목록]");

                // 플레이어의 아이템이 있을 때만 출력
                if (player.equipments != null)
                {
                    for (int i = 0; i < player.equipments.Count; i++)
                    {
                        // 장착 관리 중에는 아이템 번호 표시 
                        if (isPutOnSetting)
                        {
                            Console.WriteLine($"- {i + 1} {(player.equipments[i].isOn ? "[E]" : "")}{player.equipments[i].name} | {player.equipments[i].statName} +{player.equipments[i].stats} | {player.equipments[i].explains}");
                        }
                        // 장착 관리 안할 때는 아이템 번호 삭제 
                        else
                        {
                            Console.WriteLine($"- {(player.equipments[i].isOn ? "[E]" : "")}{player.equipments[i].name} | {player.equipments[i].statName} +{player.equipments[i].stats} | {player.equipments[i].explains}");
                        }
                    }
                }

                // 장착 관리중 장착 관리 선택지 삭제 
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

                // 입력 후 나갈 수 있기 때문에 그 전에 콘솔 초기화 
                Console.Clear();

                // 장착 관리중일 때
                if (isPutOnSetting)
                {
                    // 0 - 장착 관리 해제
                    if (input == "0")
                    {
                        isPutOnSetting = false;
                    }
                    // 이외 숫자는 아이템 장착 
                    else if (int.TryParse(input, out intInput))
                    {
                        // null 방지 
                        if (player.equipments != null)
                        {
                            // 배열 범위에 있는 숫자인지 확인 
                            if (intInput > 0 && intInput <= player.equipments.Count)
                            {
                                // 아이템 장착 표시 
                                player.equipments[intInput - 1].isOn = !player.equipments[intInput - 1].isOn;

                                // 중복 계열 아이템 장착 방지
                                if (player.isOnEquip != null)
                                {
                                    // System.InvalidOperationException = 컬렉션 순회중 컬렉션 변경시 발생하는 에러
                                    // 때문에 리스트를 복사해서 순회하고 원본을 수정한다.
                                    List<Equipment> copyEquip = new List<Equipment>(player.isOnEquip);

                                    // 복사본 순회
                                    foreach (var equipment in copyEquip)
                                    {
                                        // 같은 계열 아이템이 장착되어 있을 때 
                                        if (equipment.statName == player.equipments[intInput - 1].statName)
                                        {
                                            // 장착되어 있는 아이템 해제 
                                            player.isOnEquip.Remove(equipment);
                                            int idx = player.equipments.IndexOf(equipment);
                                            player.equipments[idx].isOn = false;
                                        }
                                    }
                                }
                                player.isOnEquip.Add(player.equipments[intInput - 1]);
                            }
                        }
                    }                  
                    // 숫자 입력이 아닐때 
                    else
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                    }
                }
                // 장착 관리 중이 아닐때 
                else
                {
                    // 1 - 장착 관리
                    if (input == "1" || input == "장착 관리")
                    {
                        isPutOnSetting = true;
                    }
                    // 0 - 나가기 
                    else if (input == "0" || input == "나가기")
                    {
                        break;
                    }
                }
            }
        }

        // 상점 
        public static void ViewShop(Player player, Shops shops)
        {
            string input = "";
            int intInput = 0;
            bool isBuyItem = false;
            while (true)
            {
                Console.Write("상점");

                // 아이템 구매중
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
                    // 아이템 구매시 아이템 번호 생성
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

                // 아이템 구매 중 아이템 구매 선택지 삭제
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

                // 입력 후 나갈 수 있기 때문에 그 전에 콘솔 초기화 
                Console.Clear();

                // 아이템 구매중일 때
                if (isBuyItem)
                {
                    // 0 - 나가기 
                    if (input == "0" || input == "나가기")
                    {
                        isBuyItem = false;
                    }
                    // 숫자 입력인지 확인
                    else if (int.TryParse(input, out intInput))
                    {
                        if (input == "0")
                        {
                            isBuyItem = false;
                        }
                        // 물품 번호 입력이라면                       
                        else if (intInput <= shops.equips.Count && intInput > 0)
                        {
                            if (!shops.equips[intInput - 1].isBuyed)
                            {
                                if (shops.equips[intInput - 1].gold < player.gold)
                                {
                                    Console.WriteLine("구매를 완료했습니다");
                                    player.gold -= shops.equips[intInput - 1].gold;
                                    shops.equips[intInput - 1].isBuyed = true;
                                    player.equipments.Add(shops.equips[intInput - 1]);
                                }
                                else
                                {
                                    Console.WriteLine("Gold가 부족합니다.");
                                }
                            }
                        }
                        // 물품 번호 입력이 아니면
                        else
                        {
                            Console.WriteLine("잘못된 입력입니다.");
                        }
                    }                    
                    // 숫자 입력이 아니면
                    else
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                    }
                }
                // 아이템 구매중이 아닐 때
                else
                {
                    // 1 - 아이템 구매하기 
                    if (input == "1" || input == "아이템 구매")
                    {
                        isBuyItem = true;
                    }
                    // 0 - 나가기 
                    else if (input == "0" || input == "나가기")
                    {
                        break;
                    }
                }
            }
        }

        // 아이템 항목 추가 
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
            // 기본 이름
            string name { get; set; }

            // 기본 체력
            float hp { get; set; }

            // 기본 공격력
            int attack { get; set; }

            // 기본 방어력
            int defense { get; set; }

            // 기본 생존 상태 
            bool isDead { get; set; }

            // 기본 데미지 받았을 때 함수
            public void TakeDamage(int damage);
        }

        // 플레이어 클래스
        public class Player : ICharacter
        {
            // 이름
            public string name { get; set; }

            // 레벨
            public int level { get; set; } = 1;

            // 직업
            public JOBENUM job { get; set; }

            // 체력
            public float hp { get; set; } = 100;

            // 공격력
            public int attack { get; set; } = 10;

            // 방어력
            public int defense { get; set; } = 5;

            // 골드
            public int gold { get; set; } = 150000;

            // 생존 상태
            public bool isDead { get; set; } = false;

            // 플레이어가 착용하고 있는 장비 목록 
            public List<Equipment> isOnEquip { get; set; } = new List<Equipment>();

            // 플레이어의 아이템 목록
            public List<Equipment> equipments { get; set; } = new List<Equipment>();

            // 데미지 받았을때 사용 함수
            public void TakeDamage(int damage)
            {

            }
        }

        // 몬스터 클래스
        public class Monster : ICharacter
        {
            // 이름
            public string name { get; set; }

            // 체력
            public float hp { get; set; }

            // 공격력
            public int attack { get; set; } = 0;

            // 방어력
            public int defense { get; set; }

            // 생존 상태 
            public bool isDead { get; set; } = false;

            // 데미지 받았을 때 사용 함수 
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
            // 아이템의 이름 
            string name { get; set; }

            // 아이템 사용 함수 
            public void Use(Player player);
        }

        // HP 포션 클래스
        public class HealthPotion : IItem
        {
            // 체력 포션의 이름
            public string name { get; set; }

            // 아이템 사용 함수 
            public void Use(Player player)
            {
            }
        }

        // 강화 포션 클래스
        public class StrengthPotion : IItem
        {
            // 강화 포션의 이름
            public string name { get; set; }

            // 아이템 사용 함수
            public void Use(Player player)
            {
            }
        }

        // 장비 아이템
        public class Equipment : IItem
        {
            // 이름 
            public string name { get; set; }

            // 스탯 이름
            public string statName { get; set; }

            // 스탯
            public int stats { get; set; }

            // 장비 설명
            public string explains { get; set; }

            // 장비 가격
            public int gold { get; set; }

            // 장비 착용 여부
            public bool isOn { get; set; } = false;

            // 장비 구매 여부 
            public bool isBuyed { get; set; } = false;

            // 장비 사용 함수
            public void Use(Player player)
            {

            }
        }

        // 상점 클래스
        public class Shops
        {
            // 상점 장비 아이템 리스트
            public List<Equipment> equips { get; set; } = new List<Equipment>();

            // 상점 체력 포션 리스트
            public List<HealthPotion> hpPotions { get; set; }

            // 상점 강화 포션 리스트
            public List<StrengthPotion> strengthPotions { get; set; }
        }

        // 스테이지 클래스
        public class Stage
        {
            // 플레이어
            Player player;

            // 몬스터
            Monster monster;

            // 게임 시작 함수
            public void Start()
            {

            }
        }

        // 직업 ENUM
        public enum JOBENUM
        {
            // 전사
            WARRIOR,

            // 도적
            THIEF,

            // 궁수
            ARCHER,

            // 마법사
            WIZARD,
        }
    }
}

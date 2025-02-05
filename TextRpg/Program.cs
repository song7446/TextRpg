using System.Threading;
using static TextRpg.Program;

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
            stage.player = player;
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
                    case 4:
                        // 던전 입장
                        stage.SelectLevel();
                        break;
                }
            }
        }

        // 게임 시작 함수
        public static void GameStart(Stage stage)
        {
            // 플레이어의 입력
            string input;

            // 싸우는 상태 
            bool isFight = false;

            // 방어 선택 상태
            bool doDefence = false;

            // 초기 플레이어의 체력 공격력 방어력
            // 포션을 먹으면 이 수치들이 바뀌기 때문에 던전에서 나가면 원래 수치로 돌려놓기 위함
            // 포션을 여러개 사용할 수 있고 사용한 포션도 삭제하기 때문에 일일이 변경된 수치 저장보다는 초기 수치를 저장하는게 더 낫다고 생각
            float firstHp = stage.player.hp;
            float firstAttack = stage.player.attack;
            float firstDefense = stage.player.defense;

            // 플레이어 추가 능력치 확인
            int plusAttack = 0;
            int plusDefense = 0;

            // 플레이어의 장비 확인
            if (stage.player.isOnEquip != null)
            {
                foreach (var equip in stage.player.isOnEquip)
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

            // 던전의 로직 
            while (true)
            {
                // 싸울 때 
                if (isFight)
                {
                    Console.WriteLine($"{stage.monster.name}과 싸웁니다.");
                    Console.WriteLine($"{stage.monster.name}의 HP : {stage.monster.hp}");
                    Console.WriteLine($"{stage.monster.name}의 공격력 : {stage.monster.attack}");
                    Console.WriteLine($"{stage.monster.name}의 방어력 : {stage.monster.defense}");
                    Console.WriteLine("");
                    Console.WriteLine($"{stage.player.name}의 HP : {stage.player.hp}");
                    Console.WriteLine($"{stage.player.name}의 공격력 : {stage.player.attack}");
                    Console.WriteLine($"{stage.player.name}의 방어력 : {stage.player.defense}");
                    Console.WriteLine("");
                    Console.WriteLine("1. 공격 하기");
                    Console.WriteLine("2. 방어 하기");
                    Console.WriteLine("3. 물약 사용하기");
                    Console.WriteLine("0. 도망 가기");
                    Console.WriteLine("원하시는 행동을 입력해주세요.");
                }
                // 싸우지 않을 때 
                else
                {
                    Console.WriteLine("던전에 들어왔습니다.");
                    Console.WriteLine("");
                    Console.WriteLine($"{stage.player.name}의 HP : {stage.player.hp}");
                    Console.WriteLine($"{stage.player.name}의 공격력 : {stage.player.attack}");
                    Console.WriteLine($"{stage.player.name}의 방어력 : {stage.player.defense}");
                    Console.WriteLine("");
                    Console.WriteLine("1. 몬스터와 싸우기");
                    Console.WriteLine("0. 던전 나가기");
                    Console.WriteLine("원하시는 행동을 입력해주세요.");
                }

                // 플레이어 턴
                input = Console.ReadLine();
                Console.Clear();

                // 싸울 때 
                if (isFight)
                {
                    // 방어 선택 초기화 
                    doDefence = false;

                    // 확률적으로 도망갈 수 없음
                    if (input == "0" || input == "도망 가기")
                    {
                        // 1 스테이지는 무조건 도망 
                        if (stage.level == 1)
                        {
                            isFight = false;
                            break;
                        }
                        // 2 스테이지는 50% 확률로 도망 
                        else if (stage.level == 2)
                        {
                            // 도망갈 확률 계산
                            Random random = new Random();
                            int runPos = random.Next(0, 101);

                            // 50이상이면 도망
                            if (runPos > 49)
                            {
                                isFight = false;
                                break;
                            }
                            // 49 이하면 도망 불가 
                            else
                            {
                                Console.WriteLine("도망갈 수 없습니다.");
                            }
                        }
                        // 3 스테이지는 도망 불가
                        else
                        {
                            Console.WriteLine("도망갈 수 없습니다.");
                        }
                    }
                    // 공격하기 
                    else if (input == "1" || input == "공격 하기")
                    {
                        // 몬스터 hp 계산 함수 
                        stage.monster.TakeDamage(stage.player.attack + plusAttack);

                        // 플레이어의 공격력보다 몬스터 방어력이 높으면 마이너스 혹은 0이 되기 때문에 최소 0.5의 데미지를 받도록 계산 
                        float minusHp = 0;
                        if (stage.player.attack + plusAttack - stage.monster.defense <= 0)
                        {
                            minusHp = 0.5f;
                        }
                        else
                        {
                            minusHp = stage.player.attack + plusAttack - stage.monster.defense;
                        }

                        Console.WriteLine("공격이 적중했습니다.");
                        Console.WriteLine($"{stage.monster.name}이(가) 데미지를 받습니다.");
                        Console.WriteLine($"{stage.monster.name}의 체력이 {minusHp}만큼 감소합니다.");
                        Console.WriteLine($"{stage.monster.name}의 체력이 {stage.monster.hp}이(가) 되었습니다.");

                        // 몬스터 사망 확인
                        if (stage.monster.isDead)
                        {
                            Console.WriteLine($"{stage.monster.name}이(가) 죽었습니다.");
                            Console.WriteLine("다음으로 넘어가려면 아무 키나 입력하세요");
                            input = Console.ReadLine();
                            isFight = false;
                            Console.Clear();
                            break;
                        }
                    }

                    // 방어하기 - 방어력 증가 및 한턴 데미지 방어 
                    else if (input == "2" || input == "방어 하기")
                    {
                        Console.WriteLine("방어를 합니다.");
                        Console.WriteLine("방어력이 1 상승 합니다.");
                        doDefence = true;
                    }

                    // 물약 사용 하기 
                    else if (input == "3" || input == "물약 사용하기")
                    {
                        Console.WriteLine("사용할 물약을 선택합니다");

                        // 3 스테이지는 물약 사용 불가 - 턴 날리기 (스테이지 선택때 알려줬기 때문)
                        if (stage.level == 3)
                        {
                            Console.WriteLine("어려움 난이도에서는 물약을 사용할 수 없습니다.");
                            Console.WriteLine("턴을 종료합니다");
                        }

                        // 다른 스테이지에서의 물약 사용 
                        else
                        {
                            // 물약이 있다면 
                            if (stage.player.potion.Count > 0)
                            {
                                // 소지중인 물약 표시 
                                for (int i = 0; i < stage.player.potion.Count; i++)
                                {
                                    Console.WriteLine($"- {i + 1} {stage.player.potion[i].name} | {stage.player.potion[i].statName} +{stage.player.potion[i].stats} | {stage.player.potion[i].explains}");
                                }

                                // 사용할 물약 선택 
                                input = Console.ReadLine();

                                // 숫자 입력 확인 
                                if (int.TryParse(input, out int intInput))
                                {
                                    // 물약 선택시 
                                    if (intInput > 0 && intInput <= stage.player.potion.Count)
                                    {
                                        Potion selectedPotion = stage.player.potion[intInput - 1];

                                        // 체력 물약 로직 
                                        if (selectedPotion.statName == "체력")
                                        {
                                            stage.player.hp += selectedPotion.stats;
                                            Console.WriteLine($"{selectedPotion.name}을(를) 사용합니다.");
                                            Console.WriteLine($"당신의 HP가 {selectedPotion.stats}만큼 상승합니다.");
                                            Console.WriteLine($"당신의 HP는 {stage.player.hp}이(가) 되었습니다.");
                                        }

                                        // 공격력 증가 물약 로직 
                                        else if (stage.player.potion[intInput - 1].statName == "공격력")
                                        {
                                            stage.player.attack += (int)selectedPotion.stats;
                                            Console.WriteLine($"{selectedPotion.name}을(를) 사용합니다.");
                                            Console.WriteLine($"당신의 공격력이 {selectedPotion.stats}만큼 상승합니다.");
                                            Console.WriteLine($"당신의 공력력은 {stage.player.attack}이(가) 되었습니다.");
                                        }

                                        // 방여력 증가 물약 로직 
                                        else
                                        {
                                            stage.player.defense += (int)selectedPotion.stats;
                                            Console.WriteLine($"{selectedPotion.name}을(를) 사용합니다.");
                                            Console.WriteLine($"당신의 방어력이 {selectedPotion.stats}만큼 상승합니다.");
                                            Console.WriteLine($"당신의 방어력은 {stage.player.defense}이(가) 되었습니다.");
                                        }

                                        // 물약 사용 후 포션 삭제 
                                        stage.player.potion.Remove(selectedPotion);
                                    }
                                    // 포션이 있는 숫자가 아닌 다른 숫자 입력시 
                                    else
                                    {
                                        Console.WriteLine("잘못된 입력입니다. 턴을 종료합니다.");
                                    }
                                }
                                // 숫자가 아닌 문자 입력 
                                else
                                {
                                    Console.WriteLine("잘못된 입력입니다. 턴을 종료합니다.");
                                }
                            }
                            // 물약이 없을 때 
                            // 그냥 턴을 날리는 이유는 전투에서 실수를 봐줄 수 없기 때문 
                            else
                            {
                                Console.WriteLine("물약이 없습니다. 턴을 종료합니다.");
                            }
                        }
                    }

                    Console.WriteLine();

                    // 몬스터의 턴 
                    Console.WriteLine($"{stage.monster.name}이(가) 공격합니다.");

                    // 플레이어가 방어를 선택했다면 데미지를 받지 않음 
                    if (doDefence)
                    {
                        Console.WriteLine("당신은 데미지를 받지 않습니다.");
                    }

                    // 방어 선택이 아니면 데미지를 받음
                    else
                    {
                        // 플레이어 HP 계산 함수
                        stage.player.TakeDamage(stage.monster.attack);

                        // 몬스터의 공격력보다 플레이어의 방어력이 높으면 마이너스 혹은 0이 되기 때문에 최소 0.5의 데미지를 받도록 계산 
                        float minusHp = 0;
                        if (stage.monster.attack - stage.player.defense <= 0)
                        {
                            minusHp = 0.5f;
                        }
                        else
                        {
                            minusHp = stage.monster.attack - stage.player.defense;
                        }

                        Console.WriteLine($"당신은 {minusHp}만큼 데미지를 받습니다.");
                        Console.WriteLine($"당신의 체력은 {stage.player.hp}이(가) 되었습니다.");

                        // 플레이어 사망 확인
                        if (stage.player.isDead)
                        {
                            Console.WriteLine($"{stage.player.name}이(가) 죽었습니다.");
                            isFight = false;
                            break;
                        }
                    }

                    Console.WriteLine();
                    // 대기용 입력 
                    Console.WriteLine("다음으로 넘어가려면 아무 키나 입력하세요");
                    input = Console.ReadLine();
                    
                    // 텍스트 정리 
                    Console.Clear();
                }
                // 싸우지 않을 때 
                else
                {
                    if (input == "0" || input == "던전 나가기")
                    {
                        break;
                    }
                    if (input == "1" || input == "몬스터와 싸우기")
                    {
                        isFight = true;
                    }
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
                Console.WriteLine("4. 던전 입장");
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                input = Console.ReadLine();
                if (input == "1" || input == "2" || input == "3" || input == "4")
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

            // 추가 공격력
            int plusAttack = 0;

            // 추가 방어력
            int plusDefense = 0;

            // 플레이어의 장비 확인 
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

                Console.WriteLine();
                if (player.potion != null)
                {
                    for (int i = 0; i < player.potion.Count; i++)
                    {
                        Console.WriteLine($"- {player.potion[i].name} | {player.potion[i].statName} +{player.potion[i].stats} | {player.potion[i].explains}");
                    }
                }
                Console.WriteLine();

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
                           $"| {(shops.equips[i].isBuyed ? "구매완료" : shops.equips[i].gold + "G")}");
                    }
                    else
                    {

                        Console.WriteLine($"- {shops.equips[i].name} | {shops.equips[i].statName} +{shops.equips[i].stats} | {shops.equips[i].explains} " +
                            $"| {(shops.equips[i].isBuyed ? "구매완료" : shops.equips[i].gold + "G")}");
                    }
                }
                Console.WriteLine();

                for (int i = 0; i < shops.potions.Count; i++)
                {
                    // 아이템 구매시 아이템 번호 생성
                    if (isBuyItem)
                    {
                        Console.WriteLine($"- {i + shops.equips.Count + 1} {shops.potions[i].name} | {shops.potions[i].statName} +{shops.potions[i].stats} | {shops.potions[i].explains} " + $"| {shops.potions[i].gold} G");
                    }
                    else
                    {
                        Console.WriteLine($"- {shops.potions[i].name} | {shops.potions[i].statName} +{shops.potions[i].stats} | {shops.potions[i].explains} " + $"| {shops.potions[i].gold} G");
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
                        // 장비 구매 
                        else if (intInput <= shops.equips.Count && intInput > 0)
                        {
                            // 구매된 물품이 아니라면
                            if (!shops.equips[intInput - 1].isBuyed)
                            {
                                // 플레이어가 물품을 살 돈이 있다면 
                                if (shops.equips[intInput - 1].gold < player.gold)
                                {
                                    // 플레이어의 골드 차감
                                    player.gold -= shops.equips[intInput - 1].gold;
                                    // 상점 물품 구매상태로 변경
                                    shops.equips[intInput - 1].isBuyed = true;
                                    // 플레이어의 장비에 추가 
                                    player.equipments.Add(shops.equips[intInput - 1]);
                                    Console.WriteLine("구매를 완료했습니다");
                                }
                                // 플레이어가 물품을 살 돈이 없다면 
                                else
                                {
                                    Console.WriteLine("Gold가 부족합니다.");
                                }
                            }
                        }
                        // 물약 구매 
                        else if (intInput > shops.equips.Count && intInput <= shops.equips.Count + shops.potions.Count)
                        {
                            // 플레이어가 물품을 살 돈이 있다면 
                            if (shops.potions[intInput - shops.equips.Count - 1].gold < player.gold)
                            {
                                // 플레이어의 골드 차감
                                player.gold -= shops.potions[intInput - shops.equips.Count - 1].gold;
                                // 플레이어의 포션에 추가 
                                player.potion.Add(shops.potions[intInput - shops.equips.Count - 1]);
                                Console.WriteLine("구매를 완료했습니다");
                            }
                            // 플레이어가 물품을 살 돈이 없다면 
                            else
                            {
                                Console.WriteLine("Gold가 부족합니다.");
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

        // 상점 아이템 항목 추가 
        public static void InitShop(Shops shops)
        {
            string[] name = { "수련자 갑옷", "무쇠갑옷", "스파르타의 갑옷", "낡은 검", "청동 도끼 ", "스파르타의 창" };
            string[] statName = { "방어력", "방어력", "방어력", "공격력", "공격력", "공격력" };
            int[] stat = { 5, 9, 15, 2, 5, 7 };
            string[] explain = { "수련에 도움을 주는 갑옷입니다.", "무쇠로 만들어져 튼튼한 갑옷입니다.", "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", "쉽게 볼 수 있는 낡은 검 입니다.", "어디선가 사용됐던거 같은 도끼입니다.", "스파르타의 전사들이 사용했다는 전설의 창입니다." };
            int[] gold = { 1000, 2000, 3500, 600, 1500, 2000 };

            string[] potionName = { "체력 물약", "공격력 증가 물약", "방어력 증가 물약" };
            string[] potionStatName = { "체력", "공격력", " 방어력" };
            int[] potionStat = { 3, 3, 3 };
            string[] potionExplain = { "체력을 회복시켜줍니다.", "공격력을 일시적으로 회복시켜줍니다", "방어력을 일시적으로 회복시켜줍니다." };
            int[] potionGold = { 300, 500, 500 };

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

            for (int i = 0; i < potionName.Length; i++)
            {
                Potion potion = new Potion();
                potion.name = potionName[i];
                potion.statName = potionStatName[i];
                potion.stats = potionStat[i];
                potion.explains = potionExplain[i];
                potion.gold = gold[i];
                shops.potions.Add(potion);
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

            // 플레이어의 포션 목록
            public List<Potion> potion { get; set; } = new List<Potion>();

            // 데미지 받았을때 사용 함수
            public void TakeDamage(int damage)
            {
                float minusHp = 0;

                if (damage - defense <= 0)
                {
                    minusHp = 0.5f;
                }
                else
                {
                    minusHp = damage - defense;
                }

                hp -= minusHp;

                if (hp < 0)
                {
                    hp = 0;
                    isDead = true;
                }
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
                float minusHp = 0;

                if (damage - defense <= 0)
                {
                    minusHp = 0.5f;
                }
                else
                {
                    minusHp = damage - defense;
                }

                hp -= minusHp;

                if (hp < 0)
                {
                    hp = 0;
                    isDead = true;
                }
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

            // 스탯 이름
            public string statName { get; set; }

            // 스탯
            public int stats { get; set; }

            // 장비 설명
            public string explains { get; set; }

            // 장비 가격
            public int gold { get; set; }
        }

        // 포션 클래스
        public class Potion : IItem
        {
            // 포션의 이름
            public string name { get; set; }

            // 포션의 스탯이름
            public string statName { get; set; }

            // 포션의 스탯
            public int stats { get; set; }

            // 포션 설명
            public string explains { get; set; }

            // 포션 가격
            public int gold { get; set; }
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
        }

        // 상점 클래스
        public class Shops
        {
            // 상점 장비 아이템 리스트
            public List<Equipment> equips { get; set; } = new List<Equipment>();

            // 상점 포션 리스트
            public List<Potion> potions { get; set; } = new List<Potion> { };

        }

        // 스테이지 클래스
        public class Stage
        {
            // 플레이어
            public Player player { get; set; }

            // 몬스터
            public Monster monster { get; set; }

            public int level { get; set; }

            // 레벨 선택 함수
            public void SelectLevel()
            {
                string input;
                while (true)
                {
                    Console.WriteLine("레벨 선택");
                    Console.WriteLine("");
                    Console.WriteLine("1. 쉬움 - 물약 사용 가능 몬스터 체력 공격력 낮음 100%로 도망가기 가능");
                    Console.WriteLine("2. 보통 - 물약 사용 가능 몬스터 체력 공격력 보통 50%로 도망가기 가능");
                    Console.WriteLine("3. 어려움 - 물약 사용 불가능 몬스터 체력 공력력 높음 도망가기 불가");
                    Console.WriteLine("원하시는 레벨을 입력해주세요.");
                    input = Console.ReadLine();
                    Console.Clear();
                    if (input == "1" || input == "쉬움")
                    {
                        level = 1;
                        break;
                    }
                    else if (input == "2" || input == "보통")
                    {
                        level = 2;
                        break;
                    }
                    else if (input == "3" || input == "어려움")
                    {
                        level = 3;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                    }
                }
                LevelSetting();
                GameStart(this);
            }

            // 레벨 세팅 함수 
            public void LevelSetting()
            {
                switch (level)
                {
                    case 1:
                        monster = new Goblin();
                        monster.name = "고블린";
                        monster.hp = 10;
                        monster.attack = 1;
                        monster.defense = 0;
                        break;
                    case 2:
                        monster = new Goblin();
                        monster.name = "고블린";
                        monster.hp = 50;
                        monster.attack = 5;
                        monster.defense = 1;
                        break;
                    case 3:
                        monster = new Dragon();
                        monster.name = "드래곤";
                        monster.hp = 100;
                        monster.attack = 10;
                        monster.defense = 5;
                        break;
                }
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

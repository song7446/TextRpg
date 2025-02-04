namespace TextRpg
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string name = "";

            Console.WriteLine($"스파르타 던전에 오신걸 환영합니다!");
            name = SettingName();

            StartScene();
        }

        public static string SettingName()
        {
            Console.WriteLine("이름을 입력해주세요");
            string name = Console.ReadLine();
            return name;
        }

        public static string StartScene()
        {
            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
            Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.");
            Console.WriteLine("1. 상태 보기");
            Console.WriteLine("2. 인벤토리");
            Console.WriteLine("3. 상점");
            Console.WriteLine("원하시는 행동을 입력해주세요.");
            string input = Console.ReadLine();

            return input;
        }
    }
}

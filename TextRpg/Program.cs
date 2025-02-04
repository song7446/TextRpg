namespace TextRpg
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"스파르타 던전에 오신걸 환영합니다!");
            Warrior player = new Warrior();
            player.name = SettingName();

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

        public interface ICharacter
        {
            string name { get; set; }
            float HP { get; set; }
            int Attack { get; set; }
            bool isDead { get; set; }

            public void TakeDamage(int damage);
        }

        public class Warrior : ICharacter 
        {
            public string name { get; set; }
            public float HP { get; set; }
            public int Attack { get; set; }
            = 0;
            public bool isDead { get; set; }
            = false;

            public void TakeDamage(int damage)
            {

            }
        }

        public class Monster : ICharacter
        {
            public string name { get; set; }
            public float HP { get; set; }
            public int Attack { get; set; }
            = 0;
            public bool isDead { get; set; }
            = false;

            public void TakeDamage(int damage)
            {

            }
        }

        public class Goblin : Monster 
        { 
        }

        public class Dragon : Monster
        {
        }

        public interface IItem
        {
            string name { get; set; }
            public void Use(Warrior warrior);
        }

        public class HealthPotion : IItem 
        {
            public string name { get; set; }
            public void Use(Warrior warrior) 
            { 
            }
        }

        public class StrengthPotion : IItem
        {
            public string name { get; set; }
            public void Use(Warrior warrior)
            {
            }
        }

        public class Stag
        { 

        }
    }
}

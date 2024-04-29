

namespace Chapter2_BY2
{
    //아이템 타입이 어떤것이 있는지 정의
    public enum ItemType
    {
        WEAPON,
        ARMOR
    }

    internal class Item
    {
        public string Name { get; }
        public string Desc { get; }
        private ItemType type;
        public int Atk { get; }
        public int Def { get; }
        public int Hp { get; }
        public int Price { get; }
        public bool isEquipped { get; private set; }
        public bool isPurchased { get; private set; }

        //isEquipped 와 is Purchased 를 기본적으로 false로 셋팅
        public Item(string name, string desc, ItemType type, int atk, int def, int hp, int price, bool isEquipped = false, bool isPurchased = false)
        {
            Name = name;
            Desc = desc;
            this.type = type;
            Atk = atk;
            Def = def;
            Hp = hp;
            Price = price;
            this.isEquipped = isEquipped;
            this.isPurchased = isPurchased;
        }

        //아이템 정보 보여줄때 타입이 비슷한것 2가지
        //1.인벤토리에서 그냥 내가 갖고있는 아이템 보여줄 때
        //2. 장착관리에서 내가 어떤 아이템을 낄지 말지 결정할 때
        internal void PrintItemStatDescription(bool withNumber = false, int idx = 0)
        {
            Console.Write("- ");
            if (withNumber) //withNumber 가 true 라면
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write($"{idx} ");
                Console.ResetColor();
            }
            if (isEquipped) // 장착이 되었다면
            {
                Console.Write("[");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("E");
                Console.ResetColor();
                Console.Write("]");
                //아이템 리스트 보여주기 + 줄맞추기
                Console.Write(ConsoleUtility.PadRightForMixedText(Name, 9));
            }
            //12개 칸 만큼 공간을 확보해달라!
            else Console.Write(ConsoleUtility.PadRightForMixedText(Name, 12));

            Console.Write(" | ");

            //공격력이 플러스면 + 표시
            if (Atk != 0) Console.Write($"공격력 {(Atk >= 0 ? "+" : "")}{Atk} ");
            if (Def != 0) Console.Write($"방어력 {(Atk >= 0 ? "+" : "")}{Def} ");
            if (Hp != 0) Console.Write($"체  력 {(Atk >= 0 ? "+" : "")}{Hp} ");

            Console.Write(" | ");
            Console.WriteLine(Desc);

        }

        public void PrintStoreItemDescription(bool withNumber = false, int idx = 0)
        {
            Console.Write("- ");
            if (withNumber) //withNumber 가 true 라면
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write($"{idx} ");
                Console.ResetColor();
            }
            else Console.Write(ConsoleUtility.PadRightForMixedText(Name, 12));

            Console.Write(" | ");

            //공격력이 플러스면 + 표시
            if (Atk != 0) Console.Write($"공격력 {(Atk >= 0 ? "+" : "")}{Atk} ");
            if (Def != 0) Console.Write($"방어력 {(Atk >= 0 ? "+" : "")}{Def} ");
            if (Hp != 0) Console.Write($"체  력 {(Atk >= 0 ? "+" : "")}{Hp} ");

            Console.Write(" | ");
            Console.Write(ConsoleUtility.PadRightForMixedText(Desc, 12));
            Console.Write(" | ");

            if (isPurchased)
            {
                Console.WriteLine("구매완료");
            }
            else
            {
                ConsoleUtility.PrintTextHighlights("", Price.ToString()," G");
            }

        }

        internal void ToggleEquipStatus()
        {
            isEquipped = !isEquipped;
        }

        internal void Purchase()
        {
            isPurchased = !isPurchased;
        }
    }
}
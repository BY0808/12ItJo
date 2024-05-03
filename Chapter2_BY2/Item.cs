

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
        public string Name { get; } // 이름
        public string Desc { get; } // 설명
        public ItemType Type { get; } // 아이템 타입
        public int Atk { get; } // 공격력
        public int Def { get; } // 방어력
        public int Hp { get; } // 체력
        public int Price { get; } // 가격
        public bool isEquipped { get; private set; } // 장착 여부
        public bool isPurchased { get; private set; } // 구매 여부

        //isEquipped 와 is Purchased 를 기본적으로 false로 셋팅
        public Item(string name, string desc, ItemType type, int atk, int def, int hp, int price, bool isEquipped = false, bool isPurchased = false)
        {
            Name = name;
            Desc = desc;
            Type = type;
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
        internal void PrintItemStatDescription(bool withNumber = false, int idx = 0) // 인벤토리에서 아이템을 표시하는 메서드
        {
            Console.Write("- ");
            if (withNumber) //withNumber 가 true 라면 (숫자랑 같이 출력)
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

        public void PrintStoreItemDescription(bool withNumber = false, int idx = 0) // 상점에서 아이템을 표시하는 메서드
        {
            Console.Write("- ");
            if (withNumber) //withNumber 가 true 라면
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write($"{idx} ");
                Console.ResetColor();
            }
            Console.Write(ConsoleUtility.PadRightForMixedText(Name, 15));

            Console.Write(" | ");

            string statStr = "";
            //공격력이 플러스면 + 표시
            if (Atk != 0) statStr = $" 공격력 {(Atk >= 0 ? "+" : "")}{Atk} ";
            if (Def != 0) statStr = $" 방어력 {(Atk >= 0 ? "+" : "")}{Def} ";
            if (Hp != 0) statStr = $" 체  력 {(Atk >= 0 ? "+" : "")}{Hp} ";
            Console.Write(ConsoleUtility.PadRightForMixedText(statStr, 13));

            Console.Write(" | ");
            Console.Write(ConsoleUtility.PadRightForMixedText(Desc, 13));
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

        internal void ToggleEquipStatus() // 장착 여부 반전시키는 메서드
        {
            isEquipped = !isEquipped;
        }

        internal void Purchase() // 구매 여부 확정시키는 메서드
        {
            isPurchased = !isPurchased;
        }
    }
}
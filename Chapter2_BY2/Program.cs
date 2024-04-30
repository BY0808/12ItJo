


namespace Chapter2_BY2
{
    public class GameManager
    {
        private Player player;
        private List<Item> inventory;

        private List<Item> storeInventory;

        List<Monster> monsters = new List<Monster>();

        int bonusAtk, bonusDef, bonusHp;

        private delegate void GameEvent(ICharacter character);
        private event GameEvent deathEvent;

        public GameManager() //클래스와 이름이 같은 함수, 생성자, 클래스 호출시 실행
        {
            InitializeGame();
        }

        private void InitializeGame()
        {
            //기본적인 초기화!
            player = new Player("BoB", "Huge", 1, 10, 5, 100, 2000);

            inventory = new List<Item>();

            storeInventory = new List<Item>();
            storeInventory.Add(new Item("수련자 갑옷", "적당한 갑옷", ItemType.ARMOR, 0, 5, 0, 1000));
            storeInventory.Add(new Item("무쇠갑옷", "조금좋은 갑옷", ItemType.ARMOR, 0, 9, 0, 1500));
            storeInventory.Add(new Item("스파르타 갑옷", "좋은 갑옷", ItemType.ARMOR, 0, 15, 0, 3500));
            storeInventory.Add(new Item("낡은 검", "적당한 무기", ItemType.WEAPON, 2, 0, 0, 600));
            storeInventory.Add(new Item("청동 도끼", "조금좋은 무기", ItemType.WEAPON, 5, 0, 0, 1500));
            storeInventory.Add(new Item("스파르타 창", "좋은 무기", ItemType.WEAPON, 7, 0, 0, 3500));

            deathEvent += RewardMenu;
        }

        //Program 이라는 다른 클래스 접근!
        public void StartGame()
        {
            //콘솔 게임은 콘솔 지워주는걸 지속해야함..!
            Console.Clear();
            //static 으로 정의된 함수라 인스턴스 없이 호출
            ConsoleUtility.PrintGameHeader();
            MainMenu();
        }

        private void MainMenu()
        {
            //구성
            //0. 화면 정리
            Console.Clear();

            //1. 선택 멘트 줌
            Console.WriteLine("--------------------------------------------------------");
            Console.WriteLine();
            Console.WriteLine("소환자의 마을에 오신것을 환영합니다.");
            Console.WriteLine("이곳에서 소환되기전 활동을 수행 할 수 있습니다.");
            Console.WriteLine();
            Console.WriteLine("--------------------------------------------------------");


            Console.WriteLine("1. 상태창");
            Console.WriteLine("2. 인벤");
            Console.WriteLine("3. 상점");
            Console.WriteLine("4. 던전");

            Console.WriteLine();

            //2. 선택 결과를 검증
            int choice = ConsoleUtility.PromptMenuChoice(1, 4);

            //3. 선택한 결과에 따라 보내줌
            switch (choice)
            {
                case 1:
                    StatusMenu();
                    break;
                case 2:
                    InventoryMenu();
                    break;
                case 3:
                    StoreMenu();
                    break;
                case 4:
                    DungeonMenu();
                    break;
            }
            MainMenu();
        }

        private void StatusMenu()
        {
            Console.Clear();
            //메뉴의 타이틀 만들기
            ConsoleUtility.ShowTitle("■ 상태보기 ■");
            Console.WriteLine("캐릭터의 정보가 표시됩니다.");

            ConsoleUtility.PrintTextHighlights("Lv. ", player.Level.ToString("00"));
            Console.WriteLine("");
            //문자열 보간
            //TODO : 능력치 강화분 표현하도록 변경
            Console.WriteLine($"{player.Name} ( {player.Job} )");

            //장착된 아이템 수치의 합 구하기
            bonusAtk = inventory.Select(item => item.isEquipped ? item.Atk : 0).Sum();
            bonusDef = inventory.Select(item => item.isEquipped ? item.Def : 0).Sum();
            bonusHp = inventory.Select(item => item.isEquipped ? item.Hp : 0).Sum();

            //보너스 어택이 0보다 크면 보여주고, 아니면 스킵
            ConsoleUtility.PrintTextHighlights("공격력 : ", (player.Atk + bonusAtk).ToString(), bonusAtk > 0 ? $" (+{bonusAtk})" : "");
            ConsoleUtility.PrintTextHighlights("방어력 : ", (player.Def + bonusDef).ToString(), bonusDef > 0 ? $" (+{bonusDef})" : "");
            ConsoleUtility.PrintTextHighlights("체  력 : ", (player.Hp + bonusHp).ToString(), bonusHp > 0 ? $" (+{bonusHp})" : "");

            /*
            ConsoleUtility.PrintTextHighlights("공격력 : ", player.Attack.ToString());
            ConsoleUtility.PrintTextHighlights("방어력 : ", player.Def.ToString());
            ConsoleUtility.PrintTextHighlights("체  력 : ", player.Hp.ToString());
            */
            ConsoleUtility.PrintTextHighlights(" Gold  : ", player.Gold.ToString());
            Console.WriteLine();
            Console.WriteLine("0. 나가기");
            Console.WriteLine();

            switch (ConsoleUtility.PromptMenuChoice(0))
            {
                case 0:
                    MainMenu();
                    break;
            }
        }

        private void InventoryMenu()
        {
            Console.Clear();

            ConsoleUtility.ShowTitle("■ 인벤토리 ■");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
            Console.WriteLine();
            Console.WriteLine("[아이템 목록]");
            Console.WriteLine();

            for (int i = 0; i < inventory.Count; i++)
            {
                inventory[i].PrintItemStatDescription();
            }
            Console.WriteLine();
            Console.WriteLine("1. 장착 관리");
            Console.WriteLine("0. 나가기");
            Console.WriteLine();

            switch (ConsoleUtility.PromptMenuChoice(0, 1))
            {
                case 0:
                    MainMenu();
                    break;
                case 1:
                    EquipMenu();
                    break;
            }
        }

        private void EquipMenu()
        {
            Console.Clear();

            ConsoleUtility.ShowTitle("■ 인벤토리 ■");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
            Console.WriteLine();
            Console.WriteLine("[아이템 목록]");
            Console.WriteLine();
            for (int i = 0; i < inventory.Count; i++)
            {
                inventory[i].PrintItemStatDescription(true, i + 1); // 나가기가 0번 고정, 나머지가 1번부터 배정
            }
            Console.WriteLine();
            Console.WriteLine("0. 나가기");

            int keyInput = ConsoleUtility.PromptMenuChoice(0, inventory.Count);

            switch (keyInput)
            {
                case 0:
                    InventoryMenu();
                    break;
                default:
                    //아이템 장착&해제 반복
                    inventory[keyInput - 1].ToggleEquipStatus();
                    EquipMenu();
                    break;
            }
        }

        private void StoreMenu()
        {
            Console.Clear();

            ConsoleUtility.ShowTitle("■ 상  점 ■");
            Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
            Console.WriteLine();
            Console.WriteLine("[보유 골드]");
            ConsoleUtility.PrintTextHighlights("", player.Gold.ToString(), " G");
            Console.WriteLine();
            Console.WriteLine("[아이템 목록]");
            for (int i = 0; i < storeInventory.Count; i++)
            {
                storeInventory[i].PrintStoreItemDescription();
            }
            Console.WriteLine();
            Console.WriteLine("1. 아이템 구매");
            Console.WriteLine("0. 나가기");
            Console.WriteLine();
            switch (ConsoleUtility.PromptMenuChoice(0, 1))
            {
                case 0:
                    MainMenu();
                    break;
                case 1:
                    PurchaseMenu();
                    break;
            }
        }

        private void PurchaseMenu(string? prompt = null)
        {
            //경고 메세지 띄우기
            if (prompt != null)
            {
                //1초간 메세지를 띄운 다음에 다시 진행
                Console.Clear();
                ConsoleUtility.ShowTitle(prompt);
                Thread.Sleep(1000); //몇 밀리 세컨드 동안 멈출 것인지
            }

            Console.Clear();

            ConsoleUtility.ShowTitle("■ 상  점 ■");
            Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
            Console.WriteLine();
            Console.WriteLine("[보유 골드]");
            ConsoleUtility.PrintTextHighlights("", player.Gold.ToString(), " G");
            Console.WriteLine();
            Console.WriteLine("[아이템 목록]");
            for (int i = 0; i < storeInventory.Count; i++)
            {
                storeInventory[i].PrintStoreItemDescription(true, i + 1);
            }
            Console.WriteLine();
            Console.WriteLine("0. 나가기");
            Console.WriteLine();

            int keyInput = ConsoleUtility.PromptMenuChoice(0, storeInventory.Count);

            switch (keyInput)
            {
                case 0:
                    StoreMenu();
                    break;
                default:
                    //1. 이미 구매한 경우
                    if (storeInventory[keyInput - 1].isPurchased) // index 맞추기
                    {
                        PurchaseMenu("이미 구매한 아이템 입니다.");
                    }
                    //2. 돈이 충분해서 살 수 있는 경우
                    else if (player.Gold >= storeInventory[keyInput - 1].Price)
                    {
                        player.Gold -= storeInventory[keyInput - 1].Price;
                        storeInventory[keyInput - 1].Purchase();
                        inventory.Add(storeInventory[keyInput - 1]);
                        PurchaseMenu();
                    }
                    //3. 돈이 모자라는 경우
                    else
                    {
                        PurchaseMenu("Gold가 부족합니다.");
                    }
                    break;
            }
        }

        private void DungeonMenu() // 던전 화면
        {
            Console.Clear();

            ConsoleUtility.ShowTitle("■ 던 전 ■");
            Console.WriteLine();
            player.Hp = 100;
            if (monsters.Count <= 0)
            {
                int monsterCount = new Random().Next(1, 5);
                for (int i = 0; i < monsterCount; i++)
                {
                    MonsterType monsterType = (MonsterType)(new Random().Next(1, 4));
                    switch (monsterType)
                    {
                        case MonsterType.Minion:
                            Minion minion = new Minion();
                            monsters.Add(minion);
                            break;
                        case MonsterType.Goblin:
                            Goblin goblin = new Goblin();
                            monsters.Add(goblin);
                            break;
                        case MonsterType.Dragon:
                            Dragon dragon = new Dragon();
                            monsters.Add(dragon);
                            break;
                    }
                }
            }
            foreach (Monster mon in monsters)
            {
                string isDeadStr = mon.IsDead ? "사망" : "";
                Console.Write(ConsoleUtility.PadRightForMixedText($"Lv {mon.Level} {mon.Name} |  HP {mon.Hp} ", 21));
                Console.Write(ConsoleUtility.PadRightForMixedText($"| Atk : {mon.Atk}", 11));
                Console.WriteLine($"| {isDeadStr}");
            }
            ConsoleUtility.PrintTextHighlights("\n", "[내정보]");
            Console.WriteLine($"Lv. {player.Level}  {player.Name} ({player.Job})");
            Console.WriteLine($"HP {player.Hp} / 100  Atk {player.Atk + bonusAtk}");

            Console.WriteLine("\n1. 공격\n");

            int keyInput = ConsoleUtility.PromptMenuChoice(1);
            FightMenu();
        }

        private void FightMenu() // 전투 선택 화면
        {
            Console.Clear();

            ConsoleUtility.ShowTitle("■ 던 전 ■");
            Console.WriteLine();

            for (int i = 0; i < monsters.Count; i++)
            {
                string isDeadStr = monsters.ToArray()[i].IsDead ? "사망" : "";
                Console.Write(ConsoleUtility.PadRightForMixedText($"{i + 1}- Lv {monsters.ToArray()[i].Level} {monsters.ToArray()[i].Name} |  HP {monsters.ToArray()[i].Hp} ", 24));
                Console.Write(ConsoleUtility.PadRightForMixedText($"| Atk : {monsters.ToArray()[i].Atk}", 11));
                Console.WriteLine($"| {isDeadStr}");
            }
            ConsoleUtility.PrintTextHighlights("\n", "[내정보]");
            Console.WriteLine($"Lv. {player.Level}  {player.Name} ({player.Job})");
            Console.WriteLine($"HP {player.Hp} / 100  Atk {player.Atk + bonusAtk}");

            string selectMaxStr = monsters.Count == 1 ? "" : $" ~ {monsters.Count}.";
            Console.WriteLine($"\n1.{selectMaxStr}  몬스터 선택");
            Console.WriteLine("0. 취소");

            int keyInput = ConsoleUtility.PromptMenuChoice(0, monsters.Count);
            while (true)
            {
                if (keyInput == 0)
                {
                    DungeonMenu();
                }
                else if (monsters.ToArray()[keyInput - 1].IsDead)
                {
                    Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요");
                    keyInput = ConsoleUtility.PromptMenuChoice(0, monsters.Count);
                }
                else BattleMenu(monsters.ToArray()[keyInput - 1]);
            }
        }
        private void BattleMenu(ICharacter character) //전투 화면
        {
            int keyInput;

            ConsoleUtility.ShowTitle("■ Battle!! ■");
            int currentDamage;
            if (character is Monster) // 입력값이 Monster인 경우 (몬스터를 공격한다.)
            {
                int deadCount = 0; // 죽은 몬스터 수
                Console.Clear();

                ICharacter opposite = player; // 상대방 : 플레이어
                currentDamage = opposite.Atk + bonusAtk + (new Random().Next(-1, 2) * (int)Math.Ceiling((opposite.Atk + bonusAtk) * 0.1f));
                Console.WriteLine($"{opposite.Name}의 공격!");
                Console.WriteLine($"Lv {character.Level} {character.Name} 을(를) 맞췄습니다. [데미지 : {currentDamage}]");
                Console.WriteLine();
                Console.WriteLine($"Lv {character.Level} {character.Name}");
                Console.Write($"HP {character.Hp}");
                character.TakeDamage(currentDamage);
                Console.WriteLine($" -> {character.Hp}");
                Console.WriteLine();
                Console.WriteLine("0. 다음");
                keyInput = ConsoleUtility.PromptMenuChoice(0);
                foreach (Monster mon in monsters) // 모든 몬스터 무리를 스캔
                {
                    if (mon.IsDead) deadCount++; // 현재 커서의 몬스터가 죽어있는 경우 카운트
                }
                if (deadCount == monsters.Count) deathEvent?.Invoke(character); // 현재 죽어있는 몬스터의 수가 몬스터 무리의 전체의 수와 같은가? (다 죽었나?)
                BattleMenu(player);
            }
            else // 입력값이 Player인 경우 (플레이어를 공격한다.)
            {

                ICharacter[] opposites = monsters.ToArray(); // 상대방 : 몬스터 무리
                foreach (ICharacter opposite in opposites) // 모든 몬스터 무리가
                {
                    Console.Clear();

                    if (opposite.IsDead) continue; // 현재 커서의 몬스터가 죽어있는 경우 아래 코드 무시 후 재진입
                    currentDamage = opposite.Atk + (new Random().Next(-1, 2) * (int)Math.Ceiling(opposite.Atk * 0.1f));
                    Console.WriteLine($"Lv {opposite.Level} {opposite.Name}의 공격!");
                    Console.WriteLine($"{character.Name} 을(를) 맞췄습니다. [데미지 : {currentDamage}]");
                    Console.WriteLine();
                    Console.WriteLine($"Lv {character.Level} {character.Name}");
                    Console.Write($"HP {character.Hp}");
                    character.TakeDamage(currentDamage);
                    Console.WriteLine($" -> {character.Hp}");
                    Console.WriteLine();
                    Console.WriteLine("0. 다음");
                    keyInput = ConsoleUtility.PromptMenuChoice(0);
                    if (character.IsDead) deathEvent?.Invoke(character); // 플레이어 죽은 경우 이벤스 실행
                }
                FightMenu();
            }
        }

        private void RewardMenu(ICharacter character)
        {
            if (character is Player) Console.WriteLine("너희들이 나를 죽였다."); // 패배시
            else Console.WriteLine("내가 너희들를 죽였다."); // 승리시
        }
    }

    public class Program
    {
        static void Main(string[] args)
        {
            //게임매니저를 만들고
            GameManager gameManager = new GameManager();
            //게임매니저를 호출
            gameManager.StartGame();
        }
    }
}
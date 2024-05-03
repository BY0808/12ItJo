


using System.Xml.Linq;

namespace Chapter2_BY2
{
    public class GameManager
    {
        /// <summary>
        /// 플레이어 객체
        /// </summary>
        private Player player;
        /// <summary>
        /// 인벤토리 리스트
        /// </summary>
        private List<Item> inventory;
        /// <summary>
        /// 상점 품목 리스트
        /// </summary>
        private List<Item> storeInventory;
        /// <summary>
        /// 던전에서 표시할 몬스터 리스트
        /// </summary>
        List<Monster> monsters = new List<Monster>();
        /// <summary>
        /// 공격 순서를 제어할 몬스터 리스트
        /// </summary>
        List<Monster> fieldMonster = new List<Monster>();

        /// <summary>
        /// 스테이지별 미니언 생성 확률 { 70, 40, 30, 0 }
        /// </summary>
        int[] minionSpawnRate = { 70, 40, 30, 0 };
        /// <summary>
        /// 스테이지별 고블린 생성 확률 { 30, 60, 30, 50 }
        /// </summary>
        int[] goblinSpawnRate = { 30, 60, 30, 50 };
        /// <summary>
        /// 스테이지 별 드래곤 생성 확률 {0, 0, 40, 50}
        /// </summary>
        int[] dragonSpawnRate = { 0, 0, 40, 50 };
        /// <summary>
        /// 스테이지 별 몬스터 최대 생성 수
        /// </summary>
        int[] monstersSpawnMax = { 2, 3, 4, 3 };

        /// <summary>
        /// 몬스터 처치시 해당 몬스터의 레벨만큼 경험치로 누적시킬 변수
        /// </summary>
        int tempGetExp; // 몬스터 처치시 해당 몬스터의 레벨 만큼 누적

        /// <summary>
        /// 플레이어, 몬스터에 공통적으로 부여할 델리게이트
        /// </summary>
        /// <param name="character"></param>
        private delegate void GameEvent(ICharacter character); // GameEvent 대리자 (함수를 담을 변수) 
        /// <summary>
        /// 플레이어, 몬스터에 공통적으로 부여할 이벤트
        /// </summary>
        private event GameEvent deathEvent; // GameEvent 형식의 event 대리자

        /// <summary>
        /// 스테이지 진입시 현재 플레이어의 체력을 저장할 변수
        /// </summary>
        int currentHp;
        /// <summary>
        /// 입력받은 정수 값을 가지고 있는 변수
        /// </summary>
        int keyInput;

        /// <summary>
        /// 파일 이름까지 포함한 세이브 파일 경로
        /// </summary>
        public string filePath;
        /// <summary>
        /// 게임 시작시 입력받을 플레이어의 이름을 저장할 변수
        /// </summary>
        public string playerName;

        /// <summary>
        /// GameManager 생성자
        /// </summary>
        public GameManager() //클래스와 이름이 같은 함수, 생성자, 클래스 호출시 실행
        {
            InitializeGame();
        }

        /// <summary>
        /// 게임을 시작하기 전 데이터를 세팅해주는 메서드
        /// </summary>
        private void InitializeGame()
        {
            //기본적인 초기화!
            inventory = new List<Item>(); // 인벤토리 객체 생성

            storeInventory = new List<Item>(); // 상점 품목 리스트 생성 & 리스트 추가
            storeInventory.Add(new Item("수련자 갑옷", "적당한 갑옷", ItemType.ARMOR, 0, 5, 0, 1000));
            storeInventory.Add(new Item("무쇠갑옷", "조금좋은 갑옷", ItemType.ARMOR, 0, 9, 0, 1500));
            storeInventory.Add(new Item("스파르타 갑옷", "좋은 갑옷", ItemType.ARMOR, 0, 15, 0, 3500));
            storeInventory.Add(new Item("낡은 검", "적당한 무기", ItemType.WEAPON, 2, 0, 0, 600));
            storeInventory.Add(new Item("청동 도끼", "조금좋은 무기", ItemType.WEAPON, 5, 0, 0, 1500));
            storeInventory.Add(new Item("스파르타 창", "좋은 무기", ItemType.WEAPON, 7, 0, 0, 3500));
            
            deathEvent += RewardMenu; // 이벤트 메서드에 해당 메서드 추가
        }

        //Program 이라는 다른 클래스 접근!
        /// <summary>
        /// 게임을 시작하는 메서드
        /// </summary>
        public void StartGame()
        {
            //콘솔 게임은 콘솔 지워주는걸 지속해야함..!
            Console.Clear();
            //static 으로 정의된 함수라 인스턴스 없이 호출
            ConsoleUtility.PrintGameHeader();
            InsertNameMenu();
        }
        /// <summary>
        /// 플레이어 이름을 입력받고 세이브 데이터를 로드 혹은 새 데이터를 생성하는 메서드
        /// </summary>
        public void InsertNameMenu()
        {
            Console.Clear();
            Console.WriteLine("스파르타 던전에 오신 여러분 환영합니다.");
            //파일 위치 찾기
            filePath = Path.Combine(Directory.GetCurrentDirectory(), "data", "data.json");
            //저장파일 불러오기
            Dictionary<string, SaveData> loadedData = SaveData.LoadDataFromJsonFile(filePath);

            //불러온 데이터가 있을 경우 저장된 데이터 이름 출력
            if (loadedData != null)
            {
                Console.WriteLine("저장된 이름을 출력합니다.");
                Console.WriteLine();
                foreach (string key in loadedData.Keys)
                {
                    Console.WriteLine($"{key}");
                }
            }
            Console.WriteLine();
            Console.WriteLine("원하시는 이름을 설정해주세요 >> ");
            do
            {
                playerName = Console.ReadLine(); // 이름 입력 창
                if (playerName == "") Console.WriteLine("다시 입력해주세요."); // 입력된 이름이 공백인 경우, 메시지 출력
            } while (playerName == ""); // 입력된 이름이 공백인 경우 계속 반복

            //이름을 입력 받은 후 파일을 불러와 비교함
            if (loadedData != null && loadedData.ContainsKey(playerName))
            {
                SaveData setData = loadedData[playerName];
                player = setData.savePlayer;
                inventory = setData.saveInventory;
                storeInventory = setData.saveStoreInventory;

                player.bonusAtk = setData.saveBonusAtk;
                player.bonusDef = setData.saveBonusDef;
                player.bonusHp = setData.saveBonusHp;
            }
            else
            {
                Console.WriteLine("직업을 선택해주세요.");

                Console.WriteLine("\n1. 전사\n2. 팔라딘\n");

                keyInput = ConsoleUtility.PromptMenuChoice((int)JobType.Warrior, (int)JobType.Paladin);
                switch (keyInput)
                {
                    case (int)JobType.Warrior:
                        player = new Player(playerName, JobType.Warrior, 1, 10, 5, 100, 2000); // 플레이어 객체 생성 & 초기화
                        break;
                    case (int)JobType.Paladin:
                        player = new Player(playerName, JobType.Paladin, 1, 6, 15, 150, 2000); // 플레이어 객체 생성 & 초기화
                        break;
                }
            }

            MainMenu();
        }
        /// <summary>
        /// 메인 메뉴를 표시하고, 주어진 입력에 맞게 화면 이동 & 게임 종료시 데이터를 저장하는 메서드
        /// </summary>
        private void MainMenu()
        {
            //구성
            //0. 메뉴 정리
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
            Console.WriteLine("0. 게임 끝내기");

            Console.WriteLine();

            //2. 선택 결과를 검증
            keyInput = ConsoleUtility.PromptMenuChoice(0, 4);

            //3. 선택한 결과에 따라 보내줌
            switch (keyInput)
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
                    DungeonEntranceMenu();
                    break;
                case 0:
                    Console.Clear();
                    ConsoleUtility.PrintGameEnd();
                    if (Console.ReadLine() == "0") //데이터 저장하기
                    {
                        //저장을 위한 딕셔너리 만들기
                        Dictionary<string, SaveData> playerDataDic = new Dictionary<string, SaveData>();
                        //저장을 위한 객체 생성
                        SaveData playerData = new SaveData(player, inventory, storeInventory, (int)player.bonusAtk, player.bonusDef, player.bonusHp);
                        //딕셔너리에 저장을 위한 데이터 넣기
                        playerDataDic[player.Name] = playerData;
                        //데이터 저장 폴더 경로 설정
                        string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "data");

                        //폴더가 없으면 생성
                        if (!Directory.Exists(directoryPath))
                        {
                            Directory.CreateDirectory(directoryPath);
                        }

                        //Json 파일 경로 설정
                        filePath = Path.Combine(directoryPath, "data.json");

                        //딕셔너리 데이터를 Json 파일에 저장
                        SaveData.SaveDataToJsonFile(playerDataDic, filePath);

                        Console.WriteLine("게임을 저장합니다.");
                        Environment.Exit(0);
                    }
                    break;
            }
            MainMenu();
        }
        /// <summary>
        /// 플레이어의 상태창을 표시하는 메서드
        /// </summary>
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
            Console.WriteLine($"{player.Name} ( {player.JobStr} )");

            //보너스 어택이 0보다 크면 보여주고, 아니면 스킵
            ConsoleUtility.PrintTextHighlights("공격력 : ", (player.Atk + player.bonusAtk).ToString(), player.bonusAtk > 0 ? $" (+{player.bonusAtk})" : "");
            ConsoleUtility.PrintTextHighlights("방어력 : ", (player.Def + player.bonusDef).ToString(), player.bonusDef > 0 ? $" (+{player.bonusDef})" : "");
            ConsoleUtility.PrintTextHighlights("체  력 : ", (player.Hp + player.bonusHp).ToString(), player.bonusHp > 0 ? $" (+{player.bonusHp})" : "");

            ConsoleUtility.PrintTextHighlights(" Gold  : ", player.Gold.ToString());
            ConsoleUtility.PrintTextHighlights(" Exp to Next Level : ", player.Experience.ToString());

            Console.WriteLine();
            Console.WriteLine("0. 나가기");
            Console.WriteLine();

            ConsoleUtility.PromptMenuChoice(0);
            MainMenu();
        }
        /// <summary>
        /// 인벤토리 화면을 표시하는 메서드
        /// </summary>
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

            switch (ConsoleUtility.PromptMenuChoice(0, 1)) // 메뉴 선택
            {
                case 0:
                    MainMenu();
                    break;
                case 1:
                    EquipMenu();
                    break;
            }
        }
        /// <summary>
        /// 인벤토리 - 장착 화면을 표시하고, 아이템의 장착 & 해제를 관리하는 메서드
        /// </summary>
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

            keyInput = ConsoleUtility.PromptMenuChoice(0, inventory.Count);

            switch (keyInput)
            {
                case 0:
                    InventoryMenu();
                    break;
                default:
                    //아이템 장착&해제 반복
                    inventory[keyInput - 1].ToggleEquipStatus();
                    //장착된 아이템 수치의 합 구하기
                    player.bonusAtk = inventory.Select(item => item.isEquipped ? item.Atk : 0).Sum();
                    player.bonusDef = inventory.Select(item => item.isEquipped ? item.Def : 0).Sum();
                    player.bonusHp = inventory.Select(item => item.isEquipped ? item.Hp : 0).Sum();
                    EquipMenu();
                    break;
            }
        } 
        /// <summary>
        /// 상점 화면을 표시하는 메서드
        /// </summary>
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
        /// <summary>
        /// 상점 - 구매 화면을 표시하고, 아이템을 구매하는 메서드
        /// </summary>
        /// <param name="prompt">표시할 메시지가 있는 경우 사용</param>
        private void PurchaseMenu(string? prompt = null)
        {
            //경고 메세지 띄우기
            if (prompt != null) // 메시지가 있으면?
            {
                //1초간 메세지를 띄운 다음에 다시 진행
                Console.Clear();
                ConsoleUtility.ShowTitle(prompt); // 메시지 표시
                Thread.Sleep(1000); //몇 밀리 세컨드 동안 멈출 것인지 1000 밀리초 = 1초
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

            keyInput = ConsoleUtility.PromptMenuChoice(0, storeInventory.Count);

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
        /// <summary>
        /// 던전 입구를 표시하는 메서드
        /// </summary>
        private void DungeonEntranceMenu()
        {
            Console.Clear();

            ConsoleUtility.ShowTitle("■ 던 전   입 구■");
            Console.WriteLine();

            for(int i = 1; i<5; i++)
            {
                if (player.CurrentLevel < i) Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write(ConsoleUtility.PadRightForMixedText($"{i}층",6));
                Console.WriteLine($" 몬스터 출현 확률 : 미니언 {minionSpawnRate[i-1]}%, 고블린 {goblinSpawnRate[i - 1]}%, 드래곤 : {dragonSpawnRate[i - 1]}%   최대 몬스터 수 : {monstersSpawnMax[i-1]}");
                Console.ResetColor();
            }
            string currentLevelStr = player.CurrentLevel == 1 ? "" : $" ~ {player.CurrentLevel}."; 
            Console.WriteLine($"0. 나가기 1.{currentLevelStr} 던전 선택");

            keyInput = ConsoleUtility.PromptMenuChoice(0, player.CurrentLevel);

            switch (keyInput)
            {
                case 0:
                    MainMenu();
                    break;
                default:
                    DungeonMenu(keyInput);
                    break;
            }
        }
        /// <summary>
        /// 던전 입구에서 선택한 스테이즈를 표시하는 메서드
        /// </summary>
        /// <param name="dungeonIdx">선택한 스테이지</param>
        private void DungeonMenu(int dungeonIdx)
        {
            Console.Clear();

            ConsoleUtility.ShowTitle($"■ {dungeonIdx} 층   던 전 ■");
            Console.WriteLine();
            if (monsters.Count <= 0) // 현재 몬스터 수가 0이하 인가?
            {
                currentHp = player.Hp; // 던전 진입시 현재 체력 저장
                int monsterCount = new Random().Next(1, monstersSpawnMax[dungeonIdx - 1] + 1); // 층 별 몬스터 최대 수 범위 안에서 랜덤
                for (int i = 0; i < monsterCount; i++)
                {
                    int monsterTypeRange = new Random().Next(1, 101);
                    MonsterType monsterType;
                    if (monsterTypeRange <= minionSpawnRate[dungeonIdx - 1])
                    {
                        monsterType = MonsterType.Minion;
                    }
                    else if (monsterTypeRange <= minionSpawnRate[dungeonIdx - 1] + goblinSpawnRate[dungeonIdx - 1])
                    {
                        monsterType = MonsterType.Goblin;
                    }
                    else monsterType = MonsterType.Dragon;
                    switch (monsterType) // 해당 몬스터 타입에 맞게
                    {
                        case MonsterType.Minion:
                            Minion minion = new Minion(); // 몬스터 생성
                            monsters.Add(minion); // 몬스터 리스트에 추가
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
            foreach (Monster mon in monsters) // 모든 몬스터 리스트 스캔하여 하나씩 표시
            {
                string isDeadStr = mon.IsDead ? "사망" : "";
                if (mon.IsDead) Console.ForegroundColor = ConsoleColor.DarkGray;

                Console.Write(ConsoleUtility.PadRightForMixedText($"Lv {mon.Level} {mon.Name} |  HP {mon.Hp} ", 21));
                Console.Write(ConsoleUtility.PadRightForMixedText($"| Atk : {mon.Atk}", 11));
                Console.WriteLine($"| {isDeadStr}");
                Console.ResetColor();
            }
            ConsoleUtility.PrintTextHighlights("\n", "[내정보]");
            Console.WriteLine($"Lv. {player.Level}  {player.Name} ({player.JobStr})");
            Console.WriteLine($"HP {player.Hp} / 100  Atk {player.Atk + player.bonusAtk}");

            Console.WriteLine("\n1. 공격\n");

            ConsoleUtility.PromptMenuChoice(1);
            FightMenu();
        }
        /// <summary>
        /// 공격할 몬스터의 리스트를 표시할 메서드
        /// </summary>
        private void FightMenu()
        {
            Console.Clear();

            ConsoleUtility.ShowTitle("■ 던 전 ■");
            Console.WriteLine();

            for (int i = 0; i < monsters.Count; i++) // 몬스터 리스트의 수 만큼 표시
            {
                string isDeadStr = monsters.ToArray()[i].IsDead ? "사망" : ""; // 해당 몬스터가 죽어있는 경우 "사망" 표시. 아닌 경우 공백.
                if (monsters.ToArray()[i].IsDead) Console.ForegroundColor = ConsoleColor.DarkGray;

                Console.Write(ConsoleUtility.PadRightForMixedText($"{i + 1}- Lv {monsters.ToArray()[i].Level} {monsters.ToArray()[i].Name} |  HP {monsters.ToArray()[i].Hp} ", 24));
                Console.Write(ConsoleUtility.PadRightForMixedText($"| Atk : {monsters.ToArray()[i].Atk}", 11));
                Console.WriteLine($"| {isDeadStr}");
                Console.ResetColor();
            }
            ConsoleUtility.PrintTextHighlights("\n", "[내정보]");
            Console.WriteLine($"Lv. {player.Level}  {player.Name} ({player.JobStr})");
            Console.WriteLine($"HP {player.Hp} / 100  Atk {player.Atk + player.bonusAtk}");

            string selectMaxStr = monsters.Count == 1 ? "" : $" ~ {monsters.Count}."; // 몬스터 수에 맞는 선택 영역 문자열 할당
            Console.WriteLine($"\n1.{selectMaxStr}  몬스터 선택"); // 해당 문자열 표시
            Console.WriteLine("0. 취소");

            keyInput = ConsoleUtility.PromptMenuChoice(0, monsters.Count);
            while (true) // 유효하는 값이 입력될 때 까지 계속 반복
            {
                if (keyInput == 0) // 0번 선택
                {
                    //DungeonMenu();
                }
                else if (monsters.ToArray()[keyInput - 1].IsDead) // 선택한 몬스터가 죽었는가? (잘못된 선택)
                {
                    Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요");
                    keyInput = ConsoleUtility.PromptMenuChoice(0, monsters.Count); // 재입력 후 반복. while이 없는 경우 정상적인 입력으로 판단함.
                }
                else
                {
                    fieldMonster.Add(monsters.ToArray()[keyInput - 1]);
                    for (int i = 0; i < monsters.Count; i++)
                    {
                        if (i == keyInput - 1) continue;
                        fieldMonster.Add(monsters.ToArray()[i]);
                    }
                    BattleMenu(monsters.ToArray()[keyInput - 1]); // 전투 메뉴 진입
                }
            }
        }
        /// <summary>
        /// 몬스터와 플레이어가 서로 데미지를 주고 받는 메서드
        /// </summary>
        /// <param name="character">데미지를 받을 객체 (플레이어 또는 몬스터 하나)</param>
        private void BattleMenu(ICharacter character)
        {
            Random crit = new Random();
            int critPer = crit.Next(101); // 치명타 확률 (15퍼센트)

            Random evade = new Random();
            int evadePer = evade.Next(101); /// 회피 확률 (10퍼센트)

            int currentDamage;
            if (character is Monster) // 입력값이 Monster인 경우 (플레이어 차례)
            {
                int deadCount = 0; // 죽은 몬스터 수
                Console.Clear();

                ConsoleUtility.ShowTitle("■ Battle!! ■\n");

                ICharacter opposite = player; // 상대방 : 플레이어
                //currentDamage = opposite.Atk + bonusAtk + (new Random().Next(-1, 2) * (int)Math.Ceiling((opposite.Atk + bonusAtk) * 0.1f)); // 가할 데미지 계산
                int sign = new Random().Next(-1, 2); // 부호
                float percent = new Random().Next(1, 101) * 0.001f; // 0.1퍼센트 ~ 10퍼센트
                currentDamage = opposite.Atk + (int)player.bonusAtk + (sign * (int)Math.Ceiling(((opposite.Atk + (int)player.bonusAtk) * percent)));
                //Console.WriteLine($"{opposite.Atk+bonusAtk}, {sign}, {percent * 100}, {(opposite.Atk + bonusAtk) * percent}, {(int)Math.Ceiling(((opposite.Atk + bonusAtk) * percent))}");
                
                if (evadePer < 10)  // 플레이어가 공격 실패
                {
                    currentDamage = 0;
                    Console.WriteLine($"{opposite.Name}의 공격!");

                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine($" LV. {character.Level} {character.Name} 을(를) 공격했지만 빗나갔다!");
                    Console.ResetColor();
                }

                else if (critPer < 15) // 플레이어가 크리티컬
                {
                    currentDamage = (int)Math.Ceiling(currentDamage * 1.6f); // 크리티컬 배수 1.6배
                    Console.WriteLine($"{opposite.Name}의 공격!");

                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write("치명적인 일격!");
                    Console.ResetColor();

                    Console.WriteLine($" LV. {character.Level} {character.Name} 을(를) 맞췄습니다. [데미지 : {currentDamage}]");
                }

                else
                {
                    Console.WriteLine($"{opposite.Name}의 공격!");
                    Console.WriteLine($"Lv {character.Level} {character.Name} 을(를) 맞췄습니다. [데미지 : {currentDamage}]");

                }

                Console.WriteLine();
                Console.WriteLine($"Lv {character.Level} {character.Name}");
                Console.Write($"HP {character.Hp}"); // 피격 전 체력
                character.TakeDamage(currentDamage); // 피격 계산
                Console.WriteLine($" -> {character.Hp}"); // 피격 후 체력
                Console.WriteLine();
                Console.WriteLine("0. 다음");

                if (character.IsDead) tempGetExp += character.Level; // 현재 가격한 몬스터가 사망한 경우 임시 경험치 변수에 누적

                ConsoleUtility.PromptMenuChoice(0);
                foreach (Monster mon in monsters) // 모든 몬스터 무리를 스캔
                {
                    if (mon.IsDead) deadCount++; // 현재 커서의 몬스터가 죽어있는 경우 카운트
                }
                if (deadCount == monsters.Count) deathEvent?.Invoke(character); // 현재 죽어있는 몬스터의 수가 몬스터 무리의 전체의 수와 같은가? (다 죽었나?)
                BattleMenu(player); // 몬스터 차례
            }
            else // 입력값이 Player인 경우 (몬스터 차례)
            {

                ICharacter[] opposites = fieldMonster.ToArray(); // 상대방 : 몬스터 무리
                foreach (ICharacter opposite in opposites) // 모든 몬스터 무리가
                {
                    Console.Clear();

                    ConsoleUtility.ShowTitle("■ Battle!! ■\n");

                    if (opposite.IsDead) continue; // 현재 커서의 몬스터가 죽어있는 경우 아래 코드 무시 후 재진입
                    //currentDamage = opposite.Atk + (new Random().Next(-1, 2) * (int)Math.Ceiling(opposite.Atk * 0.1f)); // 가할 데미지 계산
                    int sign = new Random().Next(-1, 2); // 부호 (-1, 0, 1)
                    float percent = new Random().Next(1, 101) * 0.001f; // 오차 정밀도를 높이고 싶으면 Next의 최댓값을 올리고 곱하는 0.001f의 자릿수를 늘린다.
                    currentDamage = opposite.Atk + (sign * (int)Math.Ceiling(opposite.Atk * percent));
                    //Console.WriteLine($"{opposite.Atk}, {sign}, {percent * 100}, {opposite.Atk * percent}, {(int)Math.Ceiling(opposite.Atk * percent)}");
                    
                    if (evadePer < 10)   // 플레이어가 회피
                    {
                        Console.WriteLine($"Lv {opposite.Level} {opposite.Name}의 공격!");

                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine($" LV. {character.Level} {character.Name} 을(를) 공격했지만 빗나갔다!");
                        Console.ResetColor();
                    }

                    else if (critPer < 5)  //플레이어가 맞는 크리티컬
                    {
                        currentDamage = (int)Math.Ceiling(currentDamage * 1.6f);
                        Console.WriteLine($"Lv {opposite.Level} {opposite.Name}의 공격!");

                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.Write("치명적인 일격!");
                        Console.ResetColor();

                        Console.WriteLine($" LV. {character.Level} {character.Name} 을(를) 맞췄습니다. [데미지 : {currentDamage}]");
                    }

                    else
                    {
                        Console.WriteLine($"Lv {opposite.Level} {opposite.Name}의 공격!");
                        Console.WriteLine($"{character.Name} 을(를) 맞췄습니다. [데미지 : {currentDamage}]");
                    }

                    Console.WriteLine();
                    Console.WriteLine($"Lv {character.Level} {character.Name}");
                    Console.Write($"HP {character.Hp}"); // 피격 전 체력
                    character.TakeDamage(currentDamage); // 피격 계산
                    Console.WriteLine($" -> {character.Hp}"); // 피격 후 체력
                    Console.WriteLine();
                    Console.WriteLine("0. 다음");
                    ConsoleUtility.PromptMenuChoice(0);
                    if (character.IsDead) deathEvent?.Invoke(character); // 플레이어 죽은 경우 이벤스 실행
                }
                fieldMonster.Clear();
                FightMenu(); // 다시 전투 선택 메뉴으로 이동
            }
        }
        /// <summary>
        /// 승리 혹은 패배 시 호출되는 메서드
        /// </summary>
        /// <param name="character">패배한 객체 (플레이어 또는 몬스터)</param>
        private void RewardMenu(ICharacter character)
        {
            Console.Clear();
            if (character is Player) // 패배시
            {
                ConsoleUtility.ShowTitle("■ Battle!!  - Result ■");
                ConsoleUtility.PrintTextHighlights("\n", "You Lose\n");

                Console.WriteLine($"Lv {player.Level} {player.Name}");
                Console.WriteLine($"HP {currentHp} -> {player.Hp}");

                ConsoleUtility.PromptMenuChoice(0);

                player.Hp = 100; // 플레이어 체력 다시 회복
            }
            else // 승리시
            {
                player.CurrentLevel++; //플레이어 던전 입장 레벨
                ConsoleUtility.ShowTitle("■ Battle!!  - Result ■");
                ConsoleUtility.PrintTextHighlights("\n", "Victory\n");

                Console.WriteLine($"던전에서 몬스터 {monsters.Count}마리를 잡았습니다.");
                player.GainExperience(tempGetExp); // 누적된 경험치 획득
                tempGetExp = 0;
                Console.WriteLine($"Lv {player.Level} {player.Name}");
                Console.WriteLine($"HP {currentHp} -> {player.Hp}");
                ConsoleUtility.PromptMenuChoice(0);
            }
            fieldMonster.Clear(); // 공격 순서 리스트 초기화
            monsters.Clear(); // 스테이지 몬스터 리스트 초기화
            DungeonEntranceMenu();
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
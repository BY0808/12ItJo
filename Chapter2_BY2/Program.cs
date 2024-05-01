


using System.Xml.Linq;

namespace Chapter2_BY2
{
    public class GameManager
    {
        private Player player; // 플레이어 객체
        private List<Item> inventory; // 인벤토리 리스트

        private List<Item> storeInventory; // 상점 품목 리스트

        List<Monster> monsters = new List<Monster>(); // 몬스터를 저장할 리스트

        int bonusAtk, bonusDef, bonusHp; // 추가 공격력 / 추가 방어력 / 추가 체력

        private delegate void GameEvent(ICharacter character); // GameEvent 대리자 (함수를 담을 변수) 
        private event GameEvent deathEvent; // GameEvent 형식의 event 대리자

        int currentHp; // 던전 진입시 체력을 저장할 변수

        int keyInput; // 키를 입력받을 변수 

        List<Monster> fieldMonster = new List<Monster>(); // 몬스터의 공격 순서를 제어할 리스트

        public string filePath; //파일위치

        public GameManager() //클래스와 이름이 같은 함수, 생성자, 클래스 호출시 실행
        {
            InitializeGame();
        }

        private void InitializeGame() // 게임 시작 준비
        {
            //파일 위치 찾기
            filePath = Path.Combine(Directory.GetCurrentDirectory(), "data", "data.json");
            // 저장파일 불러오기
            SaveData loadedData = SaveData.LoadDataFromJsonFile(filePath);

            //기본적인 초기화!
            if (loadedData == null)
            {
                inventory = new List<Item>(); // 인벤토리 객체 생성

                storeInventory = new List<Item>(); // 상점 품목 리스트 생성 & 리스트 추가
                storeInventory.Add(new Item("수련자 갑옷", "적당한 갑옷", ItemType.ARMOR, 0, 5, 0, 1000));
                storeInventory.Add(new Item("무쇠갑옷", "조금좋은 갑옷", ItemType.ARMOR, 0, 9, 0, 1500));
                storeInventory.Add(new Item("스파르타 갑옷", "좋은 갑옷", ItemType.ARMOR, 0, 15, 0, 3500));
                storeInventory.Add(new Item("낡은 검", "적당한 무기", ItemType.WEAPON, 2, 0, 0, 600));
                storeInventory.Add(new Item("청동 도끼", "조금좋은 무기", ItemType.WEAPON, 5, 0, 0, 1500));
                storeInventory.Add(new Item("스파르타 창", "좋은 무기", ItemType.WEAPON, 7, 0, 0, 3500));
            }
            else
            {
                player = loadedData.savePlayer;
                inventory = loadedData.saveInventory;
                storeInventory = loadedData.saveStoreInventory;
                bonusAtk = loadedData.saveBonusAtk;
                bonusDef = loadedData.saveBonusDef;
                bonusHp = loadedData.saveBonusHp;
            }

            deathEvent += RewardMenu; // 이벤트 메서드에 해당 메서드 추가
        }

        //Program 이라는 다른 클래스 접근!
        public void StartGame() // 게임 시작
        {
            //콘솔 게임은 콘솔 지워주는걸 지속해야함..!
            Console.Clear();
            //static 으로 정의된 함수라 인스턴스 없이 호출
            ConsoleUtility.PrintGameHeader();
            SaveData loadedData = SaveData.LoadDataFromJsonFile(filePath);
            if(loadedData == null) InsertNameMenu();
            else MainMenu();
        }

        public void InsertNameMenu() // 플레이어 이름 입력 메뉴
        {
            Console.Clear();
            string playerName;
            Console.WriteLine("스파르타 던전에 오신 여러분 환영합니다.");
            Console.WriteLine("원하시는 이름을 설정해주세요 >> ");
            do
            {
                playerName = Console.ReadLine(); // 이름 입력 창
                if (playerName == "") Console.WriteLine("다시 입력해주세요."); // 입력된 이름이 공백인 경우, 메시지 출력
            } while (playerName == ""); // 입력된 이름이 공백인 경우 계속 반복
            player = new Player(playerName, "Huge", 1, 10, 5, 100, 2000); // 플레이어 객체 생성 & 초기화
            MainMenu();
        }
        private void MainMenu() // 메인 메뉴
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
            Console.WriteLine("원하시는 이름을 설정해주세요 >> ");
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
                    DungeonMenu();
                    break;
                case 0:
                    Console.Clear();
                    ConsoleUtility.PrintGameEnd();
                    if (Console.ReadLine() == "0") //데이터 저장하기
                    {
                        //저장을 위한 객체 생성
                        SaveData saveData = new SaveData
                        {
                            savePlayer = player,
                            saveInventory = inventory,
                            saveStoreInventory = storeInventory,
                            saveBonusAtk = bonusAtk,
                            saveBonusDef = bonusDef,
                            saveBonusHp = bonusHp
                        };
                        //데이터 저장 폴더 경로 설정
                        string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "data");

                        //폴더가 없으면 생성
                        if (!Directory.Exists(directoryPath))
                        {
                            Directory.CreateDirectory(directoryPath);
                        }

                        //Json 파일 경로 설정
                        filePath = Path.Combine(directoryPath, "data.json");

                        //데이터를 Json 파일에 저장
                        SaveData.SaveDataToJsonFile(saveData, filePath);

                        Console.WriteLine("게임을 저장합니다.");
                        Environment.Exit(0);
                    }
                    break;
            }
            MainMenu();
        }

        private void StatusMenu() // 상태창 메뉴
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

            ConsoleUtility.PrintTextHighlights(" Gold  : ", player.Gold.ToString());
            Console.WriteLine();
            Console.WriteLine("0. 나가기");
            Console.WriteLine();

            ConsoleUtility.PromptMenuChoice(0);
            MainMenu();
        }

        private void InventoryMenu() // 인벤토리 메뉴
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

        private void EquipMenu() // 장착 메뉴
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
                    EquipMenu();
                    break;
            }
        } 

        private void StoreMenu() // 상점 메뉴
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

        private void PurchaseMenu(string? prompt = null) // 상점 구매 메뉴
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

        private void DungeonMenu() // 던전 메뉴
        {
            Console.Clear();

            ConsoleUtility.ShowTitle("■ 던 전 ■");
            Console.WriteLine();
            if (monsters.Count <= 0) // 현재 몬스터 수가 0이하 인가?
            {
                currentHp = player.Hp; // 던전 진입시 현재 체력 저장
                int monsterCount = new Random().Next(1, 5); // 1 ~ 4 마리의 몬스터 소환
                for (int i = 0; i < monsterCount; i++)
                {
                    MonsterType monsterType = (MonsterType)(new Random().Next(1, 4)); // 몬스터 타입을 미니언, 고블린, 드래곤 사이에서 랜덤하게 선택
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
            Console.WriteLine($"Lv. {player.Level}  {player.Name} ({player.Job})");
            Console.WriteLine($"HP {player.Hp} / 100  Atk {player.Atk + bonusAtk}");

            Console.WriteLine("\n1. 공격\n");

            ConsoleUtility.PromptMenuChoice(1);
            FightMenu();
        }

        private void FightMenu() // 전투 선택 메뉴
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
            Console.WriteLine($"Lv. {player.Level}  {player.Name} ({player.Job})");
            Console.WriteLine($"HP {player.Hp} / 100  Atk {player.Atk + bonusAtk}");

            string selectMaxStr = monsters.Count == 1 ? "" : $" ~ {monsters.Count}."; // 몬스터 수에 맞는 선택 영역 문자열 할당
            Console.WriteLine($"\n1.{selectMaxStr}  몬스터 선택"); // 해당 문자열 표시
            Console.WriteLine("0. 취소");

            keyInput = ConsoleUtility.PromptMenuChoice(0, monsters.Count);
            while (true) // 유효하는 값이 입력될 때 까지 계속 반복
            {
                if (keyInput == 0) // 0번 선택
                {
                    DungeonMenu();
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
        private void BattleMenu(ICharacter character) //전투 메뉴
        {
            Random crit = new Random();
            int critPer = crit.Next(101);

            Random evade = new Random();
            int evadePer = evade.Next(101);

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
                currentDamage = opposite.Atk + bonusAtk + (sign * (int)Math.Ceiling(((opposite.Atk + bonusAtk) * percent)));
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
                    currentDamage = (int)Math.Ceiling(currentDamage * 1.6f);
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

        private void RewardMenu(ICharacter character) // 보상 메뉴
        {
            Console.Clear();
            if (character is Player) // 패배시
            {
                ConsoleUtility.ShowTitle("■ Battle!!  - Result ■");
                ConsoleUtility.PrintTextHighlights("\n", "You Lose\n");

                Console.WriteLine($"Lv {player.Level} {player.Name}");
                Console.WriteLine($"HP {currentHp} -> {player.Hp}");

                ConsoleUtility.PromptMenuChoice(0);

                player.Hp = 100;
            }
            else // 승리시
            {
                ConsoleUtility.ShowTitle("■ Battle!!  - Result ■");
                ConsoleUtility.PrintTextHighlights("\n", "Victory\n");

                Console.WriteLine($"던전에서 몬스터 {monsters.Count}마리를 잡았습니다.");

                Console.WriteLine($"Lv {player.Level} {player.Name}");
                Console.WriteLine($"HP {currentHp} -> {player.Hp}");

                ConsoleUtility.PromptMenuChoice(0);
            }
            fieldMonster.Clear();
            monsters.Clear();
            MainMenu();
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
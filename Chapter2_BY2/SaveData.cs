using Newtonsoft.Json;

namespace Chapter2_BY2
{
    /// <summary>
    /// 세이브 데이터를 담을 클래스
    /// </summary>
    class SaveData
    {
        /// <summary>
        /// 플레이어 관련 데이터
        /// </summary>
        public Player savePlayer { get; set; }
        /// <summary>
        /// 인벤토리 관련 데이터
        /// </summary>
        public List<Item> saveInventory { get; set; }
        /// <summary>
        /// 상점 관련 데이터
        /// </summary>
        public List<Item> saveStoreInventory { get; set; }
        /// <summary>
        /// 현재 추가 공격력, 추가 방어력, 추가 체력 데이터
        /// </summary>
        public int saveBonusAtk, saveBonusDef, saveBonusHp;

        //데이터를 저장할 딕셔너리 생성
        //저장을 위한 딕셔너리 만들기
        /// <summary>
        /// 저장할 / 불러올 데이터
        /// </summary>
        public Dictionary<string, SaveData> loadedData = new Dictionary<string, SaveData>();

        //생성자 만들기
        /// <summary>
        /// 데이터 파일 생성
        /// </summary>
        /// <param name="player">플레이어 관련 데이터</param>
        /// <param name="inventory">인벤토리 관련 데이터</param>
        /// <param name="storeInventory">상점 관련 데이터</param>
        /// <param name="bonusAtk">추가 공격력 데이터</param>
        /// <param name="bonusDef">추가 방어력 데이터</param>
        /// <param name="bonusHP">추가 체력 데이터</param>
        public SaveData(Player player, List<Item> inventory, List<Item> storeInventory, int bonusAtk, int bonusDef, int bonusHP)
        {
            savePlayer = player;
            saveInventory = inventory;
            saveStoreInventory = storeInventory;
            saveBonusAtk = bonusAtk;
            saveBonusDef = bonusDef;
            saveBonusHp = bonusHP;
        }

        //키값(playerName) 기준으로 데이터를 파일에 저장
        //데이터를 저장하는 JSON의 형태로 저장하는 메서드
        /// <summary>
        /// 데이터를 JSON으로 저장하는 메서드
        /// </summary>
        /// <param name="data">저장할 데이터</param>
        /// <param name="filePath">파일 이름까지 포함된 파일 경로</param>
        public static void SaveDataToJsonFile(Dictionary<string, SaveData> data, string filePath) 
        {
            //이전 데이터가 있는 경우 불러오기
            Dictionary<string, SaveData> existingData = new Dictionary<string, SaveData>();
            if (File.Exists(filePath))
            {
                // 이전 데이터 가져오기
                string existingJsonData = File.ReadAllText(filePath);
                // JSON 형태의 데이터를 역직렬화
                existingData = JsonConvert.DeserializeObject<Dictionary<string, SaveData>>(existingJsonData);
            }

            // 새로운 데이터 추가 또는 기존 데이터 덮어쓰기
            foreach (var entry in data)
            {
                existingData[entry.Key] = entry.Value; // 세이브 데이터에 키 값을 통해 데이터를 저장
            }

            string jsonData = JsonConvert.SerializeObject(existingData, Formatting.Indented); // 데이터를 JSON 형태로 직렬화
            File.WriteAllText(filePath, jsonData); // 직렬화 된 JSON 데이터를 파일 경로에 생성
        }

        //키값을 바탕으로 데이터를 파일에서 불러옴
        /// <summary>
        /// JSON 데이터를 불러오는 메서드
        /// </summary>
        /// <param name="filePath">파일 이름이 포함된 경로</param>
        /// <param name="playerName">플레이어 이름</param>
        /// <returns></returns>
        public static SaveData LoadDataFromJsonFile(string filePath, string playerName)
        {
            if (File.Exists(filePath)) // 해당 경로에 파일이 있는가?
            {
                string jsonData = File.ReadAllText(filePath); // 파일 경로에 있는 데이터 불러오기
                Console.WriteLine("파일을 불러왔습니다.");
                // 불러온 데이터를 역직렬화
                Dictionary<string, SaveData> loadedData = JsonConvert.DeserializeObject<Dictionary<string, SaveData>>(jsonData);
                Console.WriteLine("데이터를 불러왔습니다.");

                //playerName을 키로 사용하여 해당 플레이어의 데이터를 가져옴
                if (loadedData.ContainsKey(playerName))
                {
                    return loadedData[playerName];
                }
                else
                {
                    Console.WriteLine($"플레이어 '{playerName}'을 생성합니다.");
                    return null;
                }
            }
            else
            {
                Console.WriteLine("저장된 파일이 없습니다.");
                return null;
            }
        }
    }
}
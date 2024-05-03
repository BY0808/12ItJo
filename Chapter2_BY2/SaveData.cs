using Newtonsoft.Json;

namespace Chapter2_BY2
{
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
        /// 상점 품목 관련 데이터
        /// </summary>
        public List<Item> saveStoreInventory { get; set; }
        /// <summary>
        /// 추가 수치 관련 데이터
        /// </summary>
        public int saveBonusAtk, saveBonusDef, saveBonusHp;

        //데이터를 저장할 딕셔너리 생성
        //저장을 위한 딕셔너리 만들기
        /// <summary>
        /// 딕셔너리 자료구조의 세이브 데이터를 받아올 변수
        /// </summary>
        public Dictionary<string, SaveData> loadedData = new Dictionary<string, SaveData>();

        //생성자 만들기
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
        /// 게임 데이터를 JSON 파일로 저장하는 메서드
        /// </summary>
        /// <param name="data">저장할 게임 데이터</param>
        /// <param name="filePath">파일 명을 포함한 파일 경로</param>
        public static void SaveDataToJsonFile(Dictionary<string, SaveData> data, string filePath) 
        {
            //이전 데이터가 있는 경우 불러오기
            Dictionary<string, SaveData> existingData = new Dictionary<string, SaveData>();
            if (File.Exists(filePath))
            {
                string existingJsonData = File.ReadAllText(filePath);
                // 기존 JSON 데이터를 역직렬화
                existingData = JsonConvert.DeserializeObject<Dictionary<string, SaveData>>(existingJsonData);
            }

            // 새로운 데이터 추가 또는 기존 데이터 덮어쓰기
            foreach (var entry in data)
            {
                existingData[entry.Key] = entry.Value; // 딕셔너리의 키값 (플레이어 이름)을 기준으로 데이터값 (게임 데이터) 추가 / 덮어쓰기
            }

            string jsonData = JsonConvert.SerializeObject(existingData, Formatting.Indented);
            File.WriteAllText(filePath, jsonData); // 직렬화 된 JSON 데이터를 파일 경로에 생성
        }

        //키값을 바탕으로 데이터를 파일에서 불러옴
        /// <summary>
        /// JSON 파일의 세이브 파일을 불러오는 메서드
        /// </summary>
        /// <param name="filePath">파일 이름을 포함한 파일 경로</param>
        /// <returns></returns>
        public static Dictionary<string, SaveData> LoadDataFromJsonFile(string filePath)
        {
            if (File.Exists(filePath)) // 해당 경로에 파일이 있는가?
            {
                string jsonData = File.ReadAllText(filePath); // 파일 경로에 있는 데이터 불러오기
                Console.WriteLine("\n파일을 불러왔습니다.");
                // 불러온 데이터를 역직렬화
                Dictionary<string, SaveData> loadedData = JsonConvert.DeserializeObject<Dictionary<string, SaveData>>(jsonData);
                Console.WriteLine("데이터를 불러왔습니다.\n");
                return loadedData;
            }
            else
            {
                Console.WriteLine("\n저장된 파일이 없습니다.");
                return null;
            }
        }
    }
}
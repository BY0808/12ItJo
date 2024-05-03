using Newtonsoft.Json;

namespace Chapter2_BY2
{
    class SaveData
    {
        public Player savePlayer { get; set; }
        public List<Item> saveInventory { get; set; }
        public List<Item> saveStoreInventory { get; set; }
        public int saveBonusAtk, saveBonusDef, saveBonusHp;

        //데이터를 저장할 딕셔너리 생성
        //저장을 위한 딕셔너리 만들기
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
        public static void SaveDataToJsonFile(Dictionary<string, SaveData> data, string filePath) 
        {
            //이전 데이터가 있는 경우 불러오기
            Dictionary<string, SaveData> existingData = new Dictionary<string, SaveData>();
            if (File.Exists(filePath))
            {
                string existingJsonData = File.ReadAllText(filePath);
                // 데이터를 JSON의 형태로 직렬화
                existingData = JsonConvert.DeserializeObject<Dictionary<string, SaveData>>(existingJsonData);
            }

            // 새로운 데이터 추가 또는 기존 데이터 덮어쓰기
            foreach (var entry in data)
            {
                existingData[entry.Key] = entry.Value;
            }

            string jsonData = JsonConvert.SerializeObject(existingData, Formatting.Indented);
            File.WriteAllText(filePath, jsonData); // 직렬화 된 JSON 데이터를 파일 경로에 생성
        }

        //키값을 바탕으로 데이터를 파일에서 불러옴
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
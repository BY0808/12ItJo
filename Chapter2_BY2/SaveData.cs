using Newtonsoft.Json;

namespace Chapter2_BY2
{
    class SaveData
    {
        public Player savePlayer { get; set; }
        public List<Item> saveInventory { get; set; }
        public List<Item> saveStoreInventory { get; set; }
        public int saveBonusAtk, saveBonusDef, saveBonusHp;

        //데이터를 파일에 저장
        public static void SaveDataToJsonFile(SaveData data, string filePath) // 데이터를 저장하는 JSON의 형태로 저장하는 메서드
        {
            string jsonData = JsonConvert.SerializeObject(data, Formatting.Indented); // 데이터를 JSON의 형태로 직렬화
            File.WriteAllText(filePath, jsonData); // 직렬화 된 JSON 데이터를 파일 경로에 생성
        }

        //데이터를 파일에서 불러옴
        public static SaveData LoadDataFromJsonFile(string filePath)
        {
            if (File.Exists(filePath)) // 해당 경로에 파일이 있는가?
            {
                string jsonData = File.ReadAllText(filePath); // 파일 경로에 있는 데이터 불러오기
                Console.WriteLine("파일을 불러왔습니다.");
                SaveData loadedData = JsonConvert.DeserializeObject<SaveData>(jsonData); // 불러온 데이터를 역직렬화
                Console.WriteLine("데이터를 불러왔습니다.");
                return loadedData;
            }
            else
            {
                Console.WriteLine("저장된 파일이 없습니다.");
                return null;
            }
        }
    }
}
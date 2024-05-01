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
        public static void SaveDataToJsonFile(SaveData data, string filePath)
        {
            string jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(filePath, jsonData);
        }

        //데이터를 파일에서 불러옴
        public static SaveData LoadDataFromJsonFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                string jsonData = File.ReadAllText(filePath);
                Console.WriteLine("파일을 불러왔습니다.");
                SaveData loadedData = JsonConvert.DeserializeObject<SaveData>(jsonData);
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
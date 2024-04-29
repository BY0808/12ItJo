namespace Chapter2_BY2
{
    internal class Player
    {
        //get 만 있는 프로퍼티 > 생성자 이후 Set 하지 않겠다! > 읽기전용
        public string Name { get; }
        public string Job { get; }
        public int Level { get; }
        public int Atk { get; }
        public int Def {  get; }
        public int Hp { get; }
        public int Gold { get; set; }

        // 생성자 용도는 기본 셋팅
        public Player(string name, string job, int level, int atk, int def, int hp, int gold)
        {
            Name = name;
            Job = job;
            Level = level;
            Atk = atk;
            Def = def;
            Hp = hp;
            Gold = gold;
        }
    }
}
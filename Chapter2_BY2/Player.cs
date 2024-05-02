namespace Chapter2_BY2
{
    internal class Player : ICharacter
    {
        private int hp { get; set; } // 체력 필드

        //get 만 있는 프로퍼티 > 생성자 이후 Set 하지 않겠다! > 읽기전용
        public string Name { get; } // 이름
        public string Job { get; } // 직업
        public int Level { get; } // 레벨
        public int Atk { get; } // 공격력
        public int Def {  get; } // 방어력
        public int Hp // 체력
        {
            get { return hp; }
            set
            {
                if (value <= 0) hp = 0; // 0 이하인 경우 0 반환
                else hp = value;
            }
        }
        public int Gold { get; set; } // 재화
        public bool IsDead => Hp <= 0; // 체력이 0 이하인 경우 true

        public int CurrentLevel {  get; set; }

        // 생성자 용도는 기본 셋팅
        public Player(string name, string job, int level, int atk, int def, int hp, int gold, int currentLevel = 1)
        {
            Name = name;
            Job = job;
            Level = level;
            Atk = atk;
            Def = def;
            Hp = hp;
            Gold = gold;
            CurrentLevel = currentLevel;
        }

        public void TakeDamage(int damage) // 플레이어가 데미지를 받는 메서드
        {
            Hp -= damage;
        }
    }
}
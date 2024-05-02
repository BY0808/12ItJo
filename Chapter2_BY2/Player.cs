namespace Chapter2_BY2
{
    internal class Player : ICharacter
    {
        private int hp { get; set; } // 체력 필드

        //get 만 있는 프로퍼티 > 생성자 이후 Set 하지 않겠다! > 읽기전용
        public string Name { get; } // 이름
        public string Job { get; } // 직업
        public int Level { get; set; } // 레벨
        public int  Atk { get; set; } // 공격력
        public int Def { get; set; } // 방어력
        public int Experience { get; set; } // 경험치

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

        private int[] levelUpExp = { 10, 35, 65, 100 }; // 레벨업에 필요한 경험치 량
        // 생성자 용도는 기본 셋팅
        public Player(string name, string job, int level, int atk, int def, int hp, int gold, int currentLevel = 1, int experience = 0)
        {
            Name = name;
            Job = job;
            Level = level;
            Atk = atk;
            Def = def;
            Hp = hp;
            Gold = gold;
            CurrentLevel = currentLevel;
            Experience = experience;
        }

        public void TakeDamage(int damage) // 플레이어가 데미지를 받는 메서드
        {
            Hp -= damage;
        }

        public void GainExperience(int experience)
        {
            Experience += experience;
            Console.WriteLine($"{Name}이(가) {experience}의 경험치를 획득했습니다!");
            CheckLevelUp(Level);
        }

        public void CheckLevelUp(int currentLevel)
        {
            // 경험치가 레벨업에 가능한지 확인하고, 레벨을 증가시킵니다.
            if (Experience >= levelUpExp[currentLevel - 1])
            {
                Level++;
                Experience -= levelUpExp[currentLevel - 1];
                Console.WriteLine($"{Name}이(가) 레벨업했습니다! 현재 레벨: {Level}");
                IncreaseStats();
            }

        }
        public void IncreaseStats()
        {
            // 레벨업 시 공격력과 방어력을 증가시킵니다.
            Atk = (int)(Atk + 0.5); // 기본 공격력을 0.5 증가
            Def +=(Def + 1); ; // 방어력을 1 증가
            Console.WriteLine($"{Name}의 공격력이 0.5, 방어력이 1 증가했습니다!");
        }

    }
}
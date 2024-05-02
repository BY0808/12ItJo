namespace Chapter2_BY2
{
    /// <summary>
    /// 플레이어 클래스
    /// </summary>
    internal class Player : ICharacter
    {
        private int hp; // 체력 필드

        //get 만 있는 프로퍼티 > 생성자 이후 Set 하지 않겠다! > 읽기전용
        /// <summary>
        /// 이름 프로퍼티
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// 직업 프로퍼티
        /// </summary>
        public string Job { get; }
        /// <summary>
        /// 레벨 프로퍼티
        /// </summary>
        public int Level { get; }
        /// <summary>
        /// 공격력 프로퍼티
        /// </summary>
        public int Atk { get; }
        /// <summary>
        /// 방어력 프로퍼티
        /// </summary>
        public int Def {  get; }
        /// <summary>
        /// 체력 프로퍼티
        /// </summary>
        public int Hp
        {
            get { return hp; }
            set
            {
                if (value <= 0) hp = 0; // 0 이하인 경우 0 반환
                else hp = value;
            }
        }
        /// <summary>
        /// 재화 프로퍼티
        /// </summary>
        public int Gold { get; set; }
        /// <summary>
        /// 사망 여부 프로퍼티 (Hp가 0 이하일 경우 true)
        /// </summary>>
        public bool IsDead => Hp <= 0;
        /// <summary>
        /// 현재 해금한 최대 레벨
        /// </summary>
        public int CurrentLevel { get; set; }

        /// <summary>
        /// 플레이어 객체 생성
        /// </summary>
        /// <param name="name">이름</param>
        /// <param name="job">직업</param>
        /// <param name="level">레벨</param>
        /// <param name="atk">공격력</param>
        /// <param name="def">방어력</param>
        /// <param name="hp">체력</param>
        /// <param name="gold">재화</param>
        /// <param name="currentLevel">해금된 최대 레벨</param>
        public Player(string name, string job, int level, int atk, int def, int hp, int gold, int currentLevel = 1) // 생성자 용도는 기본 셋팅
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

        /// <summary>
        /// 데미지 피격 메서드
        /// </summary>
        /// <param name="damage">피격 데미지</param>
        public void TakeDamage(int damage) // 플레이어가 데미지를 받는 메서드
        {
            Hp -= damage;
        }
    }
}
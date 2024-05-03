using Newtonsoft.Json.Bson;

namespace Chapter2_BY2
{
    /// <summary>
    /// 몬스터 타입
    /// </summary>
    enum MonsterType
    {
        Minion = 1,
        Goblin,
        Dragon
    }
    public class Monster : ICharacter // 미니언, 고블린, 드래곤의 부모 클래스
    {
        private int hp {  get; set; } // 몬스터 체력 필드
        /// <summary>
        /// 몬스터 레벨
        /// </summary>
        public int Level { get; }
        /// <summary>
        /// 몬스터 이름
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// 몬스터 체력
        /// </summary>
        public int Hp // ( 유효성 검사를 위해 필드랑 프로퍼티를 분리)
        {
            get { return hp; } 
            set
            {
                hp = value <= 0 ? 0 : value;
            }
        } 
        /// <summary>
        /// 몬스터 공격력
        /// </summary>
        public int Atk { get;} // 몬스터 공격력
        /// <summary>
        /// 몬스터 사망 여부
        /// </summary>
        public bool IsDead => Hp <= 0; // 체력이 0 이하인 경우 true
        /// <summary>
        /// 몬스터 생성자
        /// </summary>
        /// <param name="name">이름</param>
        /// <param name="hpMulti">체력 배수</param>
        /// <param name="attackMulti">공격력 배수</param>
        public Monster(int level, string name, int hpMulti, int attackMulti)
        {
            Level = level; // 레벨을 1 ~ 5 사이에서 랜덤
            Name = name;
            Hp = Level * hpMulti; // 체력 = 레벨 * 체력 배수
            Atk = Level * attackMulti; // 공격력 = 레벨 * 공격력 배수
        }
        /// <summary>
        /// 데미지를 받는 메서드
        /// </summary>
        /// <param name="damage">받는 데미지</param>
        public void TakeDamage(int damage) // 몬스터가 데미지를 받는 메서드
        {
            Hp -= damage;
        } 
    }

    public class Minion : Monster
    {
        public Minion() : base(new Random().Next(1, 4),"미니언", 5, 3) { } // 미니언 생성자
    }

    public class Goblin : Monster
    {
        public Goblin() : base(new Random().Next(2, 5), "고블린", 7, 4) { } // 고블린 생성자
    }
    public class Dragon : Monster
    {
        public Dragon() : base(new Random().Next(3, 5), "드래곤", 8, 5) { } // 드래곤 생성자
    }
}

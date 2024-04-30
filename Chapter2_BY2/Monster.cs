using Newtonsoft.Json.Bson;

namespace Chapter2_BY2
{
    enum MonsterType
    {
        Minion = 1,
        Goblin,
        Dragon
    }
    public class Monster : ICharacter // 미니언, 고블린, 드래곤의 부모 클래스
    {
        private int hp {  get; set; } // 몬스터 체력 필드

        public int Level { get; } // 몬스터 레벨
        public string Name { get; } // 몬스터 이름
        public int Hp // 몬스터 체력 ( 유효성 검사를 위해 필드랑 프로퍼티를 분리)
        {
            get { return hp; } 
            set
            {
                hp = value <= 0 ? 0 : value;
            }
        } 
        public int Atk { get;} // 몬스터 공격력
        public bool IsDead => Hp <= 0; // 체력이 0 이하인 경우 true

        public Monster(string name, int hpMulti, int attackMulti)
        {
            Level = new Random().Next(1, 6); // 레벨을 1 ~ 5 사이에서 랜덤
            Name = name;
            Hp = Level * hpMulti; // 체력 = 레벨 * 체력 배수
            Atk = Level * attackMulti; // 공격력 = 레벨 * 공격력 배수
        }

        public void TakeDamage(int damage) // 몬스터가 데미지를 받는 메서드
        {
            Hp -= damage;
        } 
    }

    public class Minion : Monster
    {
        public Minion() : base("미니언", 5, 3) { } // 미니언 생성자
    }

    public class Goblin : Monster
    {
        public Goblin() : base("고블린", 7, 4) { } // 고블린 생성자
    }
    public class Dragon : Monster
    {
        public Dragon() : base("드래곤", 8, 5) { } // 드래곤 생성자
    }
}

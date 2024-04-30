using Newtonsoft.Json.Bson;

namespace Chapter2_BY2
{
    enum MonsterType
    {
        Minion = 1,
        Goblin,
        Dragon
    }
    public class Monster : ICharacter
    {
        private int hp {  get; set; }

        public int Level { get; } // 몬스터 레벨
        public string Name { get; } // 몬스터 이름
        public int Hp
        {
            get { return hp; } 
            set
            {
                hp = value <= 0 ? 0 : value;
            }
        } // 몬스터 체력
        public int Attack { get;}
        public bool IsDead => Hp <= 0;

        public Monster(string name, int hpMulti, int attackMulti)
        {
            Level = new Random().Next(1, 6);
            Name = name;
            Hp = Level * hpMulti;
            Attack = Level * attackMulti;
        }

        public void TakeDamage(int damage) // 몬스터가 데미지를 받는 메서드
        {
            Hp -= damage;
        } 
    }

    public class Minion : Monster
    {
        public Minion() : base("미니언", 5, 3) { }
    }

    public class Goblin : Monster
    {
        public Goblin() : base("고블린", 7, 4) { }
    }
    public class Dragon : Monster
    {
        public Dragon() : base("드래곤", 8, 5) { }
    }
}

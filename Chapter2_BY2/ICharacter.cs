namespace Chapter2_BY2
{
    /// <summary>
    /// Player, Monster 공통 인터페이스
    /// </summary>
    internal interface ICharacter
    {
        int Hp { get; set; } // 체력
        int Level { get; } // 레벨
        string Name { get; } // 이름
        int Atk { get; } // 공격력
        bool IsDead { get; } // 죽었는가? 
        void TakeDamage(int damage); // 데미지 피격 메서드

    }
}

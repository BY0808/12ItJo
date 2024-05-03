namespace Chapter2_BY2
{
    /// <summary>
    /// Player, Monster 공통 인터페이스
    /// </summary>
    internal interface ICharacter // Player, Monster 공통 인터페이스
    {
        /// <summary>
        /// 체력 프로퍼티
        /// </summary>
        int Hp { get; set; }
        /// <summary>
        /// 레벨 프로퍼티
        /// </summary>
        int Level { get; }
        /// <summary>
        /// 이름 프로퍼티
        /// </summary>
        string Name { get; }
        /// <summary>
        /// 공격력 프로퍼티
        /// </summary>
        int Atk { get; }
        /// <summary>
        /// 사망 여부 프로퍼티 (Hp가 0 이하일 경우 false)
        /// </summary>>
        bool IsDead { get; }
        /// <summary>
        /// 데미지 피격 메서드
        /// </summary>
        /// <param name="damage">피격 데미지</param>
        void TakeDamage(int damage); // 데미지 피격 메서드

    }
}

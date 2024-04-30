namespace Chapter2_BY2
{
    internal interface ICharacter
    {
        int Hp { get; set; }
        int Level { get; }
        string Name { get; }
        int Atk { get; }
        bool IsDead { get; }
        void TakeDamage(int damage);

    }
}

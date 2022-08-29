public interface IDamageDealable
{
    public int Damage { get; }
    public float DamageMultiplier { get; }
    void ChangeDamageMultiplier(float amount);
    void ChangeDamage(int amount);
}

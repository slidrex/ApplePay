public class AttackingMob : MobEntity, IDamageDealable
{
    public int DamageField;
    public int Damage 
    {
        get => DamageField;
        set
        {
            DamageField = value;
            Damage = value;
        }
    }
    public float DamageMultiplier { get; private set; } = 1;
    public void ChangeDamageMultiplier(float amount) => DamageMultiplier += amount;
    public void ChangeDamage(int amount) => Damage += amount;
}
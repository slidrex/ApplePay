public class AttackingMob : MobEntity, IDamageDealable
{
    public int DamageField;
    public int AttackDamage 
    {
        get => DamageField;
        set
        {
            DamageField = value;
            AttackDamage = value;
        }
    }
    public void AddDamageAttribute() 
    {
        this.AddAttribute("attack_damage", new FloatRef(
            () => AttackDamage,
            val => AttackDamage = (int)val
        ), AttackDamage);
    }
}
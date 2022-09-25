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
        GetComponent<Entity>().AddAttribute("attack_damage", new ReferencedAttribute(
            () => AttackDamage,
            val => AttackDamage = (int)val
        ), AttackDamage);
    }
}
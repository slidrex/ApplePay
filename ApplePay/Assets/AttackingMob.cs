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
    public void AddDamageAttribute() 
    {
        GetComponent<Entity>().AddAttribute("attack_damage", new ReferencedAttribute(
            () => Damage,
            val => Damage = (int)val
        ), Damage);
    }
}
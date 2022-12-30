[UnityEngine.CreateAssetMenu(menuName = "Item/Charm/Other/Shadow Converter", fileName = "new charm")]

public class ShadowConverter : Charm, EntityChangeHealthCallaback
{
    public void BeforeDamageCallback(ref Creature handler, ref Damage[] damages)
    {
        int damage = 0;
        for(int i = 0; i < damages.Length; i++)
        {
            if(damages[i].type == DamageType.Magical)
            {
                int delta = (int)(damages[i].amount * GetFieldValue("conversion"));
                damage += delta;
                damages[i].amount -= delta;
            }
        }
        if(damage != 0)
        {
            Damage[] buffer = new Damage[damages.Length + 1];
            for(int i = 0; i < damages.Length; i++)
            {
                buffer[i] = damages[i];
            }
            buffer[buffer.Length - 1] = new Damage(damage, DamageType.Physical);
            damages = buffer;
        }
    }
    public override void BeginFunction(Creature entity)
    {
        base.BeginFunction(entity);
        entity.damageCallback += BeforeDamageCallback;
    }
    public override void EndFunction(Creature entity)
    {
        entity.damageCallback -= BeforeDamageCallback;
        base.EndFunction(entity);
    }
}

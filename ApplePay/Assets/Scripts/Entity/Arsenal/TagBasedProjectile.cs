public class TagBasedProjectile : Projectile
{
    public float allyDamageMultiplier = 1.0f;
    public float hostileDamageMultiplier = 1.0f;
    protected override void OnBeforeHit(Entity entity, ref int damage)
    {
        Creature currentEntity = entity as Creature;
        if(currentEntity != null)
        {
            if(PayTagHandler.IsAlly(ProjectileOwner, currentEntity)) damage = (int)(damage * allyDamageMultiplier);
            if(PayTagHandler.IsHostile(ProjectileOwner, currentEntity)) damage = (int)(damage * hostileDamageMultiplier);
        }
    }
}

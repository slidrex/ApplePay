public abstract class MobEntity : Creature
{
    protected Entity Target { get; private set; }
    public void SetTarget(Entity entity) => Target = entity;
}

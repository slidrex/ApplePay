public abstract class MobEntity : Creature
{
    protected Entity Target { get; private set; }
    public new MobMovement Movement => (MobMovement)Movement;
    public void SetTarget(Entity entity) => Target = entity;
}

public abstract class MobEntity : Creature
{
    public Entity Target { get => target; private set => target = value; }
    [UnityEngine.SerializeField] private Entity target;
    public new MobMovement Movement => (MobMovement)base.Movement;
    public void SetTarget(Entity entity) => Target = entity;
}

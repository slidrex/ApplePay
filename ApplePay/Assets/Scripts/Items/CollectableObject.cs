using UnityEngine;

public abstract class CollectableObject : ItemEntity
{
    protected abstract bool DestroyOnCollect { get; }
    public bool isCollectable { get; private set; } = true;
    [Header("Pick visual")]
    [SerializeField] private GameObject collectParticle;
    [Header("Physics")]
    protected Vector2 TargetVelocity;
    [SerializeField, Tooltip("Force multiplier coefficient")] private float damagePerForceUnit;
    private const float onCollectFailCollisionIgnoreTime = 1.0f;
    ///<summary>
    ///Item cannot be accessible during the constraint time (constraints are able to stack).
    ///</summary>
    public override void OnHitDetected(HitInfo hitInfo)
    {
        base.OnHitDetected(hitInfo);
        bool collectStatus = true;
        CollisionRequest(hitInfo, ref collectStatus);
    }
    public void AddConstraintCollider(float duration, PayHitShape hitShape)
    {
        HitShape.IgnoreShape(hitShape, duration);
    }
    protected override void Update()
    {
        base.Update();
        HitShape.CheckHit();
    }
    protected virtual void FixedUpdate() => TargetVelocity = rb.velocity;
    protected virtual void OnCollectFail(HitInfo collision)
    {
        
        Rigidbody2D targetRB = collision.entity.ForceHandler?.Rigidbody;
        if(targetRB != null)
            ForceHandler.Knock(collision.normal, ForceHandler.DragIntensity, true);
        DealCollideDamage(collision.entity, (int)(damagePerForceUnit * TargetVelocity.magnitude), null);
        HitShape.IgnoreShape(collision.entity.HitShape, onCollectFailCollisionIgnoreTime);
    }
    public void DealCollideDamage(Entity entity, int damage, Creature dealer) => entity.Damage(damage, DamageType.Physical, dealer);
    public virtual void CollisionRequest(HitInfo collision, ref bool collectStatus) => SendCollectRequest(collision, collectStatus);
    protected void SendCollectRequest(HitInfo collision, bool collectStatus)
    {
        if(collectStatus) OnCollect(collision);
        else OnCollectFail(collision);
    }
    protected virtual void OnCollect(HitInfo collision)
    {
        PayWorld.Particles.InstantiateParticles(collectParticle, transform.position, Quaternion.identity, 2);
        
        if(DestroyOnCollect)
        {
            Destroy(gameObject);
        }
    }
}

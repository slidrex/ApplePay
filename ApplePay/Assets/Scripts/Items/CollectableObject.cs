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
        if(HitShape.IgnoreShapes.ContainsKey(hitShape.M_Collider))
        {
            HitShape.IgnoreShapes[hitShape.M_Collider] = duration;
        }
        else
        {
            HitShape.IgnoreShapes.Add(hitShape.M_Collider, duration);

            foreach(Collider2D collider in hitShape.collisionColliders)
            {
                Pay.Functions.Physics.IgnoreCollision(duration, collider, HitShape.M_Collider);
            }
        }
    }
    protected override void Update()
    {
        base.Update();
        HitShape.CheckHit();
    }
    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
    }
    public void AddForce(Vector2 force) => rb.AddForce(force, ForceMode2D.Impulse);
    protected virtual void FixedUpdate() => TargetVelocity = rb.velocity;
    protected virtual void OnCollectFail(HitInfo collision)
    {
        DealCollideDamage(collision.entity, (int)(damagePerForceUnit * TargetVelocity.magnitude), null);
        
        Rigidbody2D targetRB = collision.entity.GetComponent<Rigidbody2D>();
        if(targetRB != null) targetRB.AddForce(-collision.normal * TargetVelocity.magnitude / targetRB.mass, ForceMode2D.Impulse); 
        AddForce(Vector2.Reflect(TargetVelocity, collision.normal));
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

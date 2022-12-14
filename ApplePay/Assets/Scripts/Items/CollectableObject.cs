using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public abstract class CollectableObject : ItemEntity
{
    [Header("Pick Settings")]
    [SerializeField] private Color32 constraintColor;
    private Color32 storedColor;
    private List<float> ConstraintList = new List<float>();
    public bool isCollectable { get; private set; } = true;
    [Header("Pick visual")]
    [SerializeField] private GameObject collectParticle;
    [Header("Idle Levitation")]
    public float amplitude;
    [SerializeField] private float speed;
    [Header("Physics")]
    protected Vector2 TargetVelocity;
    [SerializeField, Tooltip("Force multiplier coefficient")] private float damagePerForceUnit;
    ///<summary>
    ///Item cannot be accessible during the constraint time (constraints are able to stack).
    ///</summary>
    protected override void Awake()
    {
        base.Awake();
        HitShape = GetComponent<PayHitShape>();
        HitShape.SetResponder(this);
    }
    public override void OnHitDetected(HitInfo hitInfo)
    {
        base.OnHitDetected(hitInfo);
        bool collectStatus = true;
        CollisionRequest(hitInfo, ref collectStatus);
    }
    public void AddConstraint(float duration)
    {
        if(ConstraintList.Count == 0) StoreColor(SpriteRenderer.color);
        ConstraintList.Add(duration);
    }
    public void AddConstraintCollider(float duration, Collider2D collider)
    {
        Pay.Functions.Physics.IgnoreCollision(duration, collider, HitShape.M_Collider);
    }
    public void StoreColor(Color32 color) => storedColor = color;
    protected override void Update()
    {
        base.Update();
        HitShape.CheckHit();
        Levitation();
    }
    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
    }
    protected void Levitation() => transform.position = transform.position + Vector3.up * Time.deltaTime  * Mathf.Sin(Time.time * speed) * amplitude;

    public void AddForce(Vector2 force) => rb.AddForce(force, ForceMode2D.Impulse);
    protected virtual void FixedUpdate() => TargetVelocity = rb.velocity;
    protected virtual void OnCollectFail(HitInfo collision)
    {
        DealCollideDamage(collision.entity, (int)(damagePerForceUnit * TargetVelocity.magnitude), null);
        
        Rigidbody2D targetRB = collision.collider.GetComponent<Rigidbody2D>();
        if(targetRB != null) targetRB.AddForce(-collision.normal * TargetVelocity.magnitude / targetRB.mass, ForceMode2D.Impulse); 
        AddForce(Vector2.Reflect(TargetVelocity, collision.normal));
    }
    public void DealCollideDamage(Entity entity, int damage, Creature dealer) => entity.Damage(damage, DamageType.Physical, dealer);
    public virtual void CollisionRequest(HitInfo collision, ref bool collectStatus) => SendCollectRequest(collision, collectStatus);
    protected void SendCollectRequest(HitInfo collision, bool collectStatus)
    {
        if(collectStatus) OnCollect();
        else OnCollectFail(collision);
    }
    protected virtual void OnCollect()
    {
        PayWorld.Particles.InstantiateParticles(collectParticle, transform.position, Quaternion.identity, 2);
        
        Destroy(gameObject);
    }
}

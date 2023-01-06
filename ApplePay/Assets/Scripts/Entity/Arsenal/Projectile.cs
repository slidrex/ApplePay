using UnityEngine;

[RequireComponent(typeof(PayHitBox), typeof(PayForceHandler))]
public class Projectile : MonoBehaviour, IHitResponder
{
    public Creature ProjectileOwner { get; private set; }
    public float LifeTime;
    public int Damage;
    [Header("Velocity Settings")]
    public float Acceleration;
    public float Speed;
    public float MinSpeed;
    public float MaxSpeed;
    protected Rigidbody2D rb;
    protected Animator Animator;
    public PayHitBox HitBox;
    public PayForceHandler ForceHandler;
    public Transform Target {get; private set;}
    [SerializeField] private GameObject destroyParticle;
    [SerializeField] private DamageType damageType;
    [HideInInspector] public Vector2 MoveVector { get; private set; }
    public bool SetMoveRotation;
    public float KnockbackForce;
    public float KnockbackDamping;
    private void Awake()
    {
        HitBox.AddResponder(this);
    }
    public void Setup(Vector2 moveVector, Creature owner, Transform target)
    {
        SetOwner(owner);
        SetMoveVector(moveVector);
        Target = target;
    }
    public void DisableOwnerCollisions()
    {
        HitBox.IgnoreShape(ProjectileOwner.HitShape);
    }
    public void SetTarget(Transform target) => Target = target;
    public void SetOwner(Creature owner) => ProjectileOwner = owner;
    public void SetMoveVector(Vector2 vector) => MoveVector = vector;
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        if(SetMoveRotation) Pay.Functions.Math.Atan3(MoveVector.y, MoveVector.x);
    }
    protected virtual void Update()
    {
        HandleLifeTime();
        HitBox.CheckHit();
    }
    protected virtual void HandleLifeTime()
    {
        LifeTime -= Time.deltaTime;
        if(LifeTime <= 0)
            Destroy(gameObject);
    }
    protected virtual void FixedUpdate()
    {
        float a = Acceleration * Time.fixedDeltaTime;
        Vector2 resultVec = MoveVector * Speed;
        Speed += a;
        Speed = Mathf.Clamp(Speed, MinSpeed, MaxSpeed);

        rb.velocity = resultVec;
    }
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
    protected virtual void OnDestroy() => PayWorld.Particles.InstantiateParticles(destroyParticle, transform.position, Quaternion.identity, 1f);

    public void OnHitDetected(HitInfo hitInfo)
    {
        if(hitInfo.entity != ProjectileOwner)
        {
            int resultDamage = Damage;
            OnBeforeHit(hitInfo.entity, ref resultDamage);
            
            if(hitInfo.entity.ForceHandler != null && KnockbackForce != 0)
                hitInfo.entity.ForceHandler.Knock(hitInfo.normal * KnockbackForce, KnockbackDamping);

            hitInfo.entity.Damage(resultDamage, damageType, ProjectileOwner);
            Destroy(gameObject);
        }
    }
    protected virtual void OnBeforeHit(Entity damageEntity, ref int damage) { }
}
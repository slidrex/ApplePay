using UnityEngine;

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
    public Transform Target {get; private set;}
    [SerializeField] private GameObject destroyParticle;
    [SerializeField] private DamageType damageType;
    [HideInInspector] public Vector2 MoveVector { get; private set; }
    public bool SetMoveRotation;
    private void Awake()
    {
        HitBox.SetResponder(this);
    }
    public void Setup(Vector2 moveVector, Creature owner, Transform target)
    {
        SetOwner(owner);
        SetMoveVector(moveVector);
        Target = target;
    }
    public void SetTarget(Transform target) => Target = target;
    public void SetOwner(Creature owner) => ProjectileOwner = owner;
    public void SetMoveVector(Vector2 vector) => MoveVector = vector;
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        Physics2D.IgnoreLayerCollision(12, 12);
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
        hitInfo.entity.Damage(Damage, damageType, ProjectileOwner);
        Destroy(gameObject);
    }
}
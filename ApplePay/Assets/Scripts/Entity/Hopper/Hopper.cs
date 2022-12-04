using UnityEngine;
public class Hopper : AttackingMob, ICollideDamageDealer
{
    private SimplifiedWeaponHolder holder;
    private Animator anim;
    private MobMovement movement;
    [HideInInspector] public float switchStateInterval = 1.5f, timeSinceSwitch;
    [SerializeField] private float seenRadius;
    public int CollideDamage {get; set;} = 15;
    private const float AttackStateSpeed = 6f;
    private const float MovingStateSpeed = 10f;
    protected override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
        holder = GetComponent<SimplifiedWeaponHolder>();
        movement = GetComponent<MobMovement>();
        SetTarget(GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>());
    }
    protected override void Update()
    {
        base.Update();
        if(timeSinceSwitch < switchStateInterval)
        {
            timeSinceSwitch += Time.deltaTime;
            if(timeSinceSwitch > switchStateInterval)
            {
                GetRandomState();
                switchStateInterval = Random.Range(4, 7);
                timeSinceSwitch = 0;
            }
        }
    }
    private void GetRandomState()
    {
        System.Array val = System.Enum.GetValues(typeof(AttackState));
        System.Random rand = new System.Random();
        AttackState currentState = (AttackState)val.GetValue(rand.Next(val.Length));
        SetState(currentState);
    }
    private void SetState(AttackState state)
    {
        if(state == AttackState.Attack)
        {
            anim.SetBool("inAttack", true);
            movement.CurrentSpeed = AttackStateSpeed;
            Collider2D[] col = Physics2D.OverlapCircleAll(transform.position, seenRadius);
            foreach (Collider2D collider in col)
            {
                if(Target != null && collider.gameObject == Target.gameObject)
                {
                    holder.Activate(this, ref holder.ActiveWeapon, collider.transform.position, collider.transform, out Projectile projectile);
                }
            }
        }
        else if(state == AttackState.Moving)
        {
            anim.SetBool("inAttack", false);
            movement.CurrentSpeed = MovingStateSpeed;
        } 
    }
    public override void OnHitDetected(HitInfo hitInfo)
    {
        if(hitInfo.entity == Target)
        {
            DealCollideDamage(hitInfo.entity, CollideDamage, this);
            Pay.Functions.Physics.IgnoreCollision(0.6f, GetComponent<Collider2D>(), hitInfo.collider);
            
        }
    }
    public void DealCollideDamage(Entity entity, int damage, Creature dealer) => entity?.Damage(damage, DamageType.Physical, dealer);

    private enum AttackState
    {
        Moving,
        Attack
    }
}
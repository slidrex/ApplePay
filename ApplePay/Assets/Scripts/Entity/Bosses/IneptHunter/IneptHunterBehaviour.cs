using UnityEngine;
using Pay.Camera;
public class IneptHunterBehaviour : AttackingMob
{
    private Rigidbody2D rb;
    private Animator animator;
    private Vector3 dist;
    private Collider2D other;
    private SimplifiedWeaponHolder weapon;
    private AttackStates curAttackState;
    [SerializeField] private Attacks curState;
    [SerializeField] private GameObject shockWave;
    [SerializeField] private GameObject obstacleCollisionParticles;
    [SerializeField] private GameObject dustAfterJump;
    [SerializeField] private float maxTime = 3, curTime = 0;
    private float viewAngle;
    private int collisionDamage = 15;
    private const int obstacleLayer = 6;

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        weapon = GetComponent<SimplifiedWeaponHolder>();
        animator = GetComponent<Animator>();
        Hostiles.Add(FindObjectOfType<PlayerEntity>());
        SetTarget(FindTarget().GetComponent<Entity>());
    }
    protected override void Update()
    {
        base.Update();
        CheckAttackStates();
        if(curTime < maxTime && curState == Attacks.Idle) 
        {
            animator.SetBool("IsIdle", true);
            SetAnimatorParameters();
            curTime += Time.deltaTime;
            if(curTime > maxTime)
            {
                animator.SetBool("IsIdle", false);
                RandomState();
                maxTime = Random.Range(1, 2.5f);
                curTime = 0;
                CheckStates();
            }
        }
    }
    private void CheckStates()
    {
        switch(curState)
        {
            case Attacks.HeavyAttack:
            {
                animator.SetTrigger("Jump");
                break;
            }
            case Attacks.HeavyAttack2:
            {
                CalcDist();
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, Pay.Functions.Math.Atan3(dist.y, dist.x));
                animator.SetTrigger("HA2Jump");
                break;
            }
            case Attacks.LiteAttack:
            {
                viewAngle = 100;
                dist = Mathf.Sign(Target.transform.position.y - transform.position.y) * Vector2.up;
                Collider2D[] colls = Physics2D.OverlapCircleAll(transform.position, 25);
                foreach(Collider2D enemy in colls)
                {
                    if(enemy.gameObject == Target.gameObject)
                    {
                        float angle = Vector2.Angle(dist, CalcDist().normalized);
                        if(angle <= viewAngle / 2)
                        {
                            animator.SetTrigger("LAttack");
                            SetAnimatorParameters();
                            break;
                        }
                        else
                        {
                            curState = Attacks.HeavyAttack;
                            CheckStates();
                            break;
                        }
                    }
                }
                break;
            }
        }

    }
    private void CheckAttackStates()
    {
        switch (curAttackState)
        {
            case AttackStates.Fall: 
            {
                rb.velocity = Vector2.down * 30;
                if(transform.position.y <= Target.transform.position.y)
                    Collision();
                break;
            }
            case AttackStates.HA2Fly:
            {
                rb.velocity = dist.normalized * Pay.Functions.Math.ClampVectorComponents(dist, 22, 23);
                break;
            }
        }
    }
    private GameObject FindTarget()
    {
        GameObject returnValue = null;
        float minDist = 0;
        Creature hostile = Hostiles.Find(M => M != null);
        if(hostile != null)
        {
            returnValue = hostile.gameObject;
            minDist = Vector2.Distance(transform.position, hostile.gameObject.transform.position);
        }
        foreach (Creature item in Hostiles)
        {
            if(item != null)
            if(Vector2.Distance(item.gameObject.transform.position, transform.position) < minDist)
            {
                returnValue = item.gameObject;
            }
        }
        return returnValue;
    }
    public void Jump()
    {
        PayWorld.Particles.InstantiateParticles(dustAfterJump, new Vector2(transform.position.x, transform.position.y + 0.75f), Quaternion.identity, 0.85f);
        GetComponent<Collider2D>().enabled = false;
        rb.velocity = Vector2.up * 30;
        Invoke("Fall", 1.4f);
    }
    private void Fall()
    {
        transform.position = new Vector2(Target.transform.position.x, transform.position.y);
        animator.SetTrigger("Fall");
        curAttackState = AttackStates.Fall;
    }
    private void Collision()
    {
        animator.SetTrigger("Collision");
        rb.velocity = Vector3.zero;
        PayWorld.Particles.InstantiateParticles(shockWave, new Vector2(transform.position.x, transform.position.y - 0.6f), Quaternion.identity, 0.9f);
        StartCoroutine(CameraShake.SmoothShake(0, 6, 6, 0, 6));
        GetComponent<Collider2D>().enabled = true;
        curAttackState = AttackStates.Nothing;
    }
    public void HA2Jump()
    {
        curAttackState = AttackStates.HA2Fly;
    }
    public void ThrowSpear()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25);
        foreach (Collider2D col in colliders)
        {
            if(col.gameObject.Equals(Target.gameObject))
                weapon.Activate(this, ref weapon.ActiveWeapon, col.transform.position, col.transform);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(curAttackState == AttackStates.HA2Fly && other.gameObject.layer == obstacleLayer)
        {
            PayWorld.Particles.InstantiateParticles(obstacleCollisionParticles, other.contacts[0].point,
                 Quaternion.Euler(new Vector3(0, 0, Pay.Functions.Math.Atan3(dist.y, dist.x))), 0.35f);
            rb.velocity = Vector2.zero;
            StartCoroutine(CameraShake.SmoothShake(0, 3.5f, 3.5f, 0, 6));
            animator.SetTrigger("HA2Collision");
            curAttackState = AttackStates.Nothing;
        }
        else if(curAttackState == AttackStates.HA2Fly && other.gameObject.layer != obstacleLayer)
        {
            if(other.gameObject.GetComponent<Creature>() != null)
                other.gameObject.GetComponent<Creature>().Damage(collisionDamage, DamageType.Physical, this);
            Physics2D.IgnoreCollision(other.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>(), true);
            this.other = other.gameObject.GetComponent<Collider2D>();
            Invoke("EnableCollision", 1f);
        }
    }
    private void RandomState()
    {
        System.Array arr = System.Enum.GetValues(typeof(Attacks));
        System.Random rand = new System.Random();
        curState = (Attacks)arr.GetValue(rand.Next(arr.Length));
    }
    private void SetIdleState() => curState = Attacks.Idle;
    private Vector2 CalcDist() =>
         dist = Target.transform.position - transform.position;

    private void RotateEulerAngles()
    {
        if(CalcDist().x < 0) transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, transform.eulerAngles.z);
        else if(CalcDist().x > 0) transform.eulerAngles = new Vector3(transform.eulerAngles.x, 180, transform.eulerAngles.z);
    }
    private void SetDefaultEulerAngler() =>
        transform.eulerAngles = new Vector3(0, 0, 0);
    private void SetAnimatorParameters()
    {
        animator.SetInteger("Horizontal", (int)CalcDist().x);
        animator.SetInteger("Vertical", (int)CalcDist().y);
    }

    private void EnableCollision() => 
        Physics2D.IgnoreCollision(other.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>(), false);
        
    private enum AttackStates
    {
        Nothing, Fall, HA2Fly
    }
    private enum Attacks
    {
        Idle,
        HeavyAttack,
        HeavyAttack2, 
        LiteAttack
    }
}
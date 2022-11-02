using UnityEngine;

public class Chain : AttackingMob
{
    [SerializeField] private GameObject dust;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float radius;
    private Vector2 dist => Target.transform.position - transform.position;
    private Vector2 fixedDist;
    private ChainStates states;
    private float maxTime = 2.5f, curTime = 0;

    protected override void Start()
    {
        base.Start();
        states = ChainStates.Idle;
        SetTarget(FindObjectOfType<PlayerEntity>());
    }
    protected override void Update()
    {
        base.Update();
        if(dist != Vector2.zero) fixedDist = dist;
        Timer();
    }
    private void RandomizeAttack() => states = (ChainStates)Pay.Functions.Generic.GetRandomizedEnum(states, 1, System.Enum.GetValues(states.GetType()).Length);
    private void Timer()
    {
        if(curTime < maxTime)
        {
            curTime += Time.deltaTime;
            if(curTime > maxTime)
            {
                RandomizeAttack();
                AttackStates();
                maxTime = Random.Range(2.7f, 3f);
                curTime = 0;
            }
        }
    }
    private void AttackStates()
    {
        switch(states)
        {
            case ChainStates.DoubleAttack:
            {
                Movement.animator.SetTrigger("DoubleAttack");
                break;
            }
            case ChainStates.TripleAttack:
            {
                Movement.animator.SetTrigger("TripleAttack");
                break;
            }
        }
    }
    private void AttackChain()
    {
        Collider2D[] col = Physics2D.OverlapCircleAll(attackPoint.position, radius);
        foreach (Collider2D enemy in col)
        {
            if(enemy.GetComponent<Creature>() != null && enemy.GetComponent<Chain>() == null) 
                enemy.GetComponent<Creature>().Damage(DamageField, DamageType.Physical, this);
        }
    }
    private void Dash()
    {
        //PayWorld.Particles.InstantiateParticles(dust, transform.position, Quaternion.identity, 2);
        Movement.Rigidbody.velocity = dist.normalized * Pay.Functions.Math.ClampVectorComponents(dist, 35, 45);
    }
    public void AttackEnd()
    {
        Movement.DisablePatterns(false);
        states = ChainStates.Idle;
    }
    public void UpdateAnimatorParameters()
    {
        fixedDist.x = fixedDist.x < 0 ? -1 : 1;
        fixedDist.y = fixedDist.y < 0 ? -1 : 1;
        Movement.animator.SetInteger("Horizontal", (int)fixedDist.x);
        Movement.animator.SetInteger("Vertical", (int)fixedDist.y);
    }
    private void ThirdAttack() => Movement.animator.SetTrigger("ThirdAttack");
    private void OnDrawGizmosSelected() => Gizmos.DrawWireSphere(attackPoint.position, radius);
}
enum ChainStates
{
    Idle,
    DoubleAttack,
    TripleAttack
}
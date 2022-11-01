using UnityEngine;

public class Chain : AttackingMob
{
    [SerializeField] private GameObject dust;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float radius;
    private MobMovement movement;
    private Vector2 dist;
    private ChainStates states;
    private float maxTime = 2.5f, curTime = 0;

    protected override void Start()
    {
        base.Start();
        states = ChainStates.Idle;
        movement = GetComponent<MobMovement>();
        SetTarget(FindObjectOfType<PlayerEntity>());
    }
    protected override void Update()
    {
        base.Update();
        Timer();
    }
    private void RandomizationAttack()
    {
        System.Array arr = System.Enum.GetValues(typeof(ChainStates));
        System.Random rand = new System.Random();
        states = (ChainStates)arr.GetValue(rand.Next(arr.Length - 1));
    }
    private void Timer()
    {
        if(curTime < maxTime)
        {
            curTime += Time.deltaTime;
            if(curTime > maxTime)
            {
                RandomizationAttack();
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
            /*case ChainStates.DoubleAttack:
            {
                movement.animator.SetTrigger("DoubleAttack");
                break;
            }*/
            case ChainStates.TripleAttack:
            {
                movement.animator.SetTrigger("TripleAttack");
                break;
            }
            case ChainStates.Idle:
            {
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
        movement.Rigidbody.velocity = dist.normalized * Pay.Functions.Math.ClampVectorComponents(dist, 35, 45);
    }
    public void AttackEnd()
    {
        GetComponent<CrossMovement>().enabled = true;
        states = ChainStates.Idle;
    }
    public Vector2 CalcDist() => dist = Target.transform.position - transform.position;
    public void UpdateAnimatorParameters()
    {
        movement.animator.SetInteger("Horizontal", (int)CalcDist().x);
        movement.animator.SetInteger("Vertical", (int)CalcDist().y);
    }
    private void ThirdAttack() => movement.animator.SetTrigger("ThirdAttack");
    private void OnDrawGizmosSelected() => Gizmos.DrawWireSphere(attackPoint.position, radius);
}
enum ChainStates
{
    //DoubleAttack
    TripleAttack, Idle
}
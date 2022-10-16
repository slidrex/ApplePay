using UnityEngine;

public class Chain : AttackingMob
{
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float radius;
    private MobMovement movement;
    private Vector2 dist;
    protected override void Start()
    {
        base.Start();
        movement = GetComponent<MobMovement>();
        SetTarget(FindObjectOfType<PlayerEntity>());
    }
    protected override void Update()
    {
        base.Update();
        if(Input.GetKeyDown(KeyCode.P)) GetComponent<Animator>().SetTrigger("DoubleAttack");
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
        movement.Rigidbody.velocity = dist.normalized * Pay.Functions.Math.ClampVectorComponents(dist, 12, 13);
    }
    public void AttackEnd()
    {

    }
    public void CalcDist() => dist = Target.transform.position - transform.position;
    private void OnDrawGizmosSelected() => Gizmos.DrawWireSphere(attackPoint.position, radius);
}

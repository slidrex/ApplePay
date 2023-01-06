using UnityEngine;
using System.Linq;
public class Violet : BossEntity
{
    [SerializeField] private GameObject dust;
    [SerializeField] private ParticleSystem swordParticles;
    [HideInInspector] public byte DisableID;
    private Attacks attacks;
    private States states = States.Free;
    private float maxTime = 2, curTime;
    public float attackDistance, attackRange;
    protected override void Update()
    {
        base.Update();
        if(Target == null) Target = GetNearestHostileTarget();
        Timer();
    }
    private void Timer()
    {
        if(curTime < maxTime && states == States.Free)
        {
            curTime += Time.deltaTime;
            if(curTime > maxTime && SquareDistanceToTarget() < (attackDistance * attackDistance))
            {
                RandomizeAttack();
                ResetTimer(1.25f, 2f);
            }
            else if(curTime > maxTime && SquareDistanceToTarget() > (attackDistance * attackDistance))
            {
                attacks = Attacks.DashAttack;
                ResetTimer(1f, 1.4f);
            }
        }
    }
    private void ResetTimer(float max, float min)
    {
        UpdateAttackState();
        curTime = 0;
        maxTime = Random.Range(max, min);
    }
    public float SquareDistanceToTarget() => Vector2.SqrMagnitude(Target.transform.position - transform.position);
    private void UpdateAttackState()
    {
        states = States.Busy;
        switch(attacks)
        {
            case Attacks.LiteAttack: 
            {
                Animator.SetTrigger("LiteAttack");
                break;
            }
            case Attacks.SpinAttack: 
            {
                Animator.SetTrigger("SpinAttack");
                break;
            }
            case Attacks.DashAttack:
            {
                Animator.SetTrigger("DashAttack");
                break;
            }
        }
    }
    private void Attack()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(swordParticles.transform.position, attackRange);
        foreach (Collider2D entity in colliders)
        {
            if(entity.GetComponent<Entity>() != null && entity.gameObject != gameObject)
                entity.GetComponent<Entity>().Damage(DamageField, DamageType.Physical, this);
        }
    }
    public void AttackEnd()
    {
        Movement.RemoveDisable(DisableID);
        states = States.Free;
    }
    private void Dash()
    {
        rb.velocity = Movement.MoveVector * 25;
        PayWorld.Particles.InstantiateParticles(dust, new Vector2(transform.position.x, transform.position.y - 1.1f), Quaternion.identity, 0.5f, transform);
    }
    private void ActivatePartcles() => swordParticles.Play();
    private void DeactivatePartcles() => swordParticles.Stop();
    private void RandomizeAttack() => attacks = (Attacks)Pay.Functions.Generic.GetRandomizedEnum(attacks, 0, System.Enum.GetValues(attacks.GetType()).Length - 1);
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(swordParticles.transform.position, attackRange);
        Gizmos.DrawWireSphere(transform.position, attackDistance);
    } 
        
}
enum Attacks
{
    LiteAttack,
    SpinAttack,
    DashAttack
}
enum States
{
    Free,
    Busy
}
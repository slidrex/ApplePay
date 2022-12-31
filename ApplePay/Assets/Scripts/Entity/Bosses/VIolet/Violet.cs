using UnityEngine;
public class Violet : AttackingMob
{
    [SerializeField] private GameObject dust;
    [SerializeField] private ParticleSystem swordParticles;
    private Attacks attacks;
    private States states = States.Free;
    public byte DisableID;
    private float maxTime = 2, curTime;
    public float attackDistance;
    protected override void Update()
    {
        base.Update();
        Timer();
    }
    private void Timer()
    {
        if(curTime < maxTime && states == States.Free)
        {
            curTime += Time.deltaTime;
            if(curTime > maxTime && DistanceToTarget() < attackDistance)
            {
                RandomizeAttack();
                ResetTimer(1.25f, 2f);
            }
            else if(curTime > maxTime && DistanceToTarget() > attackDistance)
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
    public float DistanceToTarget() => Vector2.Distance(Target.transform.position, transform.position);
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
    private void OnDrawGizmos() => Gizmos.DrawWireSphere(transform.position, attackDistance);
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
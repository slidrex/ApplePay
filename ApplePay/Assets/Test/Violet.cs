using UnityEngine;
public class Violet : AttackingMob
{
    private Attacks attacks;
    private States states;
    public byte DisableID;
    private float maxTime = 2.5f, curTime;

    protected override void Start()
    {
        base.Start();
    }
    protected override void Update()
    {
        base.Update();
        if(curTime < maxTime)
        {
            curTime += Time.deltaTime;
            if(curTime > maxTime)
            {
                RandomizeAttack();
                UpdateAttackState();
                curTime = 0;
            }
        }
    }
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
        }
    }
    public void AttackEnd()
    {
        Movement.RemoveDisable(DisableID);
        states = States.Free;
    }
    private void RandomizeAttack() => attacks = (Attacks)Pay.Functions.Generic.GetRandomizedEnum(attacks, 0, System.Enum.GetValues(attacks.GetType()).Length);
}
enum Attacks
{
    LiteAttack,
    SpinAttack
}
enum States
{
    Free,
    Busy
}
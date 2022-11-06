using UnityEngine;

public class ConfusionState : StateMachineBehaviour
{
    private Chain chain;
    private MobMovement movement;
    private bool awaken;
    private int oldHealth;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(awaken == false)
        {
            if(chain == null) chain = animator.GetComponent<Chain>();
            movement = animator.GetComponent<MobMovement>();
            awaken = true;
        }
        movement.CurrentSpeed = Mathf.PI;
        Physics2D.IgnoreCollision(movement.Target.GetComponent<Collider2D>(), animator.GetComponent<Collider2D>(), false);
        oldHealth = chain.CurrentHealth;
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(oldHealth > chain.CurrentHealth)
        {
            chain.AttackEnd();
            movement.SetActivePatterns(0);
            movement.animator.SetBool("UltimateAttack", false);
        }
    }
}
using UnityEngine;

public class DoubleAttack : StateMachineBehaviour
{
    private MobMovement movement;
    private Chain chain;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        chain = animator.GetComponent<Chain>();
        movement = animator.GetComponent<MobMovement>();
        movement.isDisabled = true;
        chain.CalcDist();
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        movement.isDisabled = false;
        chain.AttackEnd();
    }
}
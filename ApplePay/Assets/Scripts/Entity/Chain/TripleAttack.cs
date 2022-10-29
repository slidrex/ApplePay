using UnityEngine;

public class TripleAttack : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<MobMovement>().isDisabled = true;
        animator.GetComponent<Chain>().CalcDist();
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<MobMovement>().isDisabled = false;
        animator.GetComponent<Chain>().AttackEnd();
    }
}
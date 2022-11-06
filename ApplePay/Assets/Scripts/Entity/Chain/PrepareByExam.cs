using UnityEngine;

public class PrepareByExam : StateMachineBehaviour
{
    private MobMovement mobMovement;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        mobMovement = animator.GetComponent<MobMovement>();
        mobMovement.DisablePatterns(true);
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        mobMovement.DisablePatterns(false);
    }
}
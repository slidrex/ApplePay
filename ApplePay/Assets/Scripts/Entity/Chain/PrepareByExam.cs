using UnityEngine;

public class PrepareByExam : StateMachineBehaviour
{
    private Chain chain;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        chain = animator.GetComponent<Chain>();
        chain.DisableID = chain.Movement.AddDisable();
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        chain.Movement.RemoveDisable(chain.DisableID);
    }
}
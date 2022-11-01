using UnityEngine;

public class IntermediateAttack : StateMachineBehaviour
{
    private MobMovement mobMovement;
    private Chain chain;
    private bool awaken;
    private byte disableID;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(awaken == false)
        {
            if(mobMovement == null) mobMovement = animator.GetComponent<MobMovement>();
            if(chain == null) chain = animator.GetComponent<Chain>();
            awaken = true;
        }
        disableID = mobMovement.AddDisable();
        animator.GetComponent<CrossMovement>().enabled = false;
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector2 targetPos = animator.GetComponent<MovePatterns>().GetTarget().position;
        chain.UpdateAnimatorParameters();
        if(animator.transform.position.x < targetPos.x)
        {
            animator.transform.eulerAngles = new Vector2(0, 0);
        }
        else
        {
            animator.transform.eulerAngles = new Vector2(0, 180);
        }
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        mobMovement.RemoveDisable(disableID);
    }
}
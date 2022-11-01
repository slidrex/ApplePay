using UnityEngine;

public class IntermediateAttack : StateMachineBehaviour
{
    private MobMovement mobMovement;
    private Chain chain;
    private bool awaken;
    private byte disableID;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(awaken == false)
        {
            if(mobMovement == null) mobMovement = animator.GetComponent<MobMovement>();
            if(chain == null) chain = animator.GetComponent<Chain>();
            awaken = true;
        }
        disableID = mobMovement.AddDisable();
        chain.CalcDist();
        animator.GetComponent<CrossMovement>().enabled = false;
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector2 dist = animator.GetComponent<MovePatterns>().GetTarget().position - animator.transform.position;
        chain.UpdateAnimatorParameters();
        if(animator.transform.position.x < dist.x)
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
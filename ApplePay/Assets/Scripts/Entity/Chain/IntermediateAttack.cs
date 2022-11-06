using UnityEngine;

public class IntermediateAttack : StateMachineBehaviour
{
    private MobMovement mobMovement;
    private Chain chain;
    private bool awaken;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(awaken == false)
        {
            if(mobMovement == null) mobMovement = animator.GetComponent<MobMovement>();
            if(chain == null) chain = animator.GetComponent<Chain>();
            awaken = true;
        }
        chain.DisableID = mobMovement.AddDisable();
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector2 targetPos = mobMovement.Target.position;
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
        mobMovement.RemoveDisable(chain.DisableID);
    }
}
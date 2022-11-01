using UnityEngine;

public class ChainAttack : StateMachineBehaviour
{
    private Chain chain;
    private bool awaken;
    private byte disableID;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(awaken == false)
        {
            if(chain == null) chain = animator.GetComponent<Chain>();
            awaken = true;
        }
        disableID = chain.Movement.AddDisable();
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        chain.Movement.RemoveDisable(disableID);
        chain.AttackEnd();
    }
}
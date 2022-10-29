using UnityEngine;

public class DoubleAttack : StateMachineBehaviour
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
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        mobMovement.RemoveDisable(disableID);
        chain.AttackEnd();
    }
}
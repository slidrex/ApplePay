using UnityEngine;

public class UltimateAttack : StateMachineBehaviour
{
    private Chain chain;
    private ReflectMovement reflectMovement;
    private bool awaken;
    private byte disableID;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(awaken == false)
        {
            if(chain == null) chain = animator.GetComponent<Chain>();
            reflectMovement = animator.GetComponent<ReflectMovement>();
            awaken = true;
        }
        chain.UltimateTrail.SetActive(true);
        disableID = chain.Movement.AddDisable();
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //chain.Movement.Rigidbody.velocity = reflectMovement.MovementVector.normalized * 
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        chain.UltimateTrail.SetActive(false);
        chain.Movement.RemoveDisable(disableID);
        chain.AttackEnd();
    }
}
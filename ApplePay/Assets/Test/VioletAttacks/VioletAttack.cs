using UnityEngine;

public class VioletAttack : StateMachineBehaviour
{
    private Violet violet;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        violet = animator.GetComponent<Violet>();
        violet.DisableID = violet.Movement.AddDisable();
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        violet.AttackEnd();
    }
}

using UnityEngine;

public class VioletDashAttack : StateMachineBehaviour
{
    private Violet violet;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        violet = animator.GetComponent<Violet>();
        violet.DisableID = violet.Movement.AddDisable();
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        int random = (int)Random.Range(0, 2);
        if(random > 0 && violet.DistanceToTarget() < violet.attackDistance)
        {
            violet.Movement.RemoveDisable(violet.DisableID);
            violet.Movement.animator.SetTrigger("SpinAttack");
        }
        else violet.AttackEnd();
    }
}

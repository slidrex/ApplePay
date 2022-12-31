using UnityEngine;

public class VioletSpinAttack : StateMachineBehaviour
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
            Vector2 dist = violet.Target.transform.position - animator.transform.position;
            violet.Movement.MoveVector = Mathf.Abs(dist.x) > Mathf.Abs(dist.y) ? Vector2.right * Mathf.Sign(dist.x) : Vector2.up * Mathf.Sign(dist.y);
            violet.Movement.animator.SetTrigger("LiteAttack");
        }
        else violet.AttackEnd(); 
    }
}
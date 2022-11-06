using UnityEngine;
public class UltimateAttack : StateMachineBehaviour
{
    private Chain chain;
    private MobMovement mobMovement;
    private bool awaken;
    private float maxTime, curTime;
    private float timeToUpdateVector = 0.7f, timeUpdateVector;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(awaken == false)
        {
            if(chain == null) chain = animator.GetComponent<Chain>();
            mobMovement = animator.GetComponent<MobMovement>();
            awaken = true;
        }
        animator.GetComponent<ReflectMovement>().
            CollsionsManipulation(animator.GetComponent<Collider2D>(), chain.Movement.Target.GetComponent<Collider2D>(), true, 0.5f);
        chain.UltimateTrail.SetActive(true);
        maxTime = Random.Range(5, 7);
        mobMovement.SetActivePatterns(1);
        mobMovement.CurrentSpeed = 37;
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(curTime < maxTime) 
        {
            curTime += Time.deltaTime;
            if(curTime > maxTime)
            {
                chain.UltimateTrail.SetActive(false);
                mobMovement.DisablePatterns(true);
                curTime = 0;
                mobMovement.animator.SetTrigger("ConfusionState");
            }
        }
        if(timeUpdateVector < timeToUpdateVector)
        {
            timeUpdateVector += Time.deltaTime;
            if(timeUpdateVector > timeToUpdateVector)
            {
                mobMovement.MoveVector = (mobMovement.Target.position - animator.transform.position).normalized;
                timeUpdateVector = 0;
            }
        }
    }
}
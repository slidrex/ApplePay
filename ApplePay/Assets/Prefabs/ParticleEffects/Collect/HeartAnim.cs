using UnityEngine;

public class HeartAnim : TrackAnim
{
    [HideInInspector] public Creature EntityApply;
    [HideInInspector] public Animator HealthBarAnim;
    [HideInInspector] public int HealAmount; 
    protected override void OnAimDestinate()
    {
        EntityApply.ChangeHealth(HealAmount);
        HealthBarAnim.SetTrigger("Take");
        base.OnAimDestinate();
    }
}
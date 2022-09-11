using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartAnim : TrackAnim
{
    [HideInInspector] public Creature EntityApply;
    [HideInInspector] public Animator HealthBarAnim;
    [HideInInspector] public int HealAmount; 
    protected override void AimDestinate()
    {
        EntityApply.ChangeHealth(HealAmount);
        HealthBarAnim.SetTrigger("Take");
        base.AimDestinate();
    }
}

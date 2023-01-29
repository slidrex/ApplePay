using UnityEngine;
using UnityEngine.UI;

public class HealthBar : EntityIndicatorBar
{
    public Transform MagnitizedObj;
    public Animator Animator;
    protected override float BarCurrentValue => entity.CurrentHealth;
    protected override float BarMaxValue => entity.MaxHealth;
}
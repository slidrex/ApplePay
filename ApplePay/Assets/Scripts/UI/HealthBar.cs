using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Transform MagnitizedObj;
    public Animator Animator;
    [SerializeField] private Image value;
    [SerializeField] private Creature entityScript;
    [SerializeField] private Pay.UI.UIHolder holder;
    private Pay.UI.IndicatorObject indicatorObject;
    public void IndicatorSetup() => indicatorObject = Pay.UI.UIManager.Indicator.RegisterIndicator(holder, value);
    public void IndicatorUpdate() => Pay.UI.UIManager.Indicator.UpdateIndicator(indicatorObject, entityScript.CurrentHealth, entityScript.MaxHealth);
}
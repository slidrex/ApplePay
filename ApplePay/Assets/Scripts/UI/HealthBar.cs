using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Transform MagnitizedObj;
    public Animator Animator;
    [SerializeField] private Image value;
    [SerializeField] private Entity entity;
    private Pay.UI.IndicatorObject indicatorObject;
    public void IndicatorSetup() => indicatorObject = Pay.UI.UIManager.Indicator.RegisterIndicator(entity.GetComponent<IUIHolder>()?.GetHolder(), value);
    public void IndicatorUpdate() => Pay.UI.UIManager.Indicator.UpdateIndicator(indicatorObject, entity.CurrentHealth, entity.MaxHealth);
}
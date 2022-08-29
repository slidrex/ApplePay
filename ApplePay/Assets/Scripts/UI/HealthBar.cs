using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{
    public Transform MagnitizedObj;
    public Animator Animator;
    [SerializeField] private Image value;
    [SerializeField] private Creature entityScript;
    [SerializeField] private Pay.UI.UIHolder holder;
    private byte healthBarId;
    public void IndicatorSetup() => Pay.UI.UIManager.Indicator.RegisterIndicator(holder, value, out healthBarId);
    public void IndicatorUpdate() => Pay.UI.UIManager.Indicator.UpdateIndicator(holder, healthBarId, entityScript.CurrentHealth, entityScript.MaxHealth);
}
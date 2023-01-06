using UnityEngine.UI;
using UnityEngine;

public class BossBar : MonoBehaviour
{
    [SerializeField] private Image value;
    public BossEntity entity;
    public Animator animator;
    public BossEntity SetEntity(BossEntity ent) => entity = ent;
    private Pay.UI.IndicatorObject indicatorObject;
    public Image GetValue() => value;
    public void IndicatorSetup() => indicatorObject = Pay.UI.UIManager.Indicator.RegisterIndicator(entity.GetComponent<IUIHolder>()?.GetHolder(), value);
    public void IndicatorUpdate() => Pay.UI.UIManager.Indicator.UpdateIndicator(indicatorObject, entity.CurrentHealth, entity.MaxHealth);
}

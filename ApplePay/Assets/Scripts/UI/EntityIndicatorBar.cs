using UnityEngine;

public abstract class EntityIndicatorBar : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image value;
    protected Entity entity;
    private Pay.UI.IndicatorObject indicatorObject;
    protected abstract float BarCurrentValue { get; }
    protected abstract float BarMaxValue { get; }
    public virtual void IndicatorSetup(Entity entity) 
    {
        indicatorObject = Pay.UI.UIManager.Indicator.RegisterIndicator(entity.GetComponent<IUIHolder>()?.GetHolder(), value);
        this.entity = entity;
    }
    public void IndicatorUpdate() => Pay.UI.UIManager.Indicator.UpdateIndicator(indicatorObject, BarCurrentValue, BarMaxValue);
}

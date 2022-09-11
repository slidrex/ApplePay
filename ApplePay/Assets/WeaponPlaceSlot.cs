using UnityEngine;
public class WeaponPlaceSlot : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image ImageRenderer;
    [Header("Slot indicator")]
    [SerializeField] private Pay.UI.Indicator indicator;
    private Pay.UI.IndicatorObject indicatorBuffer;
    private Pay.UI.TextObject textBuffer;
    private void Start() => SetItem(null);
    public void SetItem(Sprite item)
    {
        ImageRenderer.sprite = item;
        ImageRenderer.enabled = item != null;
    }
    public void CreateSlotIndicator(Pay.UI.UIHolder holder)
    {
        Pay.UI.UIManager.Indicator.CreateIndicator(holder, holder.HUDCanvas, indicator, out indicatorBuffer,
        Pay.UI.Options.Transform.StaticProperty.Position(ImageRenderer.transform.position - Vector3.up),
        Pay.UI.Options.Transform.StaticProperty.LocalScale(Vector3.one / 4)
        );
    }
    public void CreateSlotText(Pay.UI.UIHolder holder, string text, Pay.UI.TextConfiguration configuration, float duration, float fadeIn, float fadeOut)
    {
        Pay.UI.UIManager.Text.CreateText(holder, holder.HUDCanvas, text, configuration, duration, fadeIn, fadeOut, out textBuffer,
            Pay.UI.Options.Transform.StaticProperty.LocalScale(Vector3.one),
            Pay.UI.Options.Transform.StaticProperty.Position(ImageRenderer.transform.position + Vector3.up)
        );
    }
    public void SlotIndicatorUpdate(Pay.UI.UIHolder holder, float currentValue, float maxValue) => Pay.UI.UIManager.Indicator.UpdateIndicator(indicatorBuffer, currentValue, maxValue, Pay.UI.Options.IndicatorSettings.AutoRemove());
    public void RemoveSlotUI()
    {
        RemoveText();
        RemoveIndicator();
    }
    public void RemoveText() => Pay.UI.UIManager.RemoveUI(textBuffer);
    public void RemoveIndicator() => Pay.UI.UIManager.RemoveUI(indicatorBuffer);
    
}

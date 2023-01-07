using UnityEngine;
public class WeaponPlaceSlot : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image ImageRenderer;
    [Header("Slot indicator")]
    [SerializeField] private Pay.UI.Indicator indicator;
    public Pay.UI.IndicatorObject IndicatorBuffer {get; private set; }
    private Pay.UI.TextObject textBuffer;
    [Header("Display settings")]
    [SerializeField] private Pay.UI.TextConfiguration textConfiguration;
    [SerializeField] private float fadeIn;
    [SerializeField] private float duration;
    [SerializeField] private float fadeOut;
    private void Start() => SetItem(null);
    public void SetItem(Sprite item)
    {
        ImageRenderer.sprite = item;
        ImageRenderer.enabled = item != null;
    }
    public void CreateSlotIndicator(Pay.UI.UIHolder holder)
    {
        IndicatorBuffer = Pay.UI.UIManager.Indicator.CreateIndicator(holder, holder.HUDCanvas, indicator,
            Pay.UI.Options.Transform.StaticProperty.Position(ImageRenderer.transform.position - Vector3.up),
            Pay.UI.Options.Transform.StaticProperty.LocalScale(Vector3.one / 5)
        );
    }
    public void CreateSlotText(Pay.UI.UIHolder holder, string text)
    {
        textBuffer = Pay.UI.UIManager.Text.CreateText(holder, holder.HUDCanvas, text, textConfiguration, fadeIn, duration, fadeOut,
            Pay.UI.Options.Transform.StaticProperty.LocalScale(Vector3.one / 1.5f),
            Pay.UI.Options.Transform.StaticProperty.Position(ImageRenderer.transform.position + Vector3.up/1.5f)
        );
    }
    public void SlotIndicatorUpdate(Pay.UI.UIHolder holder, float currentValue, float maxValue) => Pay.UI.UIManager.Indicator.UpdateIndicator(IndicatorBuffer, currentValue, maxValue, Pay.UI.Options.IndicatorSettings.AutoRemove());
    public void RemoveSlotUI()
    {
        RemoveText();
        SetItem(null);
        RemoveIndicator();
    }
    public void RemoveText() => Pay.UI.UIManager.RemoveUI(textBuffer);
    public void RemoveIndicator() => Pay.UI.UIManager.RemoveUI(IndicatorBuffer);
}

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
    [SerializeField] private float textDuration;
    [SerializeField] private AnimationCurve fadeBehaviour;
    private void Start() => SetItem(null);
    public void SetItem(Sprite item)
    {
        ImageRenderer.sprite = item;
        ImageRenderer.enabled = item != null;
    }
    public void CreateSlotIndicator(Pay.UI.UIHolder holder)
    {
        Pay.UI.IndicatorObject buffer;
        Pay.UI.UIManager.Indicator.CreateIndicator(holder, holder.HUDCanvas, indicator, out buffer,
        Pay.UI.Options.Transform.StaticProperty.Position(ImageRenderer.transform.position - Vector3.up),
        Pay.UI.Options.Transform.StaticProperty.LocalScale(Vector3.one / 5)
        );
        IndicatorBuffer = buffer;
    }
    public void CreateSlotText(Pay.UI.UIHolder holder, string text)
    {
        Pay.UI.UIManager.Text.CreateText(holder, holder.HUDCanvas, text, textConfiguration, textDuration, fadeBehaviour, out textBuffer,
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

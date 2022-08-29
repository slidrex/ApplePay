using UnityEngine;
public class WeaponPlaceSlot : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image ImageRenderer;
    [Header("Slot indicator")]
    [SerializeField] private Pay.UI.Indicator indicator;
    private byte indicatorID, textID;
    private void Start() => SetItem(null);
    public void SetItem(Sprite item)
    {
        ImageRenderer.sprite = item;
        ImageRenderer.enabled = item != null ? true : false;
    }
    public void CreateSlotIndicator(Pay.UI.UIHolder holder)
    {
        Pay.UI.UIManager.Indicator.CreateIndicator(holder, holder.HUDCanvas, indicator, out indicatorID,
        Pay.UI.Options.Transform.StaticProperty.Position(ImageRenderer.transform.position - Vector3.up),
        Pay.UI.Options.Transform.StaticProperty.LocalScale(Vector3.one / 4)
        );
    }
    public void CreateSlotText(Pay.UI.UIHolder holder, string text, Pay.UI.TextConfiguration configuration, float duration, float fadeIn, float fadeOut)
    {
        Pay.UI.UIManager.Text.CreateText(holder, holder.HUDCanvas, text, configuration, duration, fadeIn, fadeOut, out textID,
            Pay.UI.Options.Transform.StaticProperty.LocalScale(Vector3.one),
            Pay.UI.Options.Transform.StaticProperty.Position(ImageRenderer.transform.position + Vector3.up)
        );
    }
    public void SlotIndicatorUpdate(Pay.UI.UIHolder holder, float currentValue, float maxValue) => Pay.UI.UIManager.Indicator.UpdateIndicator(holder, indicatorID, currentValue, maxValue, Pay.UI.Options.IndicatorSettings.AutoRemove());
    public void RemoveSlotUI(Pay.UI.UIHolder holder)
    {
        RemoveText(holder);
        RemoveIndicator(holder);
    }
    public void RemoveText(Pay.UI.UIHolder holder) => Pay.UI.UIManager.RemoveUI(holder, ref textID);
    public void RemoveIndicator(Pay.UI.UIHolder holder) => Pay.UI.UIManager.RemoveUI(holder, ref indicatorID);
    
}

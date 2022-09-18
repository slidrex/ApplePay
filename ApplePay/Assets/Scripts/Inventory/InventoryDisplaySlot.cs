using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryDisplaySlot : HoverableObject
{
    protected RepositoryRenderer attachedRenderer;
    [SerializeField] private UnityEngine.UI.Image Slot;
    [HideInInspector] public ItemDisplay Display;
    public void SetItem(Sprite display) => RenderItem(display);
    public void LinkDisplay(ItemDisplay display) => Display = display;
    public void RenderItem(Sprite sprite)
    {
        if(sprite == null)
        {
            Slot.enabled = false;
            return;
        }
        Slot.enabled = true;
        Slot.sprite = sprite;
    }
    public override void OnPointerEnter(PointerEventData pointerData)
    {
        base.OnPointerEnter(pointerData);
        attachedRenderer.OnCellTriggerEnter(Display, this);
    }
    public override void OnPointer()
    {
        base.OnPointer();
        attachedRenderer.OnCellTrigger(Display, this);
    }
    public override void OnPointerExit(PointerEventData pointerData)
    {
        base.OnPointerExit(pointerData);
        attachedRenderer.OnCellTriggerExit(Display, this);
    }
    public void LinkRender(RepositoryRenderer renderer) => attachedRenderer = renderer;
}

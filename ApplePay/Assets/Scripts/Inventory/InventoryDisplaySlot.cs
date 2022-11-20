using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryDisplaySlot<Display> : HoverableObject
{
    protected RepositoryRenderer<Display> attachedRenderer;
    [SerializeField] private UnityEngine.UI.Image Slot;
    [HideInInspector] public Display m_Display;
    public void LinkRender(RepositoryRenderer<Display> renderer) => attachedRenderer = renderer;
    public void LinkDisplay(Display display) => m_Display = display;
    public void RenderIcon(Sprite sprite)
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
        attachedRenderer.OnCellTriggerEnter(m_Display, this);
    }
    public override void OnPointer()
    {
        base.OnPointer();
        attachedRenderer.OnCellTrigger(m_Display, this);
    }
    public override void OnPointerExit(PointerEventData pointerData)
    {
        base.OnPointerExit(pointerData);
        attachedRenderer.OnCellTriggerExit(m_Display, this);
    }
}

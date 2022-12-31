using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryDisplaySlot<ItemType> : HoverableObject
{
    protected RepositoryRenderer<ItemType> attachedRenderer;
    [SerializeField] private UnityEngine.UI.Image rarityFrame;
    [SerializeField] private UnityEngine.UI.Image Slot;
    [HideInInspector] public ItemType Item;
    public void LinkRender(RepositoryRenderer<ItemType> renderer) => attachedRenderer = renderer;
    public void LinkItem(ItemType item) => Item = item;
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
    public void RenderRarityFrame(Color color, bool on = true)
    {
        rarityFrame.color = color;
        if(on) rarityFrame.gameObject.SetActive(true);
        else rarityFrame.gameObject.SetActive(false);
    }
    public override void OnPointerEnter(PointerEventData pointerData)
    {
        if(Item == null) return;
        base.OnPointerEnter(pointerData);
        attachedRenderer.OnCellTriggerEnter(Item, this);
    }
    public override void OnPointer()
    {
        base.OnPointer();
        attachedRenderer.OnCellTrigger(Item, this);
    }
    public override void OnPointerExit(PointerEventData pointerData)
    {
        if(Item == null) return;
        base.OnPointerExit(pointerData);
        attachedRenderer.OnCellTriggerExit(Item, this);
    }
}
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryDisplaySlot<ItemType> : HoverableObject, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    protected RepositoryRenderer<ItemType> attachedRenderer;
    public byte Index { get; set; }
    [SerializeField] private UnityEngine.UI.Image rarityFrame;
    public UnityEngine.UI.Image Slot;
    [HideInInspector] public ItemType Item;
    public void InitSlot(RepositoryRenderer<ItemType> renderer, byte index)
    {
        attachedRenderer = renderer;
        Index = index;
    }
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
    public void RenderSlotFrame(Color color, bool on = true)
    {
        rarityFrame.color = color;
        if(on) rarityFrame.gameObject.SetActive(true);
        else rarityFrame.gameObject.SetActive(false);
    }
    public override void OnPointerEnter(PointerEventData pointerData)
    {
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
        base.OnPointerExit(pointerData);
        attachedRenderer.OnCellTriggerExit(Item, this);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(Item != null)
            attachedRenderer.OnItemDragBegin(this, eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(Item != null)
            attachedRenderer.OnItemDragEnd(this, eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(Item != null)
            attachedRenderer.OnItemDrag(this, eventData);
    }

    public void OnDrop(PointerEventData eventData)
    {
        attachedRenderer.OnItemDrop(this, eventData);
    }
}
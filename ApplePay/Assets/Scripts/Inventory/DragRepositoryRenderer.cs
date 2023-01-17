using System;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class DragRepositoryRenderer<ItemType> : RepositoryRenderer<ItemType>
{
    protected DraggingImplementation draggingImplementation;
    protected override void OnSlotDragBegin(InventoryDisplaySlot<ItemType> slot, PointerEventData eventData)
    {
        draggingImplementation = new DraggingImplementation(slot.Index);
        draggingImplementation.isDragging = true;
        draggingImplementation.SetDraggingItem(slot.Slot.transform);
        draggingImplementation.GetDraggingTransform().SetParent(transform.parent.parent);
    }
    protected override void OnSlotDrag(InventoryDisplaySlot<ItemType> slot, PointerEventData eventData)
    {
        
        draggingImplementation.GetDraggingTransform().position = Pay.Functions.Generic.GetMousePos(Camera.main);
    }
    protected override void OnSlotDrop(InventoryDisplaySlot<ItemType> slot, PointerEventData eventData)
    {
        if(draggingImplementation.isDragging)
        {
            draggingImplementation.FoundDropSlot = true;
            draggingImplementation.newSlot = slot.Index;
            byte source = draggingImplementation.sourceSlot;
            byte current = slot.Index;
            ItemType tempItem = repository.Items[source];
            repository.Items[source] = repository.Items[current];
            repository.Items[current] = tempItem;
            RenderSlotCall.Invoke(slot.Index);
        }
    }
    protected override void OnSlotDragEnd(InventoryDisplaySlot<ItemType> slot, PointerEventData eventData)
    {
        draggingImplementation.isDragging = false;
        if(draggingImplementation.FoundDropSlot == false)
        {
            InventorySystem system = repository.AttachedSystem;
            ItemType charm = repository.Items[slot.Index];
            OnItemDrop(charm, slot);
        }
        draggingImplementation.GetDraggingTransform().SetParent(Slots[draggingImplementation.sourceSlot].transform);
        draggingImplementation.GetDraggingTransform().localPosition = Vector3.zero;
        RenderSlotCall.Invoke(slot.Index);
    }
    protected virtual void OnItemDrop(ItemType item, InventoryDisplaySlot<ItemType> slot) {}
}

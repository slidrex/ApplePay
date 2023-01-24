using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharmRepositoryRenderer : DragRepositoryRenderer<CollectableCharm>
{
    [SerializeField] private Hoverboard hoverboard;
    public override string RepositoryType => "charm";
    protected override Action<int> RenderSlotCall => RenderSlot;
    private void OnEnable()
    {
        hoverboard.SetDefaultDescription();
        Render();
    }
    protected override void OnRepositoryUpdated(int index) => RenderSlot(index);
    public override void OnCellTriggerEnter(CollectableCharm charm, InventoryDisplaySlot<CollectableCharm> slot)
    {
        if(charm != null)
        {
            CharmDisplay charmDisplay = charm.Charm.GetActiveCharm().Display;
            UpdateHoverboard(charmDisplay);
        }
        else hoverboard.SetDefaultDescription();
        if(draggingImplementation.isDragging)
        {
            slot.RenderSlotFrame(Color.white, true);
        }
    }
    public override void OnCellTriggerExit(CollectableCharm charm, InventoryDisplaySlot<CollectableCharm> slot)
    {
        hoverboard.SetDefaultDescription();
        if(draggingImplementation.isDragging)
        {
            RenderSlot(slot.Index);
        }
    }
    private void UpdateHoverboard(CharmDisplay display)
    {
        if(display.Equals(default(CharmDisplay)))
        {
            hoverboard.SetDefaultDescription();
        }
        else
        {
            ItemRarityInfo rarity = ItemRarityExtension.GetRarityInfo(display.Rarity);
            hoverboard.RemoveAddditionalFields();
            hoverboard.SetHeader(display.Description.Name, rarity.color);
            hoverboard.SetDescription(display.Description.Description);

            foreach(CharmDisplay.CharmAddtionalField addtionalField in display.AdditionalFields)
            {
                hoverboard.AddField(addtionalField.Text, addtionalField.GetColor(), Vector2.one/2);
            }
        }
    }
    public void OnCharmSwitched(InventoryCharmSlot slot)
    {
        for(int i = 0; i < Slots.Length; i++)
        {
            if(slot == Slots[i] && slot.Item.Charm.GetActiveCharm().Display.Equals(default(CharmDisplay)) == false)
            {
                CharmObject item = repository.Items[i].Charm;
                
                MixedCharm mixeedCharm = (MixedCharm)item;
                
                item.GetActiveCharm().EndFunction(InventorySystem.SystemOwner);
                
                mixeedCharm.ActiveIndex = (byte)Mathf.Repeat(mixeedCharm.ActiveIndex + 1, mixeedCharm.Charms.Length);
                
                
                item.GetActiveCharm().BeginFunction(InventorySystem.SystemOwner);
                CollectableCharm collectableCharm = repository.Items[i];
                collectableCharm.Charm = item;
                repository.Items[i] = collectableCharm;
                UpdateHoverboard(item.GetActiveCharm().Display);
                ItemRarityInfo rarityInfo = ItemRarityExtension.GetRarityInfo(collectableCharm.Charm.GetActiveCharm().Display.Rarity);
                RenderSlot(slot, collectableCharm.Charm.GetActiveCharm().Display.Icon, collectableCharm, rarityInfo.color, true, true);
                
                OnRepositoryUpdated((byte)i);
            }
        }
    }
    protected override void OnSlotDragEnd(InventoryDisplaySlot<CollectableCharm> slot, PointerEventData eventData)
    {
        base.OnSlotDragEnd(slot, eventData);
        if(draggingImplementation.newSlot == DraggingImplementation.NoTargetSlot)
            hoverboard.SetDefaultDescription();
        else
        {
            CharmDisplay display = repository.Items[draggingImplementation.newSlot].Charm.GetActiveCharm().Display;
            UpdateHoverboard(display);
        }
    }
    protected override void OnItemDrop(CollectableCharm item, InventoryDisplaySlot<CollectableCharm> slot)
    {
        InventorySystem system = repository.AttachedSystem;
        CollectableCharm charm = repository.Items[slot.Index];
        charm.Charm.GetActiveCharm().EndFunction(system.SystemOwner);
        charm.transform.SetParent(null);
        charm.transform.rotation = Quaternion.identity;
        Vector2 movementVector = (Pay.Functions.Generic.GetMousePos(Camera.main) - (Vector2)slot.transform.position).normalized;
        charm.transform.position = system.SystemOwner.transform.position + (Vector3)movementVector * system.dropItemOffset;
        charm.AddConstraintCollider(1.0f, system.SystemOwner.HitShape);
        charm.gameObject.SetActive(true);
        charm.ForceHandler.Knock(movementVector, system.dropItemBlockTime);
        repository.Items[slot.Index] = null;
    }
    private void RenderSlot(InventoryCharmSlot slot, Sprite icon, CollectableCharm item, Color frameColor, bool renderFrame, bool renderSwitchIcon)
    {
        slot.RenderSlotFrame(frameColor, renderFrame);
        slot.RenderIcon(icon);
        slot.RenderSwitchIcon(renderSwitchIcon);
        slot.LinkItem(item);
        if(item != null)
        {
            Charm charm = item.Charm.GetActiveCharm();
            for(int i = 0; i < charm.Display.AdditionalFields.Length; i++)
            {
                charm.Display.AdditionalFields[i].Color = charm.Display.AdditionalFields[i].Color;
                charm.Display.AdditionalFields[i].Text = CharmDisplay.FormatCharmField(charm.Display.AdditionalFields[i].Text, charm);
            }
            UpdateHoverboard(charm.Display);
        }
    }
    ///<summary>Renders slot using repository item data.</summary>
    private void RenderSlot(int index)
    {
        InventoryCharmSlot slot = Slots[index] as InventoryCharmSlot;
        if(repository.Items[index] == null) 
        {
            RenderSlot(slot, null, null, Color.white, false, false);
            return;
        }
        CollectableCharm charm = repository.Items[index];
        
        Sprite sprite = charm.Charm.GetActiveCharm().Display.Icon;
        Color colorFrame = ItemRarityExtension.GetRarityInfo(charm.Charm.GetActiveCharm().Display.Rarity).color;
        RenderSlot(slot, sprite, charm, colorFrame, true, repository.Items[index].Charm.charmType == CharmObject.CharmType.Switchable);
    }
}
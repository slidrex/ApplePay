using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharmRepositoryRenderer : RepositoryRenderer<CollectableCharm>
{
    [SerializeField] private Hoverboard hoverboard;
    public override string RepositoryType => "charm";
    private DraggingItem dragging;
    private void OnEnable()
    {
        hoverboard.SetDefaultDescription();
        Render();
    }
    protected override void OnRepositoryUpdated(byte index) { }
    public override void OnCellTriggerEnter(CollectableCharm charm, InventoryDisplaySlot<CollectableCharm> slot)
    {
        if(charm != null)
        {
            CharmDisplay charmDisplay = charm.charm.GetActiveCharm().Display;
            UpdateHoverboard(charmDisplay);
        }
        else hoverboard.SetDefaultDescription();
        if(dragging.isDragging)
        {
            slot.RenderSlotFrame(Color.white, true);
        }
    }
    public override void OnCellTriggerExit(CollectableCharm charm, InventoryDisplaySlot<CollectableCharm> slot)
    {
        hoverboard.SetDefaultDescription();
        if(dragging.isDragging)
        {
            RenderSlot(slot as InventoryCharmSlot);
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
            if(slot == Slots[i] && slot.Item.charm.GetActiveCharm().Display.Equals(default(CharmDisplay)) == false)
            {
                CharmObject item = repository.Items[i].charm;
                
                MixedCharm mixeedCharm = (MixedCharm)item;
                
                item.GetActiveCharm().EndFunction(InventorySystem.SystemOwner);
                
                mixeedCharm.ActiveIndex = (byte)Mathf.Repeat(mixeedCharm.ActiveIndex + 1, mixeedCharm.Charms.Length);
                
                
                item.GetActiveCharm().BeginFunction(InventorySystem.SystemOwner);
                CollectableCharm collectableCharm = repository.Items[i];
                collectableCharm.charm = item;
                repository.Items[i] = collectableCharm;
                UpdateHoverboard(item.GetActiveCharm().Display);
                ItemRarityInfo rarityInfo = ItemRarityExtension.GetRarityInfo(collectableCharm.charm.GetActiveCharm().Display.Rarity);
                RenderSlot(slot, collectableCharm.charm.GetActiveCharm().Display.Icon, collectableCharm, rarityInfo.color, true, true);
                
                OnRepositoryUpdated((byte)i);
            }
        }
    }
    public override void OnItemDragBegin(InventoryDisplaySlot<CollectableCharm> slot, PointerEventData eventData)
    {
        dragging = new DraggingItem(slot.Index, slot.Slot.transform);
        dragging.draggingItem.transform.SetParent(transform.parent.parent);
        dragging.isDragging = true;
    }
    public override void OnItemDrag(InventoryDisplaySlot<CollectableCharm> slot, PointerEventData eventData)
    {
        dragging.draggingItem.transform.position = Pay.Functions.Generic.GetMousePos(Camera.main);
    }
    public override void OnItemDrop(InventoryDisplaySlot<CollectableCharm> slot, PointerEventData eventData)
    {
        byte source = dragging.sourceSlot;
        byte current = slot.Index;
        CollectableCharm tempItem = repository.Items[source];
        repository.Items[source] = repository.Items[current];
        repository.Items[current] = tempItem;
        
        
        CharmDisplay dispay = repository.Items[current]?.charm?.GetActiveCharm() == null ? default(CharmDisplay) : repository.Items[current].charm.GetActiveCharm().Display;
        
        
        dragging.newSlot = slot.Index;
        dragging.isDragging = false;
        dragging.slotPlaced = true;
        RenderSlot(slot as InventoryCharmSlot);
    }
    public override void OnItemDragEnd(InventoryDisplaySlot<CollectableCharm> slot, PointerEventData eventData)
    {
        if(dragging.slotPlaced == false)
        {
            InventorySystem system = repository.AttachedSystem;
            CollectableCharm charm = repository.Items[slot.Index];
            charm.charm.GetActiveCharm().EndFunction(system.SystemOwner);
            charm.transform.SetParent(null);
            charm.transform.rotation = Quaternion.identity;
            Vector2 movementVector = (Pay.Functions.Generic.GetMousePos(Camera.main) - (Vector2)slot.transform.position).normalized;
            charm.transform.position = system.SystemOwner.transform.position + (Vector3)movementVector * system.dropItemOffset;
            charm.AddConstraintCollider(1.0f, system.SystemOwner.HitShape);
            charm.gameObject.SetActive(true);
            charm.ForceHandler.Knock(movementVector, system.dropItemBlockTime);
            repository.Items[slot.Index] = null;
        }
        dragging.draggingItem.SetParent(Slots[dragging.sourceSlot].transform);
        dragging.draggingItem.localPosition = Vector3.zero;
        dragging.isDragging = false;
        RenderSlot(Slots[dragging.sourceSlot] as InventoryCharmSlot);

        if(dragging.newSlot == DraggingItem.NoTargetSlot)
            hoverboard.SetDefaultDescription();
        else
        {
            CharmDisplay display = repository.Items[dragging.newSlot].charm.GetActiveCharm().Display;
            UpdateHoverboard(display);
        }
    }
    private void Render()
    {
        if(repository.Items.Length != Slots.Length) throw new Exception("Renderer slots list count and repository slots count are out of sync");
        
        for(int i = 0; i < Slots.Length; i++)
        {
            InventoryCharmSlot slot = Slots[i] as InventoryCharmSlot;
            RenderSlot(slot);
        }
    }
    private void RenderSlot(InventoryCharmSlot slot, Sprite icon, CollectableCharm item, Color frameColor, bool renderFrame, bool renderSwitchIcon)
    {
        slot.RenderSlotFrame(frameColor, renderFrame);
        slot.RenderIcon(icon);
        slot.RenderSwitchIcon(renderSwitchIcon);
        slot.LinkItem(item);
        if(item != null)
        {
            Charm charm = item.charm.GetActiveCharm();
            for(int i = 0; i < charm.Display.AdditionalFields.Length; i++)
            {
                charm.Display.AdditionalFields[i].Color = charm.Display.AdditionalFields[i].Color;
                charm.Display.AdditionalFields[i].Text = CharmDisplay.FormatCharmField(charm.Display.AdditionalFields[i].Text, charm);
            }
            UpdateHoverboard(charm.Display);
        }
    }
    ///<summary>Renders slot using repository item data.</summary>
    private void RenderSlot(InventoryCharmSlot slot)
    {
        byte index = slot.Index;
        if(repository.Items[index] == null) 
        {
            RenderSlot(slot, null, null, Color.white, false, false);
            return;
        }
        CollectableCharm charm = repository.Items[index];
        
        Sprite sprite = charm.charm.GetActiveCharm().Display.Icon;
        Color colorFrame = ItemRarityExtension.GetRarityInfo(charm.charm.GetActiveCharm().Display.Rarity).color;
        RenderSlot(slot, sprite, charm, colorFrame, true, repository.Items[index].charm.charmType == CharmObject.CharmType.Switchable);
    }
    internal struct DraggingItem
    {
        internal const int NoTargetSlot = -1;
        internal bool isDragging;
        internal byte sourceSlot;
        internal int newSlot;
        internal bool slotPlaced;
        internal Transform draggingItem;
        internal DraggingItem(byte source, Transform dragging)
        {
            newSlot = NoTargetSlot;
            isDragging = false;
            slotPlaced = false;
            sourceSlot = source;
            draggingItem = dragging;
        }
    }
}
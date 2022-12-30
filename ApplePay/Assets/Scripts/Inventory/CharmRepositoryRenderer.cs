using System;
using UnityEngine;

public class CharmRepositoryRenderer : RepositoryRenderer<CollectableCharm>
{
    [SerializeField] private Hoverboard hoverboard;
    public override string RepositoryType => "charm";
    private void OnEnable()
    {
        hoverboard.SetDefaultDescription();
        Render();
    }
    protected override void OnRepositoryUpdated(byte index) => Render();
    public override void OnCellTriggerEnter(CollectableCharm charm, InventoryDisplaySlot<CollectableCharm> slot)
    {
        CharmDisplay charmDisplay = charm.charm.Display;
        UpdateHoverboard(charmDisplay);
    }
    private void UpdateHoverboard(CharmDisplay display)
    {
        if(display.Equals(new CharmDisplay()))
        {
            hoverboard.SetDefaultDescription();
            return;
        }
        ItemRarityInfo rarity = ItemRarityExtension.GetRarityInfo(display.Rarity);
        hoverboard.RemoveAddditionalFields();
        hoverboard.SetHeader(display.Description.Name, rarity.color);
        hoverboard.SetDescription(display.Description.Description);

        foreach(CharmDisplay.CharmAddtionalField addtionalField in display.AdditionalFields)
        {
            hoverboard.AddField(addtionalField.Text, addtionalField.GetColor());
        }

    }
    public void OnCharmSwitched(InventoryDisplaySlot<CollectableCharm> slot)
    {
        for(int i = 0; i < Slots.Length; i++)
        {
            if(slot == Slots[i] && slot.Item.charm.Display.Equals(new CharmDisplay()) == false)
            {
                CharmObject item = repository.Items[i].charm;
                
                MixedCharm mixeedCharm = (MixedCharm)item;
                
                item.GetActiveCharm().EndFunction(InventorySystem.SystemOwner);
                
                mixeedCharm.ActiveIndex = (byte)Mathf.Repeat(mixeedCharm.ActiveIndex + 1, mixeedCharm.Charms.Length);
                
                
                item.GetActiveCharm().BeginFunction(InventorySystem.SystemOwner);
                CollectableCharm collectableCharm = repository.Items[i];
                collectableCharm.charm = (Charm)item;
                repository.Items[i] = collectableCharm;
                UpdateHoverboard(item.GetActiveCharm().Display);
                OnRepositoryUpdated((byte)i); //Performance issue.
            }
        }
    }
    public override void OnCellTriggerExit(CollectableCharm charm, InventoryDisplaySlot<CollectableCharm> slot) => hoverboard.SetDefaultDescription();
    private void Render()
    {
        if(repository.Items.Length != Slots.Length) throw new Exception("Renderer slots list count and repository slots count are out of sync");
        CharmInventoryRenderItem[] renderItems = new CharmInventoryRenderItem[repository.Items.Length];
        for(int i = 0; i < repository.Items.Length; i++)
        {
            if(repository.Items[i] != null) 
            {
            
                CollectableCharm item = repository.Items[i];
                CharmInventoryRenderItem _item = new CharmInventoryRenderItem();
                _item.Type = item.charm.charmType;
                _item.Item = item;
                Charm currentCharm = _item.Item.charm.GetActiveCharm();
                for(int j = 0; j < currentCharm.Display.AdditionalFields.Length; j++)
                {
                    currentCharm.Display.AdditionalFields[j].Color = currentCharm.Display.AdditionalFields[j].Color;
                    currentCharm.Display.AdditionalFields[j].Text = CharmDisplay.FormatCharmField(currentCharm.Display.AdditionalFields[j].Text, item.charm.GetActiveCharm());
                }
                renderItems[i] = _item;
            }
            
        }
        for(int i = 0; i < Slots.Length; i++)
        {
            if(renderItems[i].Item != null)
            {
                InventoryDisplaySlot<CollectableCharm> slot = (InventoryDisplaySlot<CollectableCharm>)Slots[i];
                bool switchable = renderItems[i].Type == CharmObject.CharmType.Base ? false : true;
                ItemRarityInfo info = ItemRarityExtension.GetRarityInfo(renderItems[i].Item.charm.Display.Rarity);
                slot.SetRarityFrameColor(info.color);
                slot.RenderIcon(renderItems[i].Item.charm.GetActiveCharm().Display.Icon);
                slot.LinkItem(renderItems[i].Item);
            }
        }
    }
    private struct CharmInventoryRenderItem
    {
        internal CollectableCharm Item;
        internal CharmObject.CharmType Type;
    }

    public static explicit operator CharmRepositoryRenderer(RepositoryRenderer<Charm> v)
    {
        throw new NotImplementedException();
    }
}
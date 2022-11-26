using UnityEngine;

public class CharmRepositoryRenderer : RepositoryRenderer<Charm>
{
    [SerializeField] private Hoverboard hoverboard;

    public override string RepositoryType => "charm";
    private void OnEnable() => Render();
    protected override void OnRepositoryUpdated(byte index) => Render();
    public override void OnCellTriggerEnter(Charm charm, InventoryDisplaySlot<Charm> slot)
    {
        CharmDisplay charmDisplay = charm.Display;
        UpdateHoverboard(charmDisplay);
    }
    private void UpdateHoverboard(CharmDisplay display)
    {
        if(display.Equals(new CharmDisplay()))
        {
            hoverboard.SetDefaultDescription();
            return;
        }
        hoverboard.RemoveAddditionalFields();
        hoverboard.SetDescription(display.Description.Name, display.Description.Description);

        foreach(CharmDisplay.CharmAddtionalField addtionalField in display.AdditionalFields)
        {
            hoverboard.AddField(addtionalField.Text, addtionalField.GetColor());
        }

    }
    public void OnCharmSwitched(InventoryDisplaySlot<Charm> slot)
    {
        for(int i = 0; i < Slots.Length; i++)
        {
            if(slot == Slots[i] && slot.Item.Display.Equals(new CharmDisplay()) == false)
            {
                CharmObject item = repository.Items[i];
                
                MixedCharm mixeedCharm = (MixedCharm)item;
                
                item.GetActiveCharm().EndFunction(InventorySystem.SystemOwner);
                
                mixeedCharm.ActiveIndex = (byte)Mathf.Repeat(mixeedCharm.ActiveIndex + 1, mixeedCharm.Charms.Length);
                
                
                item.GetActiveCharm().BeginFunction(InventorySystem.SystemOwner);
                repository.Items[i] = item.GetActiveCharm();
                UpdateHoverboard(item.GetActiveCharm().Display);
                OnRepositoryUpdated((byte)i); //Performance issue.
            }
        }
    }
    public override void OnCellTriggerExit(Charm charm, InventoryDisplaySlot<Charm> slot) => hoverboard.SetDefaultDescription();
    private void Render()
    {
        CharmInventoryRenderItem[] renderItems = new CharmInventoryRenderItem[repository.Items.Length];
        for(int i = 0; i < repository.Items.Length; i++)
        {
            CharmObject item = repository.Items[i];
            CharmInventoryRenderItem _item = new CharmInventoryRenderItem();
            if(item != null) 
            {
                _item.Type = item.charmType;
                _item.Item = item;
                Charm currentCharm = _item.Item.GetActiveCharm();
                for(int j = 0; j < currentCharm.Display.AdditionalFields.Length; j++)
                {
                    currentCharm.Display.AdditionalFields[j].Color = currentCharm.Display.AdditionalFields[j].Color;
                    currentCharm.Display.AdditionalFields[j].Text = CharmDisplay.FormatCharmField(currentCharm.Display.AdditionalFields[j].Text, item.GetActiveCharm());
                }
                renderItems[i] = _item;
            }
            
        }
        for(int i = 0; i < Slots.Length; i++)
        {
            if(renderItems[i].Item != null)
            {
                InventoryCharmSlot slot = (InventoryCharmSlot)Slots[i];
                bool switchable = renderItems[i].Type == CharmObject.CharmType.Base ? false : true;
                
                slot.RenderItem(renderItems[i].Item.GetActiveCharm(), switchable);

            }
        }
    }
    private struct CharmInventoryRenderItem
    {
        internal CharmObject Item;
        internal CharmObject.CharmType Type;
    }
    
}
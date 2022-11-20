using UnityEngine;

public class CharmRepositoryRenderer : RepositoryRenderer<CharmDisplay>
{
    [SerializeField] private Hoverboard hoverboard;
    private void OnEnable() => Render();
    protected override void OnRepositoryUpdated(byte index) => Render();
    public override void OnCellTriggerEnter(CharmDisplay display, InventoryDisplaySlot<CharmDisplay> slot)
    {
        CharmDisplay charmDisplay = (CharmDisplay)display;
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
    public void OnCharmSwitched(InventoryDisplaySlot<CharmDisplay> slot)
    {
        for(int i = 0; i < Slots.Length; i++)
        {
            if(slot == Slots[i] && slot.m_Display.Equals(new CharmDisplay()) == false)
            {
                CharmItem item = (CharmItem)Repository.Items[i];
                
                MixedCharm mixedCharm = (MixedCharm)item.Item;
                
                
                item.GetActiveCharm().EndFunction(Inventory.InventoryOwner, item.Manual);
                
                item.ActiveIndex = (byte)Mathf.Repeat(item.ActiveIndex + 1, mixedCharm.Charms.Length);
                
                
                item.GetActiveCharm().BeginFunction(Inventory.InventoryOwner, item.Manual);
                Repository.Items[i] = item;
                UpdateHoverboard(item.GetActiveCharm().Display);
                OnRepositoryUpdated((byte)i); //Performance issue.
            }
        }
    }
    public override void OnCellTriggerExit(CharmDisplay display, InventoryDisplaySlot<CharmDisplay> slot) => hoverboard.SetDefaultDescription();
    private void Render()
    {
        InventoryRepository repository = Inventory.GetRepository(RepositoryName);
        CharmInventoryRenderItem[] renderItems = new CharmInventoryRenderItem[repository.Items.Length];
        for(int i = 0; i < repository.Items.Length; i++)
        {
            CharmItem item = (CharmItem)repository.Items[i];
            CharmInventoryRenderItem _item = new CharmInventoryRenderItem();
            if(item != null) 
            {
                _item.Type = item.Type;
                CharmDisplay currentCharmDisplay = item.GetActiveCharm().Display; 
                
                _item.Display = new CharmDisplay();
                _item.Display.Description = new ItemDescription();
                _item.Display.Description = item.GetActiveCharm().Display.Description;
                _item.Display.Icon = item.GetActiveCharm().Display.Icon;
                _item.Display.AdditionalFields = new CharmDisplay.CharmAddtionalField[currentCharmDisplay.AdditionalFields.Length];
                
                
                for(int j = 0; j < _item.Display.AdditionalFields.Length; j++)
                {
                    _item.Display.AdditionalFields[j].Color = currentCharmDisplay.AdditionalFields[j].Color;
                    _item.Display.AdditionalFields[j].Text = CharmDisplay.FormatCharmField(currentCharmDisplay.AdditionalFields[j].Text, item.GetActiveCharm(), item.Manual);
                }
            }
            renderItems[i] = _item;
            
        }
        for(int i = 0; i < Slots.Length; i++)
        {
            InventoryCharmSlot slot = (InventoryCharmSlot)Slots[i];
            bool switchable = renderItems[i].Type == CharmType.Base ? false : true;
            slot.RenderItem(renderItems[i].Display, switchable);
        }
    }
    private struct CharmInventoryRenderItem
    {
        internal CharmDisplay Display;
        internal CharmType Type;
    }
    
}
public enum CharmType
{
    Base,
    Switchable
}

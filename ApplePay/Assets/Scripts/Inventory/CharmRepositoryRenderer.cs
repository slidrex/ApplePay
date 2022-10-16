using UnityEngine;

public class CharmRepositoryRenderer : RepositoryRenderer
{
    [SerializeField] private Hoverboard hoverboard;
    private void OnEnable() => Render();
    protected override void OnRepositoryUpdated(byte index) => Render();
    public override void OnCellTriggerEnter(ItemDisplay display, InventoryDisplaySlot slot)
    {
        CharmDisplay charmDisplay = (CharmDisplay)display;
        UpdateHoverboard(charmDisplay);
    }
    private void UpdateHoverboard(CharmDisplay display)
    {
        if(display == null)
        {
            hoverboard.SetDefaultDescription();
            return;
        }
        hoverboard.RemoveAddditionalFields();
        hoverboard.SetDescription(display.Description.Name, display.Description.Description);

        foreach(CharmDisplay.CharmAddtionalField addtionalField in display.AdditionalFields)
        {
            hoverboard.AddField(addtionalField.Text, addtionalField.Color);
        }

    }
    public void OnCharmSwitched(InventoryDisplaySlot slot)
    {
        for(int i = 0; i < Slots.Length; i++)
        {
            if(slot == Slots[i] && slot.Display != null)
            {
                CharmItem item = (CharmItem)Repository.Items[i];
                
                MixedCharm mixedCharm = (MixedCharm)item.Item;
                
                
                item.GetActiveCharm().EndFunction(Inventory.InventoryOwner);
                
                item.ActiveIndex = (byte)Mathf.Repeat(item.ActiveIndex + 1, mixedCharm.Charms.Length);
                
                
                item.GetActiveCharm().BeginFunction(Inventory.InventoryOwner);
                Repository.Items[i] = item;
                UpdateHoverboard(item.GetActiveCharm().Display);
                OnRepositoryUpdated((byte)i); //Performance issue.
            }
        }
    }
    public override void OnCellTriggerExit(ItemDisplay display, InventoryDisplaySlot slot) => hoverboard.SetDefaultDescription();
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
                _item.Display = item.GetActiveCharm().Display;
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

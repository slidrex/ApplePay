using UnityEngine;

public class CharmRepositoryRenderer : RepositoryRenderer
{
    [SerializeField] private CharmRepository repository;
    [SerializeField] private Hoverboard hoverboard;
    public override void OnRepositoryUpdate(InventoryRepository repository) => Render();
    private void OnEnable() => Render();
    private void Render()
    {
        CharmDisplay[] itemDisplays = new CharmDisplay[repository.InventoryItems.Count];
        for(int i = 0; i < itemDisplays.Length; i++)
        {
            itemDisplays[i] = repository.InventoryItems[i].Item.Display;
        }
        SetupItems(itemDisplays);
    }
    public override void OnCellTriggerEnter(ItemDisplay display, InventoryDisplaySlot slot)
    {
        hoverboard.RemoveAddditionalFields();
        if(display.InventorySprite == null)
        {
            hoverboard.SetDefaultDescription();
            return;
        }
        CharmDisplay _display = (CharmDisplay)display;
        if(_display == null || _display.Description.Name == "")
        {
            hoverboard.SetDefaultDescription();
            return;
        }
        hoverboard.SetDescription(_display.Description.Name, _display.Description.Description);
        
        foreach(CharmDisplay.CharmAddtionalField stat in _display.AdditionalFields)
        {
            hoverboard.AddField(stat.Text, stat.Color);
        }
    }
    public override void OnCellTriggerExit(ItemDisplay display, InventoryDisplaySlot slot)
    {
        hoverboard.SetDefaultDescription();
        hoverboard.RemoveAddditionalFields();
    }
}

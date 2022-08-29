using UnityEngine;
public class CharmRepositoryRenderer : RepositoryRenderer
{
    [SerializeField] private CharmRepository repository;
    [SerializeField] private Hoverboard hoverboard;
    public override void OnRepositoryUpdate(InventoryRepository repository) => Render();
    private void OnEnable() => Render();
    private void Render()
    {
        ItemDisplay[] itemDisplays = new ItemDisplay[repository.InventoryItems.Count];
        for(int i = 0; i < itemDisplays.Length; i++)
        {
            itemDisplays[i] = repository.InventoryItems[i].Item.Display;
        }
        RenderItems(itemDisplays);
    }
    public override void OnCellTriggerEnter(ItemDisplay display, InventoryDisplaySlot slot)
    {
        if(display == null || display.Description.Name == "")
        {
            hoverboard.SetDefaultDescription();
            return;
        }
        hoverboard.SetDescription(display.Description.Name, display.Description.Description);
    }
    public override void OnCellTriggerExit(ItemDisplay display, InventoryDisplaySlot slot) => hoverboard.SetDefaultDescription();
}

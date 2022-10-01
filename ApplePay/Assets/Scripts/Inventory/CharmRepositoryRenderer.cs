using UnityEngine;
using System.Linq;

public class CharmRepositoryRenderer : RepositoryRenderer
{
    [SerializeField] private Hoverboard hoverboard;
    private void OnEnable() => Render();
    public override void OnRepositoryUpdate() => Render();
    public override void OnCellTriggerEnter(ItemDisplay display, InventoryDisplaySlot slot)
    {
        if(display == null)
        {
            hoverboard.SetDefaultDescription();
            return;
        }
        CharmDisplay charmDisplay = (CharmDisplay)display;
        hoverboard.RemoveAddditionalFields();
        hoverboard.SetDescription(charmDisplay.Description.Name, charmDisplay.Description.Description);

        foreach(CharmDisplay.CharmAddtionalField addtionalField in charmDisplay.AdditionalFields)
        {
            hoverboard.AddField(addtionalField.Text, addtionalField.Color);
        }
    }
    public override void OnCellTriggerExit(ItemDisplay display, InventoryDisplaySlot slot) => hoverboard.SetDefaultDescription();
    private void Render()
    {
        InventoryRepository repository = Inventory.GetRepository(RepositoryName);
        ItemDisplay[] displays = new ItemDisplay[repository.Items.Length];
        for(int i = 0; i < repository.Items.Length; i++)
        {
            CharmItem item = (CharmItem)repository.Items[i];
            displays[i] = item?.Item?.Display;
        }
        SetupItems(displays);
    }
}

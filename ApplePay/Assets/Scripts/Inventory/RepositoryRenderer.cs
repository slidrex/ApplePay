using UnityEngine;
[System.Serializable]
public abstract class RepositoryRenderer : MonoBehaviour
{
    public InventorySystem Inventory;
    [SerializeField] protected string RepositoryName;
    private InventoryRepository repository;
    private void Start()
    {
        repository = Inventory.GetRepository(RepositoryName);
        SetSlotsRenderer();
    }
    public InventoryDisplaySlot[] Slots
    {
        get
        {
            System.Collections.Generic.List<InventoryDisplaySlot> slotList = new System.Collections.Generic.List<InventoryDisplaySlot>();
            for(byte i = 0; i < transform.childCount; i++)
            {
                if(transform.GetChild(i).GetComponent<InventoryDisplaySlot>() != null) slotList.Add(transform.GetChild(i).GetComponent<InventoryDisplaySlot>());
            }
            return slotList.ToArray();
        }
    }
    public virtual void OnRepositoryUpdate() { }
    public void SetupItems(ItemDisplay[] displayItems)
    {
        for(int i = 0; i < displayItems.Length; i++)
        {
            Slots[i].LinkDisplay(displayItems[i]);
            Slots[i].SetItem(displayItems[i]?.InventorySprite);
        }
    }
    private void SetSlotsRenderer()
    {
        foreach(InventoryDisplaySlot slot in Slots) slot.LinkRender(this);
    }
    public virtual void OnCellTriggerEnter(ItemDisplay display, InventoryDisplaySlot slot) {}
    public virtual void OnCellTrigger(ItemDisplay display, InventoryDisplaySlot slot) {}
    public virtual void OnCellTriggerExit(ItemDisplay display, InventoryDisplaySlot slot) {}
}

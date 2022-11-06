using UnityEngine;
[System.Serializable]

public abstract class RepositoryRenderer : MonoBehaviour, IRepositoryCallbackHandler
{
    public InventorySystem Inventory;
    [SerializeField] protected string RepositoryName;
    protected InventoryRepository Repository;
    private void Start()
    {
        Repository = Inventory.GetRepository(RepositoryName);
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
    public void OnRepositoryUpdated(Item item, byte index, RepositoryChangeFeedback feedback) => OnRepositoryUpdated(index);
    protected virtual void OnRepositoryUpdated(byte index) {}
    private void SetSlotsRenderer()
    {
        foreach(InventoryDisplaySlot slot in Slots) slot.LinkRender(this);
    }
    public virtual void OnCellTriggerEnter(ItemDisplay display, InventoryDisplaySlot slot) {}
    public virtual void OnCellTrigger(ItemDisplay display, InventoryDisplaySlot slot) {}
    public virtual void OnCellTriggerExit(ItemDisplay display, InventoryDisplaySlot slot) {}
}

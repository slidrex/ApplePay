using UnityEngine;
[System.Serializable]

public abstract class RepositoryRenderer<Display> : MonoBehaviour, IRepositoryCallbackHandler
{
    public InventorySystem Inventory;
    [SerializeField] protected string RepositoryName;
    protected InventoryRepository Repository;
    private void Start()
    {
        Repository = Inventory.GetRepository(RepositoryName);
        SetSlotsRenderer();
    }
    public InventoryDisplaySlot<Display>[] Slots
    {
        get
        {
            System.Collections.Generic.List<InventoryDisplaySlot<Display>> slotList = new System.Collections.Generic.List<InventoryDisplaySlot<Display>>();
            for(byte i = 0; i < transform.childCount; i++)
            {
                if(transform.GetChild(i).GetComponent<InventoryDisplaySlot<Display>>() != null) slotList.Add(transform.GetChild(i).GetComponent<InventoryDisplaySlot<Display>>());
            }
            return slotList.ToArray();
        }
    }
    public void OnRepositoryUpdated(Item item, byte index, RepositoryChangeFeedback feedback) => OnRepositoryUpdated(index);
    protected virtual void OnRepositoryUpdated(byte index) {}
    private void SetSlotsRenderer()
    {
        foreach(InventoryDisplaySlot<Display> slot in Slots) slot.LinkRender(this);
    }
    public virtual void OnCellTriggerEnter(Display display, InventoryDisplaySlot<Display> slot) {}
    public virtual void OnCellTrigger(Display display, InventoryDisplaySlot<Display> slot) {}
    public virtual void OnCellTriggerExit(Display display, InventoryDisplaySlot<Display> slot) {}
}

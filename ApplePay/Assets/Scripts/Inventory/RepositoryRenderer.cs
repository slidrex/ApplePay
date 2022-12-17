using UnityEngine;

[System.Serializable]
public abstract class RepositoryRenderer<ItemType> : MonoBehaviour
{
    public InventorySystem InventorySystem;
    public InventoryRepository<ItemType> repository {get; set;}
    public abstract string RepositoryType {get;}
    private void Awake()
    {
        repository = (InventoryRepository<ItemType>)InventorySystem.GetRepository(RepositoryType);
    }
    private void Start()
    {
        SetSlotsRenderer();
    }
    public InventoryDisplaySlot<ItemType>[] Slots
    {
        get
        {
            System.Collections.Generic.List<InventoryDisplaySlot<ItemType>> slotList = new System.Collections.Generic.List<InventoryDisplaySlot<ItemType>>();
            for(int i = 0; i < transform.childCount; i++)
            {
                if(transform.GetChild(i).GetComponent<InventoryDisplaySlot<ItemType>>() != null) slotList.Add(transform.GetChild(i).GetComponent<InventoryDisplaySlot<ItemType>>());
            }
            return slotList.ToArray();
        }
    }
    protected virtual void OnRepositoryUpdated(byte index) {}
    private void SetSlotsRenderer()
    {
        foreach(InventoryDisplaySlot<ItemType> slot in Slots) slot.LinkRender(this);
    }
    public virtual void OnCellTriggerEnter(ItemType display, InventoryDisplaySlot<ItemType> slot) {}
    public virtual void OnCellTrigger(ItemType display, InventoryDisplaySlot<ItemType> slot) {}
    public virtual void OnCellTriggerExit(ItemType display, InventoryDisplaySlot<ItemType> slot) {}
}
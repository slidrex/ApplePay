using UnityEngine;

[System.Serializable]
public abstract class RepositoryRenderer<ItemType> : RepositoryRendererBase
{
    public InventorySystem InventorySystem;
    [SerializeField] private Transform slotsContainer;
    public InventoryRepository<ItemType> repository {get; set;}
    public abstract string RepositoryType {get;}
    protected virtual void Awake()
    {
        repository = (InventoryRepository<ItemType>)InventorySystem.GetRepository(RepositoryType);
    }
    protected virtual void Start()
    {
        SetSlotsRenderer();
    }
    public InventoryDisplaySlot<ItemType>[] Slots
    {
        get
        {
            System.Collections.Generic.List<InventoryDisplaySlot<ItemType>> slotList = new System.Collections.Generic.List<InventoryDisplaySlot<ItemType>>();
            for(int i = 0; i < slotsContainer.childCount; i++)
            {
                if(slotsContainer.GetChild(i).GetComponent<InventoryDisplaySlot<ItemType>>() != null) slotList.Add(slotsContainer.GetChild(i).GetComponent<InventoryDisplaySlot<ItemType>>());
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
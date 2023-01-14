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
        SetupSlots();
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
    public virtual void OnItemDragBegin(InventoryDisplaySlot<ItemType> slot, UnityEngine.EventSystems.PointerEventData eventData)
    {
        
    }
    public virtual void OnItemDragEnd(InventoryDisplaySlot<ItemType> slot, UnityEngine.EventSystems.PointerEventData eventData)
    {
        
    }
    public virtual void OnItemDrag(InventoryDisplaySlot<ItemType> slot, UnityEngine.EventSystems.PointerEventData eventData)
    {
        
    }
    public virtual void OnItemDrop(InventoryDisplaySlot<ItemType> slot, UnityEngine.EventSystems.PointerEventData eventData)
    {
        
    }
    protected virtual void OnRepositoryUpdated(byte index) {}
    private void SetupSlots()
    {
        for(int i = 0; i < Slots.Length; i++)
        {
            Slots[i].InitSlot(this, (byte)i);
        }
    }
    public virtual void OnCellTriggerEnter(ItemType display, InventoryDisplaySlot<ItemType> slot) {}
    public virtual void OnCellTrigger(ItemType display, InventoryDisplaySlot<ItemType> slot) {}
    public virtual void OnCellTriggerExit(ItemType display, InventoryDisplaySlot<ItemType> slot) {}
}
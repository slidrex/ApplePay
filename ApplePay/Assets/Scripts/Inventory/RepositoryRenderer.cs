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
        InitSlotArray();
        SetupSlots();
    }
    private void InitSlotArray()
    {
        int slotsCount = slotsContainer.childCount;
        Slots = new InventoryDisplaySlot<ItemType>[slotsCount];
        int index = 0;

        while(index < slotsContainer.childCount)
        {
            InventoryDisplaySlot<ItemType> slot = slotsContainer.GetChild(index).GetComponent<InventoryDisplaySlot<ItemType>>();
            if(slot != null)
            {
                Slots[index] = slot;
                index++;
            }
            else slotsCount--;
        }
    }
    private void SetupSlots()
    {
        for(int i = 0; i < Slots.Length; i++)
        {
            Slots[i].InitSlot(this, (byte)i);
        }
    }
    public InventoryDisplaySlot<ItemType>[] Slots { get; private set; }
    protected virtual void Start() {}
    public virtual void OnItemDragBegin(InventoryDisplaySlot<ItemType> slot, UnityEngine.EventSystems.PointerEventData eventData) {}
    public virtual void OnItemDragEnd(InventoryDisplaySlot<ItemType> slot, UnityEngine.EventSystems.PointerEventData eventData) {}
    public virtual void OnItemDrag(InventoryDisplaySlot<ItemType> slot, UnityEngine.EventSystems.PointerEventData eventData) {}
    public virtual void OnItemDrop(InventoryDisplaySlot<ItemType> slot, UnityEngine.EventSystems.PointerEventData eventData) {}
    protected virtual void OnRepositoryUpdated(byte index) {}
    public virtual void OnCellTriggerEnter(ItemType display, InventoryDisplaySlot<ItemType> slot) {}
    public virtual void OnCellTrigger(ItemType display, InventoryDisplaySlot<ItemType> slot) {}
    public virtual void OnCellTriggerExit(ItemType display, InventoryDisplaySlot<ItemType> slot) {}
}
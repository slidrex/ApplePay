using UnityEngine;

[System.Serializable]
public abstract class RepositoryRenderer<ItemType> : RepositoryRendererBase
{
    protected abstract System.Action<int> RenderSlotCall {get;}
    public InventorySystem InventorySystem;
    [SerializeField] private Transform slotsContainer;
    public InventoryRepository<ItemType> repository {get; set;}
    public abstract string RepositoryType {get;}
    protected virtual void Awake()
    {
        repository = (InventoryRepository<ItemType>)InventorySystem.GetRepository(RepositoryType);
        repository.RepositoryChangeCallback += OnRepositoryUpdated;
        
        InitSlotArray();
        SetupSlots();
    }
    protected void Render()
    {
        if(repository.Items.Length != Slots.Length) throw new System.Exception("Renderer slots list count and repository slots count are out of sync");
        for(int i = 0; i < repository.Items.Length; i++)
        {
            RenderSlotCall.Invoke(i);
        }
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
    public void ItemDragBegin(InventoryDisplaySlot<ItemType> slot, UnityEngine.EventSystems.PointerEventData eventData)
    {
        OnSlotDragBegin(slot, eventData);
    }
    public void ItemDrag(InventoryDisplaySlot<ItemType> slot, UnityEngine.EventSystems.PointerEventData eventData)
    {
        OnSlotDrag(slot, eventData);
    }
    public void ItemDragEnd(InventoryDisplaySlot<ItemType> slot, UnityEngine.EventSystems.PointerEventData eventData)
    {
        OnSlotDragEnd(slot, eventData);
    }
    public void ItemDrop(InventoryDisplaySlot<ItemType> slot, UnityEngine.EventSystems.PointerEventData eventData)
    {
        OnSlotDrop(slot, eventData);
    }
    protected virtual void OnSlotDragBegin(InventoryDisplaySlot<ItemType> slot, UnityEngine.EventSystems.PointerEventData eventData) {}
    protected virtual void OnSlotDragEnd(InventoryDisplaySlot<ItemType> slot, UnityEngine.EventSystems.PointerEventData eventData) {}
    protected virtual void OnSlotDrag(InventoryDisplaySlot<ItemType> slot, UnityEngine.EventSystems.PointerEventData eventData) {}
    protected virtual void OnSlotDrop(InventoryDisplaySlot<ItemType> slot, UnityEngine.EventSystems.PointerEventData eventData) {}
    protected virtual void OnRepositoryUpdated(int index) {}
    public virtual void OnCellTriggerEnter(ItemType display, InventoryDisplaySlot<ItemType> slot) {}
    public virtual void OnCellTrigger(ItemType display, InventoryDisplaySlot<ItemType> slot) {}
    public virtual void OnCellTriggerExit(ItemType display, InventoryDisplaySlot<ItemType> slot) {}
    protected struct DraggingImplementation
    {
        internal const int NoTargetSlot = -1;
        public bool isDragging { get; internal set; }
        internal byte sourceSlot;
        internal int newSlot;
        internal bool FoundDropSlot;
        private Transform draggingItem;
        public void SetDraggingItem(Transform dragging)
        {
            draggingItem = dragging;
        }
        public Transform GetDraggingTransform() => draggingItem;
        internal DraggingImplementation(byte source)
        {
            newSlot = NoTargetSlot;
            isDragging = false;
            FoundDropSlot = false;
            sourceSlot = source;
            draggingItem = null;
        }
    }
}
using UnityEngine;
using System.Linq;
public abstract class AdvancedWeaponHolder : WeaponHolder, IRepositoryCallbackHandler, IRepositoryPreUpdateHandler
{
    [Header("Weapon display")]
    [SerializeField, Tooltip("The object which stores weapon drop object in inventory.")] private Transform weaponList;
    public InventorySystem InventorySystem;
    [SerializeField] private string repositoryName;
    protected abstract Vector2 DropDirection { get; }
    protected InventoryRepository Repository; 
    [Header("Weapon Switch")]
    [SerializeField, ReadOnly] private byte activeWeaponIndex;
    [SerializeField] protected DropIndicator DropSettings;
    protected byte ActiveWeaponIndex 
    { 
        get => activeWeaponIndex;
        set 
        { 
            if(Repository.Items[activeWeaponIndex] != Repository.Items[value]) OnActiveWeaponUpdate();
            activeWeaponIndex = value;
        }
    }
    protected override void Start() 
    {
        base.Start();
        Repository = InventorySystem.GetRepository(repositoryName);
    }
    public virtual void OnAddItem(WeaponItem item, byte index)
    {
        CreateDropInstance(ref item.DropPrefab);
        
        activeWeaponIndex = index;
        OnActiveWeaponUpdate();
    }
    private void CreateDropInstance(ref CollectableObject drop)
    {
        drop = Instantiate(drop, weaponList.position, Quaternion.identity);
        drop.StoredHealth = drop.CurrentHealth;
        drop.transform.SetParent(weaponList);
        drop.gameObject.SetActive(false);
    }
    public void OnBeforeRepositoryUpdate()
    {
        if(Repository.IsRepositoryFull()) DropRelease(ActiveWeaponIndex, 0);
    }
    public void OnRepositoryUpdated(Item item, byte index, RepositoryChangeFeedback feedback)
    {
        Debug.Log("Weapon repository was updated! Item " + item + " was " + feedback + " at index " + index + "!");
        WeaponItem weaponItem = (WeaponItem)item;
        if(feedback == RepositoryChangeFeedback.Added)
                OnAddItem(weaponItem, index);
    }
    protected WeaponItem GetActiveWeapon()
    {
        if(Repository.Items.Length == 0) return null;
        return (WeaponItem)Repository.Items[ActiveWeaponIndex];
    }
    protected override void UpdateWeaponList()
    {
        for(int i = 0; i < Repository.Items.Length; i++)
        {
            if(Repository.Items[i] != null)
            {
                WeaponItem currentItem = (WeaponItem)Repository.Items[i];
                if(currentItem.WeaponInfo.AnimationInfo.inAnimation || currentItem.WeaponInfo.AnimationInfo.timeSinceUse < currentItem.WeaponInfo.GetAttackInterval())
                {
                    currentItem.WeaponInfo.AnimationInfo.timeSinceUse += currentItem.WeaponInfo.AnimationInfo.inAnimation ? 0 : Time.deltaTime;
                    currentItem.WeaponInfo.AnimationInfo.canActivate = false;
                }
                else currentItem.WeaponInfo.AnimationInfo.canActivate = true;
                Repository.Items[i] = currentItem;
            }
        }
    }
    protected void OffsetActiveWeapon(int offset)
    {
        System.Collections.Generic.List<byte> activeSlots = new System.Collections.Generic.List<byte>();

        for(byte i = 0; i < Repository.Items.Length; i++)
        {
            if(Repository.Items[i] != null) activeSlots.Add(i);
        }
        if(activeSlots.Count > 1)
            ActiveWeaponIndex = activeSlots[(byte)Mathf.Repeat(activeSlots.IndexOf(activeWeaponIndex) + offset, activeSlots.Count)];
        else ActiveWeaponIndex = 0;
    }

    protected void SetActiveWeapon(byte index) => ActiveWeaponIndex = index;
    protected void DropPreparation() 
    {
        if(DropSettings.currentDropIndicatorObject?.GetObject() == null) 
        {
            DropSettings.IndicatorStartup(transform);
        }
        
        Pay.UI.UIManager.Indicator.UpdateIndicator(DropSettings.currentDropIndicatorObject, DropSettings.TargetForce, DropSettings.MaxForce - DropSettings.MinForce);
        DropSettings.TargetForce = Mathf.Clamp(DropSettings.TargetForce + DropSettings.ForceAddSpeed * Time.deltaTime, 0, DropSettings.MaxForce - DropSettings.MinForce);
    }
    
    protected void DropRelease(byte index, int offset)
    {
        DropHandler(index);
        Repository.Items[index] = null;
        OnActiveWeaponUpdate();
        OffsetActiveWeapon(1);
    }
    private void DropHandler(byte index)
    {
        InstantiateDroppedObject((WeaponItem)Repository.Items[index], DropDirection, DropDirection *(DropSettings.TargetForce + DropSettings.MinForce));
        Pay.UI.UIManager.RemoveUI(DropSettings.currentDropIndicatorObject);
        DropSettings.TargetForce = 0;
    }
    private CollectableObject InstantiateDroppedObject(WeaponItem instanceObject, Vector2 offsetDirection, Vector2 force)
    {
        CollectableObject droppedObject = Instantiate(instanceObject.DropPrefab, (Vector2)transform.position + offsetDirection * DropSettings.dropOffset, Quaternion.identity);
        
        droppedObject.transform.localScale = instanceObject.DropPrefab.transform.lossyScale;
        droppedObject.gameObject.SetActive(true);
        droppedObject.StoredHealth = instanceObject.DropPrefab.StoredHealth;
        droppedObject.AddConstraintCollider(DropSettings.droppedItemBlockTime, GetComponent<Collider2D>());
        droppedObject.CollisionHandler.Knock(force, Mathf.PI);
        Destroy(instanceObject.DropPrefab.gameObject);
        return droppedObject;
    }
    ///<summary>
    ///Calls when active weapon has changed (Not calls if replaced weapon is equal to current).
    ///</summary>
    protected virtual void OnActiveWeaponUpdate() {}

    [System.Serializable]

    protected internal struct DropIndicator
    {
        [SerializeField] internal float MinForce;
        [SerializeField] internal float MaxForce;
        [SerializeField] internal float ForceAddSpeed;
        [SerializeField] internal float dropOffset;
        [SerializeField] internal Pay.UI.UIHolder Holder;
        [SerializeField, ReadOnly] internal float TargetForce;
        [SerializeField] internal Pay.UI.Indicator dropIndicator;
        [SerializeField] internal Pay.UI.IndicatorObject currentDropIndicatorObject;
        [SerializeField] internal float droppedItemBlockTime;
        internal void IndicatorStartup(Transform transform)
        {
            Pay.UI.UIManager.RemoveUI(currentDropIndicatorObject);

            Pay.UI.UIManager.Indicator.CreateIndicator(Holder, Holder.FollowCanvas, dropIndicator, out currentDropIndicatorObject,
                Pay.UI.Options.Transform.DynamicProperty.LocalScale(Vector3.one / 8, Vector3.one / 4, true, 1f),
                Pay.UI.Options.Transform.StaticProperty.Position(transform.position + Vector3.up)
            );
        }

    }
}
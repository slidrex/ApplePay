using UnityEngine;

public abstract class AdvancedWeaponHolder : WeaponHolder, IRepositoryUpdateHandler
{
    [Header("Weapon display")]
    [SerializeField, Tooltip("The object which stores weapon drop object in inventory.")] private Transform weaponList;
    public InventorySystem InventorySystem;
    [SerializeField] private string repositoryName;
    protected InventoryRepository Repository; 
    [Header("Weapon Switch")]
    [SerializeField, ReadOnly] protected byte ActiveWeaponIndex;
    [SerializeField, ReadOnly] private float targetForce;
    [Header("On drop weapon settings")]
    [SerializeField] protected float minForce;
    [SerializeField] protected float maxForce;
    [SerializeField] protected float forceAddSpeed;
    [SerializeField] private float dropOffset;
    [SerializeField] protected Pay.UI.UIHolder holder;
    [SerializeField] private Pay.UI.Indicator indicator;
    private Pay.UI.IndicatorObject currentDropIndicatorObject;
    [Header("Item Constraints")]
    [SerializeField] private float droppedItemBlockTime;
    private void Start() => Repository = InventorySystem.GetRepository(repositoryName);
    public virtual void OnAddItem(WeaponItem item, byte index)
    {
        CreateDropInstance(item.DropPrefab);
        SetActiveWeapon(index);
    }
    private void CreateDropInstance(CollectableObject drop)
    {
        drop = Instantiate(drop, weaponList.position, Quaternion.identity);
        drop.StoredHealth = drop.CurrentHealth;
        drop.transform.SetParent(weaponList);
        drop.gameObject.SetActive(false);
    }
    public void OnRepositoryUpdated(Item item, byte index, RepositoryChangeFeedback feedback)
    {
        Debug.Log("Weapon repository was updated! Item " + item + " was " + feedback + " at index " + index + "!");
        WeaponItem charmItem = (WeaponItem)item;
        if(feedback == RepositoryChangeFeedback.Added) OnAddItem(charmItem, index);
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
        if(Repository.Items.Length != 0) ActiveWeaponIndex = (byte)Mathf.Repeat(ActiveWeaponIndex + offset, Repository.Items.Length);
        OnActiveWeaponUpdate();
    }
    protected void SetActiveWeapon(byte index) 
    {
        if(index != ActiveWeaponIndex && Repository.Items[index] != Repository.Items[ActiveWeaponIndex]) OnActiveWeaponUpdate();
        ActiveWeaponIndex = index;
    }
    protected virtual void DropStart()
    {
        if(Repository.Items.Length == 0 || Repository.Items[ActiveWeaponIndex] == null) return;
        IndicatorStartup();
        targetForce = 0;
    }
    private void IndicatorStartup()
    {
        Pay.UI.UIManager.RemoveUI(currentDropIndicatorObject);
        
        Pay.UI.UIManager.Indicator.CreateIndicator(holder, holder.FollowCanvas, indicator, out currentDropIndicatorObject,
            Pay.UI.Options.Transform.DynamicProperty.LocalScale(Vector3.one / 8, Vector3.one / 4, true, 1f),
            Pay.UI.Options.Transform.StaticProperty.Position(transform.position + Vector3.up)
        );
    }
    protected void DropPreparation() 
    {
        if(currentDropIndicatorObject == null) DropStart();
        Pay.UI.UIManager.Indicator.UpdateIndicator(currentDropIndicatorObject, targetForce, maxForce);
        targetForce = Mathf.Clamp(targetForce + forceAddSpeed * Time.deltaTime, 0, maxForce);
    }
    protected void DropRelease(byte index)
    {
        DropHandler(index);
        Repository.Items[index] = null;
        OffsetActiveWeapon(-1);
    }
    private void DropHandler(byte index)
    {
        if(Repository.Items.Length == 0) return;
        InstantiateDroppedObject((WeaponItem)Repository.Items[index], SetDropDirection(), SetDropDirection() *(targetForce + minForce));
        if(currentDropIndicatorObject != null) Pay.UI.UIManager.RemoveUI(currentDropIndicatorObject);
        targetForce = 0;
        OnActiveWeaponUpdate();
    }
    private CollectableObject InstantiateDroppedObject(WeaponItem instanceObject, Vector2 offsetDirection, Vector2 force)
    {
        CollectableObject droppedObject = Instantiate(instanceObject.DropPrefab, (Vector2)transform.position + offsetDirection * dropOffset, Quaternion.identity);
        
        droppedObject.transform.localScale = instanceObject.DropPrefab.transform.lossyScale;
        droppedObject.gameObject.SetActive(true);
        droppedObject.StoredHealth = instanceObject.DropPrefab.StoredHealth;
        droppedObject.AddConstraintCollider(droppedItemBlockTime, GetComponent<Collider2D>());
        droppedObject.AddForce(force);
        Destroy(instanceObject.DropPrefab.gameObject);
        return droppedObject;
    }
    ///<summary>
    ///Calls when active weapon has changed (Not calls if replaced weapon is equal to current).
    ///</summary>
    protected virtual void OnActiveWeaponUpdate() {}
    protected abstract Vector2 SetDropDirection();
}
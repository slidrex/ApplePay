using UnityEngine;
public abstract class AdvancedWeaponHolder : WeaponHolder
{
    [Header("Weapon display")]
    [SerializeField, Tooltip("The object which stores weapon drop object in inventory.")] private Transform weaponList;
    public WeaponRepository Repository;
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
    public virtual void OnAddItem(ref WeaponItem item)
    {
        if(weaponList == null) throw new System.NullReferenceException("Weapon list hasn't been specified.");
        CreateDropInstance(ref item.DropPrefab);
        SetActiveWeapon(item);
    }
    public virtual void OnCapacityLimit(ref WeaponItem item)
    {
        DropRelease(GetActiveWeapon());
    }
    private void CreateDropInstance(ref CollectableObject drop)
    {
        CollectableObject dropPrefab = Instantiate(drop, weaponList.position, Quaternion.identity);
        dropPrefab.StoredHealth = drop.CurrentHealth;
        drop = dropPrefab;
        dropPrefab.transform.SetParent(weaponList);
        dropPrefab.gameObject.SetActive(false);
    }
    protected WeaponItem GetActiveWeapon()
    {
        if(Repository.InventoryItems.Count == 0) return null;
        return Repository.InventoryItems[ActiveWeaponIndex];
    }
    protected override void UpdateWeaponList()
    {
        for(int i = 0; i < Repository.InventoryItems.Count; i++)
        {
            WeaponItem currentItem = Repository.InventoryItems[i];
            if(currentItem.WeaponInfo.AnimationInfo.inAnimation || currentItem.WeaponInfo.AnimationInfo.timeSinceUse < currentItem.WeaponInfo.GetAttackInterval())
            {
                currentItem.WeaponInfo.AnimationInfo.timeSinceUse += currentItem.WeaponInfo.AnimationInfo.inAnimation ? 0 : Time.deltaTime;
                currentItem.WeaponInfo.AnimationInfo.canActivate = false;
            }
            else currentItem.WeaponInfo.AnimationInfo.canActivate = true;
            Repository.InventoryItems[i] = currentItem;
        }
    }
    protected void SetActiveWeapon(int offset)
    {
        if(Repository.InventoryItems.Count != 0) ActiveWeaponIndex = (byte)Mathf.Repeat(ActiveWeaponIndex + offset, Repository.InventoryItems.Count);
        OnActiveWeaponUpdate();
    }
    protected void SetActiveWeapon(WeaponItem weaponItem)
    {
        if(Repository.InventoryItems.Count != 0) ActiveWeaponIndex = (byte)Repository.InventoryItems.IndexOf(weaponItem);
        OnActiveWeaponUpdate();
    }
    protected virtual void DropStart()
    {
        if(Repository.InventoryItems.Count == 0 || Repository.InventoryItems[ActiveWeaponIndex] == null) return;
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
    protected void DropRelease(WeaponItem dropItem)
    {
        DropHandler(dropItem);
        Repository.RemoveItem(dropItem, out bool removed);
        SetActiveWeapon(-1);
    }
    private void DropHandler(WeaponItem drop)
    {
        if(Repository.InventoryItems.Count == 0 || Repository.InventoryItems.Contains(drop) == false) return;
        InstantiateDroppedObject(drop, SetDropDirection(), SetDropDirection() * (targetForce + minForce));
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
    protected virtual void OnActiveWeaponUpdate() {}
    protected abstract Vector2 SetDropDirection();
}

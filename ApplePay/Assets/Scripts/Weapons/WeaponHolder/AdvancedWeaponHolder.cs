using UnityEngine;

public abstract class AdvancedWeaponHolder : WeaponHolder, IRepositoryUpdateCallback<CollectableWeapon>
{
    [Header("Weapon display")]
    public InventorySystem InventorySystem;
    private const string repositoryName = "weapon";
    protected abstract Vector2 DropDirection { get; }
    protected WeaponRepository Repository; 
    [Header("Weapon Switch")]
    private byte activeWeaponIndex;
    [SerializeField] protected DropIndicator DropSettings;
    protected byte ActiveWeaponIndex 
    {
        get => activeWeaponIndex;
        private set 
        { 
            if(Repository.Items[activeWeaponIndex] != Repository.Items[value]) OnActiveWeaponUpdate();
            activeWeaponIndex = value;
            OnActiveWeaponIndexSet();
        }
    }
    public void OnBeforeRepositoryUpdate(InventoryRepository.UpdateType type, ref CollectableWeapon weapon)
    {
        if(type == InventoryRepository.UpdateType.Add)
            if(Repository.IsFull()) DropRelease(ActiveWeaponIndex, 0);
    }
    protected override void Start() 
    {
        base.Start();
        Repository = (WeaponRepository)InventorySystem.GetRepository(repositoryName);
        Repository.RepositoryUpdateCallback += OnBeforeRepositoryUpdate;
    }
    private void OnDestroy()
    {
        Repository.RepositoryUpdateCallback -= OnBeforeRepositoryUpdate;
    }
    public virtual void OnAddItem(CollectableWeapon item, byte index)
    {
        ActiveWeaponIndex = index;
    }
    protected CollectableWeapon GetActiveWeapon()
    {
        if(Repository.Items.Length == 0) return null;
        return (CollectableWeapon)Repository.Items[ActiveWeaponIndex];
    }
    protected override void UpdateWeaponList()
    {
        CollectableWeapon[] items = Repository.GetExistingItems();
        for(int i = 0; i < Repository.Items.Length; i++)
        {
            if(Repository.Items[i] != null)
            {
                CollectableWeapon currentItem = (CollectableWeapon)Repository.Items[i];
                
                if(currentItem.weapon.weaponInfo.OnCooldown && currentItem.weapon.weaponInfo.timeSinceUse < currentItem.weapon.weaponInfo.AttackCooldown)
                {
                    currentItem.weapon.weaponInfo.timeSinceUse += Time.deltaTime;
                    currentItem.weapon.weaponInfo.isActivatable = false;
                }
                else if(currentItem.weapon.weaponInfo.timeSinceUse >= currentItem.weapon.weaponInfo.AttackCooldown && currentItem.weapon.weaponInfo.OnCooldown)
                {
                    currentItem.weapon.weaponInfo.isActivatable = true;
                    currentItem.weapon.weaponInfo.OnCooldown = false;    
                }
                Repository.Items[i] = currentItem;
            }
        }
        UpdateDisableStatus();
    }
    private void UpdateDisableStatus()
    {
        if(Disable && DropSettings.currentDropIndicatorObject != null) 
        {
            DropSettings.TargetForce = 0;
            Pay.UI.UIManager.RemoveUI(DropSettings.currentDropIndicatorObject);
            return;
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
            ActiveWeaponIndex = activeSlots[(byte)Mathf.Repeat(activeSlots.IndexOf(ActiveWeaponIndex) + offset, activeSlots.Count)];
        else ActiveWeaponIndex = 0;
    }

    protected void SetActiveWeapon(byte index) => ActiveWeaponIndex = index;
    protected void DropPreparation() 
    {
        if(DropSettings.currentDropIndicatorObject?.GetObject() == null) 
        {
            OnDropStart();
            DropSettings.IndicatorStartup(transform);
        }
        
        
        Pay.UI.UIManager.Indicator.UpdateIndicator(DropSettings.currentDropIndicatorObject, DropSettings.TargetForce, DropSettings.MaxForce - DropSettings.MinForce);
        DropSettings.TargetForce = Mathf.Clamp(DropSettings.TargetForce + DropSettings.ForceAddSpeed * Time.deltaTime, 0, DropSettings.MaxForce - DropSettings.MinForce);
    }
    protected virtual void DropCancel()
    {
        DropSettings.TargetForce = 0.0f;
        DropSettings.currentDropIndicatorObject?.OnRemove();
    }
    protected virtual void OnDropStart() { }
    protected void DropRelease(byte index, int offset)
    {
        DropHandler(index);
        
        Repository.Items[index] = null;
        
        OffsetActiveWeapon(1);
    }
    private void DropHandler(byte index)
    {
        GetDroppedObject(Repository.Items[index], DropDirection, DropDirection *(DropSettings.TargetForce + DropSettings.MinForce));
        Pay.UI.UIManager.RemoveUI(DropSettings.currentDropIndicatorObject);
        DropSettings.TargetForce = 0;
    }
    private CollectableObject GetDroppedObject(CollectableWeapon instanceObject, Vector2 offsetDirection, Vector2 force)
    {
        Vector2 lossyScale = instanceObject.gameObject.transform.lossyScale;
        instanceObject.AddConstraintCollider(InventorySystem.dropItemBlockTime, Owner.HitShape);
        instanceObject.transform.position += (Vector3)offsetDirection;
        instanceObject.gameObject.SetActive(true);
        instanceObject.gameObject.transform.SetParent(null);
        instanceObject.gameObject.transform.localScale = lossyScale;
        instanceObject.ForceHandler.Knock(force, Mathf.PI);
        return instanceObject;
    }
    ///<summary>
    ///Calls when active weapon has changed (Not calls if replaced weapon is equal to current).
    ///</summary>
    protected virtual void OnActiveWeaponUpdate() {}
    protected virtual void OnActiveWeaponIndexSet() {}
    [System.Serializable]

    protected internal struct DropIndicator
    {
        [SerializeField] internal float MinForce;
        [SerializeField] internal float MaxForce;
        [SerializeField] internal float ForceAddSpeed;
        [SerializeField] internal Pay.UI.UIHolder Holder;
        internal float TargetForce;
        [SerializeField] internal Pay.UI.Indicator dropIndicator;
        [SerializeField] internal Pay.UI.IndicatorObject currentDropIndicatorObject;
        internal void IndicatorStartup(Transform transform)
        {
            Pay.UI.UIManager.RemoveUI(currentDropIndicatorObject);

            currentDropIndicatorObject = Pay.UI.UIManager.Indicator.CreateIndicator(Holder, Holder.FollowCanvas, dropIndicator,
                Pay.UI.Options.Transform.DynamicProperty.LocalScale(Vector3.one / 8, Vector3.one / 4, true, 1f),
                Pay.UI.Options.Transform.StaticProperty.Position(transform.position + Vector3.up)
            );
        }

    }
}
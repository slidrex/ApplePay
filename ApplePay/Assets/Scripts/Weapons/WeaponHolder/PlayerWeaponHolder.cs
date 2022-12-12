using UnityEngine;

public class PlayerWeaponHolder : AdvancedWeaponHolder
{
    [Header("Player Holder")]
    [SerializeField] private KeyCode switchKey;
    [SerializeField] private KeyCode dropKey;
    [SerializeField] private KeyCode activateKey;
    [SerializeField] private WeaponPlaceSlot weaponPlaceSlot;
    protected override Vector2 DropDirection => (Pay.Functions.Generic.GetMousePos(Camera.main) - (Vector2)transform.position).normalized;
    override protected void Update()
    {
        base.Update();
        
        if(!Disable) InventoryController();
        UpdateSlotIndicator();
    }
    private void InventoryController()
    {
        if(Input.GetKeyDown(switchKey)) 
        {
            CollectableWeapon old = GetActiveWeapon();
            OffsetActiveWeapon(1);
            if(old != null && old == GetActiveWeapon()) SetupText();
        }
        if(Input.GetKey(activateKey)) 
        {
            Weapon current = GetActiveWeapon().weapon;
            
            Activate(Owner, ref current, Pay.Functions.Generic.GetMousePos(Camera.main), null, out Projectile projectile);
        }
        if(GetActiveWeapon() != null && Repository.Items.Length != 0)
        {
            if(Input.GetKey(dropKey)) DropPreparation();
            if(Input.GetKeyUp(dropKey)) DropRelease(ActiveWeaponIndex, -1);
        }
    }
    
    private void UpdateSlotIndicator()
    {
        if(GetActiveWeapon() != null && weaponPlaceSlot.IndicatorBuffer != null)
            weaponPlaceSlot.SlotIndicatorUpdate(DropSettings.Holder, GetActiveWeapon().weapon.WeaponInfo.AnimationInfo.timeSinceUse, GetActiveWeapon().weapon.WeaponInfo.GetAttackInterval());
    }
    public override void OnWeaponActivate(Weapon weapon, bool status)
    {
        weaponPlaceSlot.RemoveIndicator();
        if(weapon != null) weaponPlaceSlot.CreateSlotIndicator(DropSettings.Holder);
    }
    protected override void OnActiveWeaponUpdate()
    {
        weaponPlaceSlot.RemoveSlotUI();
        
        CollectableWeapon currentItem = GetActiveWeapon();
        if(currentItem != null)
        {
            weaponPlaceSlot.CreateSlotIndicator(DropSettings.Holder);
            weaponPlaceSlot.SetItem(currentItem.weapon.WeaponInfo.Display.Icon);
            weaponPlaceSlot.CreateSlotText(DropSettings.Holder, GetActiveWeapon().weapon.WeaponInfo.Display.Description.Name);
        }
    }
    private void SetupText()
    {
        weaponPlaceSlot.RemoveText();
        weaponPlaceSlot.CreateSlotText(DropSettings.Holder, GetActiveWeapon().weapon.WeaponInfo.Display.Description.Name);
    }
}
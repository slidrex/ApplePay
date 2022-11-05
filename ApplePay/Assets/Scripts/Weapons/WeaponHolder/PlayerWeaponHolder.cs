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
        InventoryController();
        UpdateSlotIndicator();
    }
    private void InventoryController()
    {
        if(Input.GetKeyDown(switchKey)) OffsetActiveWeapon(1);
        if(Input.GetKey(activateKey)) 
        {
            WeaponItem current = GetActiveWeapon();
            
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
            weaponPlaceSlot.SlotIndicatorUpdate(DropSettings.Holder, GetActiveWeapon().WeaponInfo.AnimationInfo.timeSinceUse, GetActiveWeapon().WeaponInfo.GetAttackInterval());
    }
    public override void OnWeaponActivate(WeaponItem weapon, bool status)
    {
        weaponPlaceSlot.RemoveIndicator();
        if(weapon != null) weaponPlaceSlot.CreateSlotIndicator(DropSettings.Holder);
    }
    protected override void OnActiveWeaponUpdate()
    {
        weaponPlaceSlot.RemoveSlotUI();
        
        WeaponItem currentItem = GetActiveWeapon();
        if(currentItem != null)
        {
            weaponPlaceSlot.CreateSlotIndicator(DropSettings.Holder);
            weaponPlaceSlot.SetItem(currentItem.WeaponInfo.Display.InventorySprite);
            weaponPlaceSlot.CreateSlotText(DropSettings.Holder, GetActiveWeapon().WeaponInfo.Display.Description.Name);

        }
    }
}

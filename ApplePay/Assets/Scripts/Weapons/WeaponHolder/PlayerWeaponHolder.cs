using UnityEngine;

public class PlayerWeaponHolder : AdvancedWeaponHolder
{
    [Header("Player Holder")]
    [SerializeField] private KeyCode switchKey;
    [SerializeField] private KeyCode dropKey;
    [SerializeField] private KeyCode activateKey;
    [SerializeField] private WeaponPlaceSlot weaponPlaceSlot;
    protected override void Update()
    {
        base.Update();
        InventoryController();
        UpdateSlotIndicator();
    }
    private void InventoryController()
    {
        if(Input.GetKeyDown(switchKey)) SetActiveWeapon(1);
        if(Input.GetKey(activateKey)) 
        {
            WeaponItem current = GetActiveWeapon();
            
            Activate(GetComponent<Creature>(), ref current, Pay.Functions.Generic.GetMousePos(Camera.main), null, out Projectile projectile);
        }
        if(GetActiveWeapon() != null)
        {
            if(Input.GetKey(dropKey)) DropPreparation();
            if(Input.GetKeyUp(dropKey)) DropRelease(ActiveWeaponIndex);
        }
    }
    private void UpdateSlotIndicator()
    {
        //if(GetActiveWeapon() != null)
        //    weaponPlaceSlot.SlotIndicatorUpdate(holder, GetActiveWeapon().WeaponInfo.AnimationInfo.timeSinceUse, GetActiveWeapon().WeaponInfo.GetAttackInterval());
    }
    public override void OnWeaponActivate(WeaponItem weapon, bool status)
    {
        weaponPlaceSlot?.RemoveIndicator();
        if(weapon != null) weaponPlaceSlot?.CreateSlotIndicator(holder);
    }
    protected override void OnActiveWeaponUpdate()
    {
        weaponPlaceSlot.SetItem(null);
        weaponPlaceSlot.RemoveSlotUI();
        if(GetActiveWeapon() != null)
        {
            weaponPlaceSlot.CreateSlotIndicator(holder);
            weaponPlaceSlot.SetItem(GetActiveWeapon().WeaponInfo.Display.InventorySprite);
        }
    }
    protected override Vector2 SetDropDirection() => (Pay.Functions.Generic.GetMousePos(Camera.main) - (Vector2)transform.position).normalized;
}

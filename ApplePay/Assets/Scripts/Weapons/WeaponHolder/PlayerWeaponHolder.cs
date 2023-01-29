using UnityEngine;

public class PlayerWeaponHolder : AdvancedWeaponHolder
{
    [Header("Player Holder")]
    [SerializeField] private KeyCode switchKey;
    [SerializeField] private KeyCode dropKey;
    [SerializeField] private KeyCode activateKey;
    [SerializeField] private WeaponPlaceSlot weaponPlaceSlot;
    protected override Vector2 DropDirection => (Pay.Functions.Generic.GetMousePos(Camera.main) - (Vector2)transform.position).normalized;
    private Creature.EntityState state;
    private bool dropping;
    public Creature creature;
    override protected void Update()
    {
        base.Update();
        if(!Disable) InventoryController();
        UpdateSlotIndicator();
    }
    private void InventoryController()
    {
        if(Input.GetKeyDown(switchKey) && Owner.IsFree()) 
        {
            OffsetActiveWeapon(1);
            
            if(GetActiveWeapon() != null) SetupText();
        }
        if(Input.GetKey(activateKey) && Owner.IsFree())
        {
            Weapon current = GetActiveWeapon()?.Weapon;
            if(current != null)
                Activate(Owner, ref current, Owner.transform.position, Pay.Functions.Generic.GetMousePos(Camera.main), null);
        }
        if(GetActiveWeapon() != null && Repository.Items.Length != 0)
        {
            if(Input.GetKey(dropKey) && ((dropping == false && Owner.IsFree()) || dropping == true)) DropPreparation();
            if(Input.GetKeyUp(dropKey) && dropping)
            {
                DropRelease(ActiveWeaponIndex, -1);
                state.Remove();
            }
        }
    }
    protected override void OnDropStart()
    {
        dropping = true;
        state = Owner.Engage(DropCancel);
    }
    
    private void UpdateSlotIndicator()
    {
        if(GetActiveWeapon() != null && weaponPlaceSlot.IndicatorBuffer != null)
            weaponPlaceSlot.SlotIndicatorUpdate(DropSettings.Holder, GetActiveWeapon().Weapon.weaponInfo.timeSinceUse, GetActiveWeapon().Weapon.weaponInfo.AttackCooldown);
    }
    public override void OnWeaponActivate(Weapon weapon, bool status)
    {
        weaponPlaceSlot.RemoveIndicator();
        if(weapon != null) weaponPlaceSlot.CreateSlotIndicator(DropSettings.Holder);
    }
    protected override void OnActiveWeaponIndexSet()
    {
        weaponPlaceSlot.RemoveSlotUI();
        
        CollectableWeapon currentItem = GetActiveWeapon();
        if(currentItem != null)
        {
            int energyConsumption = currentItem.Weapon.EnergyConsumption;
            if(energyConsumption != 0)
                weaponPlaceSlot.RenderEnergyCost(energyConsumption, true);
            else weaponPlaceSlot.RenderEnergyCost(0, false);
            weaponPlaceSlot.CreateSlotIndicator(DropSettings.Holder);
            weaponPlaceSlot.SetItem(currentItem.Weapon.display.Icon);
            weaponPlaceSlot.CreateSlotText(DropSettings.Holder, GetActiveWeapon().Weapon.display.Description.Name);
        }
    }
    private void SetupText()
    {
        weaponPlaceSlot.RemoveText();
        weaponPlaceSlot.CreateSlotText(DropSettings.Holder, GetActiveWeapon().Weapon.display.Description.Name);
    }
}
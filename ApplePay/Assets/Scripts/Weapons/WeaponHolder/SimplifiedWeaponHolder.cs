using UnityEngine;

public class SimplifiedWeaponHolder : WeaponHolder
{
    public Weapon ActiveWeapon;
    protected override void UpdateWeaponList()
    {
        if(ActiveWeapon != null)
        {
            if(ActiveWeapon.weaponInfo.OnCooldown && ActiveWeapon.weaponInfo.timeSinceUse < ActiveWeapon.weaponInfo.AttackCooldown)
            {
                ActiveWeapon.weaponInfo.isActivatable = false;
                ActiveWeapon.weaponInfo.timeSinceUse += Time.deltaTime;
            }
            else
                ActiveWeapon.weaponInfo.isActivatable = true;
        }
    }
}
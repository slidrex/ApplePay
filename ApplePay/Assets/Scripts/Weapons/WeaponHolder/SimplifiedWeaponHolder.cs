using UnityEngine;

public class SimplifiedWeaponHolder : WeaponHolder
{
    public Weapon ActiveWeapon;
    protected override void UpdateWeaponList()
    {
        if(ActiveWeapon == null) return;
        if(ActiveWeapon.WeaponInfo.AnimationInfo.inAnimation || ActiveWeapon.WeaponInfo.AnimationInfo.timeSinceUse < ActiveWeapon.WeaponInfo.GetAttackCooldown())
        {
            ActiveWeapon.WeaponInfo.AnimationInfo.canActivate = false;
            ActiveWeapon.WeaponInfo.AnimationInfo.timeSinceUse += Time.deltaTime;
        }
        else
            ActiveWeapon.WeaponInfo.AnimationInfo.canActivate = true;
    }
}
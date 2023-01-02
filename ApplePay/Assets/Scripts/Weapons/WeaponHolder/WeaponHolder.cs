using UnityEngine;
public abstract class WeaponHolder : MonoBehaviour
{
    [Header("Weapon Holder")]
    public WeaponPlace WeaponPlace;
    protected Creature Owner { get; set; }
    public bool Disable { get => Disables.Count != 0; }
    public System.Collections.Generic.List<float> Disables = new System.Collections.Generic.List<float>();
    private const float additionalFreezeStateTime = .5f;
    protected virtual void Start() => Owner = GetComponent<Creature>();
    protected virtual void Update()
    {
        UpdateWeaponList();
    }
    protected virtual void UpdateWeaponList() { }
    public virtual bool Activate(Creature attacker, ref Weapon activeWeapon, Vector2 endTrajectory, Transform target, out Projectile projectile)
    {
        return ActivateHandler(attacker, transform.position, endTrajectory, ref activeWeapon, target, out projectile);
    }
    public virtual bool Activate(Creature attacker, ref Weapon activeWeapon, Vector2 endTrajectory, Transform target) => ActivateHandler(attacker, transform.position, endTrajectory, ref activeWeapon, target, out Projectile projectile);
    public virtual bool Activate(Creature attacker, ref Weapon activeWeapon, Vector2 beginTrajectory, Vector2 endTrajectory, Transform target) => ActivateHandler(attacker, beginTrajectory, endTrajectory, ref activeWeapon, target, out Projectile projectile);
    private bool ActivateHandler(Creature attacker, Vector2 beginTrajectory , Vector2 endTrajectory, ref Weapon activeWeapon, Transform target, out Projectile projectile)
    {
        if(activeWeapon == null || Disable || !activeWeapon.WeaponInfo.AnimationInfo.canActivate || attacker.IsFree() == false) 
        {
            projectile = null;
            OnWeaponActivate(activeWeapon, false);
            return false;
        };
        activeWeapon.WeaponInfo.AnimationInfo.timeSinceUse = 0;
        attacker.Engage(activeWeapon.WeaponInfo.GetAnimationTime(), null);
        SetFacing(activeWeapon.WeaponInfo.GetAnimationTime(), beginTrajectory, endTrajectory, WeaponPlace.FreezeHorizontal, WeaponPlace.FreezeVertical);

        activeWeapon.Activate(attacker, beginTrajectory, endTrajectory, out GameObject weaponObject, target, out projectile);
        WeaponPlace.ActivateAnimation(activeWeapon, weaponObject, endTrajectory);
        
        OnWeaponActivate(activeWeapon, true);
        return true;
    }
    public virtual void OnWeaponActivate(Weapon weapon, bool status) {}
    private void SetFacing(float freezeTime, Vector2 beginPosition, Vector2 endPosition, bool freezeHorizontal, bool freezeVertical)
    {
        Vector2 distance = endPosition - beginPosition;
        
        Vector2 facing = Mathf.Abs(distance.x) > Mathf.Abs(distance.y) ? Vector2.right * Mathf.Sign(distance.x) : Vector2.up * Mathf.Sign(distance.y);
        if(freezeHorizontal) facing = Vector2.up * Mathf.Sign(distance.y);
        if(freezeVertical) facing = Vector2.right * Mathf.Sign(distance.x);
        if(freezeHorizontal && freezeVertical) facing = Vector2.zero;
        Owner.Movement.SetFacingState(facing, freezeTime + additionalFreezeStateTime, StateParameter.MirrorHorizontal);
    }
    
}

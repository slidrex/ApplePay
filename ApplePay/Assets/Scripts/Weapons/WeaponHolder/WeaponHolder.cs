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
    public virtual bool Activate(Creature attacker, ref Weapon activeWeapon, Vector2 endTrajectory, Transform target, out GameObject output)
    {
        return ActivateHandler(attacker, transform.position, endTrajectory, ref activeWeapon, target, out output);
    }
    public virtual bool Activate(Creature attacker, ref Weapon activeWeapon, Vector2 beginTrajectory, Vector2 endTrajectory, Transform target) => ActivateHandler(attacker, beginTrajectory, endTrajectory, ref activeWeapon, target, out GameObject output);
    private bool ActivateHandler(Creature attacker, Vector2 beginTrajectory , Vector2 endTrajectory, ref Weapon activeWeapon, Transform target, out GameObject output)
    {
        if(activeWeapon == null || Disable || activeWeapon.weaponInfo.isActivatable == false || attacker.IsFree() == false) 
        {
            output = null;
            OnWeaponActivate(activeWeapon, false);
            return false;
        };
        InstantiateWeapon animatableWeapon = activeWeapon as InstantiateWeapon;
        float facingTime = activeWeapon.weaponInfo.AdditionalFacingTime;
        if(animatableWeapon != null) facingTime += animatableWeapon.animationInfo.animationTime;
        Vector2 facing = SetFacing(facingTime, beginTrajectory, endTrajectory, WeaponPlace.FreezeHorizontal, WeaponPlace.FreezeVertical);
        activeWeapon.Activate(attacker, beginTrajectory, endTrajectory, target, out output);
        if(animatableWeapon != null)
        {
            float animationTime = animatableWeapon.animationInfo.animationTime;
            attacker.Engage(animationTime, null);
            WeaponPlace.ActivateAnimation(animatableWeapon, output, endTrajectory);
            if(facing == Vector2.down) output.GetComponent<SpriteRenderer>().sortingOrder = 1;
            activeWeapon.weaponInfo.timeSinceUse = 0.0f;
            activeWeapon.weaponInfo.isActivatable = false;
        }
        else
        {
            activeWeapon.weaponInfo.SetCooldown();
        }
        
        OnWeaponActivate(activeWeapon, true);
        return true;
    }
    public virtual void OnWeaponActivate(Weapon weapon, bool status) {}
    protected Vector2 SetFacing(float freezeTime, Vector2 beginPosition, Vector2 endPosition, bool freezeHorizontal, bool freezeVertical)
    {
        Vector2 distance = endPosition - beginPosition;
        
        Vector2 facing = Mathf.Abs(distance.x) > Mathf.Abs(distance.y) ? Vector2.right * Mathf.Sign(distance.x) : Vector2.up * Mathf.Sign(distance.y);
        if(freezeHorizontal) facing = Vector2.up * Mathf.Sign(distance.y);
        if(freezeVertical) facing = Vector2.right * Mathf.Sign(distance.x);
        if(freezeHorizontal && freezeVertical) facing = Vector2.zero;

        Owner.Movement.SetFacing(facing, freezeTime + additionalFreezeStateTime, StateParameter.MirrorHorizontal);
        return facing;
    }
    
}

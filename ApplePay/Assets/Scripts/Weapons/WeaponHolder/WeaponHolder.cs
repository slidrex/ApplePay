using UnityEngine;
public abstract class WeaponHolder : MonoBehaviour
{
    [Header("Weapon Holder")]
    public WeaponPlace WeaponPlace;
    protected Creature Owner { get; set; }
    [HideInInspector] public bool Disable; 
    private const float additionalFreezeStateTime = .5f;
    private void Start() => Owner = GetComponent<Creature>();
    protected virtual void Update()
    {
        if(!Disable) UpdateWeaponList();
    }
    protected virtual void UpdateWeaponList() { }
    public virtual void Activate(Creature attacker, ref WeaponItem activeWeapon, Vector2 endTrajectory, Transform target, out Projectile projectile)
    {
        ActivateHandler(attacker, transform.position, endTrajectory, ref activeWeapon, target, out projectile);
    }
    public virtual void Activate(Creature attacker, ref WeaponItem activeWeapon, Vector2 endTrajectory, Transform target) => ActivateHandler(attacker, transform.position, endTrajectory, ref activeWeapon, target, out Projectile projectile);
    public virtual void Activate(Creature attacker, ref WeaponItem activeWeapon, Vector2 beginTrajectory, Vector2 endTrajectory, Transform target) => ActivateHandler(attacker, beginTrajectory, endTrajectory, ref activeWeapon, target, out Projectile projectile);
    private void ActivateHandler(Creature attacker, Vector2 beginTrajectory , Vector2 endTrajectory, ref WeaponItem activeWeapon, Transform target, out Projectile projectile)
    {
        if(activeWeapon == null || Disable || !activeWeapon.WeaponInfo.AnimationInfo.canActivate) 
        {
            projectile = null;
            OnWeaponActivate(activeWeapon, false);
            return;
        };
        activeWeapon.WeaponInfo.AnimationInfo.timeSinceUse = 0;

        SetFacing(beginTrajectory, endTrajectory, WeaponPlace.FreezeHorizontal, WeaponPlace.FreezeVertical);
        WeaponPlace.WeaponActivate(attacker, beginTrajectory, endTrajectory, activeWeapon, target, out projectile);
        OnWeaponActivate(activeWeapon, true);
    }
    public virtual void OnWeaponActivate(WeaponItem weapon, bool status) {}
    private void SetFacing(Vector2 beginPosition, Vector2 endPosition, bool freezeHorizontal, bool freezeVertical)
    {
        Vector2 distance = endPosition - beginPosition;
        float freezeTime = WeaponPlace.animator.GetRemainAnimationTime();
        Vector2 facing = Mathf.Abs(distance.x) > Mathf.Abs(distance.y) ? Vector2.right * Mathf.Sign(distance.x) : Vector2.up * Mathf.Sign(distance.y);
        if(freezeHorizontal) facing = Vector2.up * Mathf.Sign(distance.y);
        if(freezeVertical) facing = Vector2.right * Mathf.Sign(distance.x);
        if(freezeHorizontal && freezeVertical) facing = Vector2.zero;
        GetComponent<EntityMovement>().SetFacingState(facing, freezeTime + additionalFreezeStateTime, StateParameter.MirrorHorizontal);
    }
    
}

using UnityEngine;

public class WeaponPlace : MonoBehaviour
{
    public Vector2 HorizontalOffset = Vector2.right;
    public Vector2 VerticalOffset = Vector2.up;
    public bool FreezeHorizontal, FreezeVertical;
    public float Radius;
    public float WeaponScale = 1f;
    [HideInInspector] public WeaponPlaceAnimator animator = new WeaponPlaceAnimator();
    public void WeaponActivate(Creature attacker, Vector2 origin, Vector2 attackPosition, WeaponItem weaponItem, Transform target, out Projectile projectile)
    {
        weaponItem.Weapon.Activate(attacker, origin, attackPosition, out GameObject weaponObject, target, out projectile);
        
        Transform container = new GameObject().transform;
        container.transform.position = transform.position;
        container.transform.SetParent(transform);
        weaponObject.transform.position = transform.position;
        weaponObject.transform.SetParent(container, true);
        weaponObject.transform.localScale *= WeaponScale;
        TranformPlaceSetup(container, weaponItem.Weapon.AttackAnimationSettings.AttackType, attackPosition, out Vector2 _facing);
        animator.StartAnimation(weaponItem, container);
    }
    private void TranformPlaceSetup(Transform transform, AttackType type, Vector2 attackPosition, out Vector2 facing)
    {
        Vector2 dist = attackPosition - (Vector2)transform.position;
        Vector2 offset, attackDirection;
        if(type == AttackType.AllDir)
        {
            attackDirection = dist.normalized;
            offset = Radius * attackDirection;
        }
        else
        {
            if(!FreezeHorizontal && !FreezeVertical) attackDirection = Mathf.Abs(dist.x) > Mathf.Abs(dist.y) ?  Vector2.right * Mathf.Sign(dist.x) : Vector2.up * Mathf.Sign(dist.y);
            else attackDirection = FreezeHorizontal && !FreezeVertical ? Vector2.up * Mathf.Sign(dist.y) : Vector2.right * Mathf.Sign(dist.x);
            offset = attackDirection.x == 0 ? attackDirection.y * VerticalOffset : attackDirection.x * HorizontalOffset;
        }
        facing = Mathf.Abs(attackDirection.x) > Mathf.Abs(attackDirection.y) ? Vector2.right * Mathf.Sign(attackDirection.x) : Vector2.up * Mathf.Sign(attackDirection.y);
        float rotation = Pay.Functions.Math.Atan3(attackDirection.y, attackDirection.x);
        transform.eulerAngles = Vector3.forward * rotation;
        transform.position += (Vector3)offset;
    }
    private void Update() => animator.Update();
    
}

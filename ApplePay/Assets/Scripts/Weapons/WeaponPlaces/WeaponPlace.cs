using UnityEngine;

public class WeaponPlace : MonoBehaviour
{
    public Vector2 HorizontalOffset = Vector2.right;
    public Vector2 VerticalOffset = Vector2.up;
    public bool FreezeHorizontal, FreezeVertical;
    public float Radius;
    public float WeaponScale = 1f;
    [HideInInspector] public WeaponPlaceAnimator animator = new WeaponPlaceAnimator();
    public void ActivateAnimation(InstantiateWeapon weapon, GameObject animateObject, Vector2 attackPosition)
    {   
        AnimateObjectSetup(animateObject, out Transform container);
        TranformPlaceSetup(container, weapon.attackType, attackPosition, out Vector2 _facing);
        animator.StartAnimation(weapon, container);
    }
    private void AnimateObjectSetup(GameObject animateObject, out Transform container)
    {
        container = new GameObject().transform;
        container.transform.position = transform.position;
        container.transform.SetParent(transform);
        animateObject.transform.position = transform.position;
        animateObject.transform.SetParent(container, true);
        animateObject.transform.localScale *= WeaponScale;

    }
    private void TranformPlaceSetup(Transform transform, Weapon.AttackType type, Vector2 attackPosition, out Vector2 facing)
    {
        Vector2 dist = attackPosition - (Vector2)transform.position;
        Vector2 offset, attackDirection;
        if(type == Weapon.AttackType.Radial)
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

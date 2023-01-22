using UnityEngine;

public class WeaponPlace : MonoBehaviour
{
    public Vector2 HorizontalOffset = Vector2.right;
    public Vector2 VerticalOffset = Vector2.up;
    public bool FreezeHorizontal, FreezeVertical;
    public float Radius;
    public float WeaponScale = 1f;
    [HideInInspector] public WeaponPlaceAnimator animator = new WeaponPlaceAnimator();
    public void ActivateAnimation(InstantiateWeapon weapon, GameObject animateObject, Vector2 attackDirection)
    {   
        AnimateObjectSetup(animateObject, out Transform container);
        TranformPlaceSetup(container, weapon.attackType, attackDirection);
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
    private void TranformPlaceSetup(Transform transform, Weapon.AttackType type, Vector2 attackDirection)
    {
        Vector2 offset;
        if(type == Weapon.AttackType.Radial)
        {
            offset = Radius * attackDirection;
        }
        else
        {
            offset = attackDirection.x == 0 ? attackDirection.y * VerticalOffset : attackDirection.x * HorizontalOffset;
        }

        float rotation = Pay.Functions.Math.Atan3(attackDirection.y, attackDirection.x);
        transform.eulerAngles = Vector3.forward * rotation;
        transform.position += (Vector3)offset;
    }
    private void Update() => animator.Update();
}

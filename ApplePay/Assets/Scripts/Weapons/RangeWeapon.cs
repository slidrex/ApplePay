using UnityEngine;

public class RangeWeapon : WeaponObject
{
    [SerializeField] protected Transform FirePoint;
    public Vector2 GetFirePointPos()
    {
        if(FirePoint != null) return FirePoint.position;
        return (Vector2)transform.position;
    }
}

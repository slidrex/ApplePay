using UnityEngine;

public class BatBehaviour : AttackingMob
{
    [Header("Ban settings")]
    private SimplifiedWeaponHolder weaponHolder;
    [SerializeField] private float seenRadius;
    private const int bulletLayer = 13;
    protected override void Start()
    {
        base.Start();
        weaponHolder = GetComponent<SimplifiedWeaponHolder>();
    }
    protected override void Update()
    {
        base.Update();
        Collider2D[] obj = Physics2D.OverlapCircleAll(transform.position, seenRadius);
        foreach(Collider2D objec in obj)
        {
            Creature currentEntity = objec.GetComponent<Creature>();
            if(currentEntity != null && currentEntity != this && PayTagHandler.IsHostile(this, currentEntity))
            {
                if(weaponHolder.Activate(this, ref weaponHolder.ActiveWeapon, objec.gameObject.transform.position, objec.transform, out GameObject[] output))
                {
                    foreach(var _obj in output)
                    {
                        Projectile projectile = _obj.GetComponent<Projectile>();
                        if(projectile != null)
                        {
                            projectile.DisableOwnerCollisions();
                            projectile.gameObject.layer = bulletLayer;
                        }

                    }
                }
            }
        }
    }
}
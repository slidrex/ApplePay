using UnityEngine;

public class BatBehaviour : AttackingMob
{
    [Header("Ban settings")]
    private SimplifiedWeaponHolder weaponHolder;
    [SerializeField] private float seenRadius;
    protected override void Start()
    {
        base.Start();
        weaponHolder = GetComponent<SimplifiedWeaponHolder>();
        Hostiles.Add(GameObject.FindGameObjectWithTag("Player").GetComponent<Creature>());
    }
    protected override void Update()
    {
        base.Update();
        Collider2D[] obj = Physics2D.OverlapCircleAll(transform.position, seenRadius);
        foreach(Collider2D objec in obj)
        {
            if(Hostiles.Contains(objec.GetComponent<Creature>()))
            {
                weaponHolder.Activate(this, ref weaponHolder.ActiveWeapon, objec.gameObject.transform.position, objec.transform, out Projectile projectile);
            }
        }
    }
}
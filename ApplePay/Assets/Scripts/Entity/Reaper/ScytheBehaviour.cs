using System.Collections.Generic;
using UnityEngine;

public class ScytheBehaviour : AttackingMob
{
    private Rigidbody2D rb;
    [Header("Scythe Behaviour")]
    [SerializeField] private float speed;
    [SerializeField] private float obstacleHitDist;
    private Vector2 moveVector;
    private Vector2 baseVelocity, additionalVelocity;
    [Header("Physics Settings")]
    [SerializeField] private float collisionForce;
    [SerializeField] private float forceResistance;
    private const int projectileLayer = 12;
    private List<Vector2> forces = new List<Vector2>();
    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        AimSelect();
    }
    private void FixedUpdate()
    {
        baseVelocity = moveVector * speed;
        additionalVelocity = Vector2.zero;
        for(int i = 0; i < forces.Count; i++)
        {
            additionalVelocity += forces[i];
            forces[i] = Vector2.MoveTowards(forces[i], Vector2.zero, forceResistance * Time.fixedDeltaTime);   
        }
        rb.velocity = baseVelocity + additionalVelocity;
    }
    protected override void Update()
    {
        if(forces.Contains(Vector2.zero))
            forces.RemoveAll(s => s == Vector2.zero);

        RaycastHit2D[] obstcs = Physics2D.RaycastAll(transform.position, moveVector, obstacleHitDist);
        
        foreach(RaycastHit2D ray in obstcs)
        {
            if(ray.collider.gameObject != gameObject && ray.collider.isTrigger == false && ray.collider.gameObject != Target.gameObject) AimSelect();
            if(Target.gameObject != null && ray.collider.gameObject == Target.gameObject)
            {
                if(ray.collider.gameObject.GetComponent<Creature>().Immortal) Physics2D.IgnoreCollision(ray.collider, GetComponent<Collider2D>(), true);
                else Physics2D.IgnoreCollision(ray.collider, GetComponent<Collider2D>(), false);
                Target.GetComponent<Creature>().Damage(AttackDamage, DamageType.Magical, this);
            }
        }
    }
    private void AimSelect()
    {
        Vector2 dist = Target.transform.position - transform.position;
        moveVector = new Vector2(dist.normalized.x, dist.normalized.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == projectileLayer)
        {
            Vector2 DragForce = collision.gameObject.GetComponent<Projectile>().MoveVector * collisionForce * collision.gameObject.GetComponent<Rigidbody2D>().mass;
            forces.Add(DragForce);
        }
    }
}
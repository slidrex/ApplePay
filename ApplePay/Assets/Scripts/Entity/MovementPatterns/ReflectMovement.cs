using UnityEngine;
using System.Collections;
public class ReflectMovement : MovementPattern
{
    [SerializeField] private GameObject collisionEffect;
    [SerializeField] private float collisionEffectDuration;
    [SerializeField] private int collisionDamage;
    private Vector2 lastVelocity;
    private Rigidbody2D rb;
    public Collision2D lastCollision {get; private set;}
    public override void OnSpeedUpdate()
    {
        if(MovementVector == Vector2.zero) MovementVector = Pay.Functions.Math.GetRandomFixedVector();
        lastVelocity = Movement.Rigidbody.velocity;
        UpdateRigidbodyVector();
    }
    public override void OnCollision(Collision2D collision)
    {
        if(collision.collider.GetComponent<Creature>() != null)
        {
            lastCollision = collision;
            collision.collider.GetComponent<Creature>().Damage(collisionDamage, DamageType.Physical, GetComponent<Creature>());
            CollsionsManipulation(GetComponent<Collider2D>(), lastCollision.gameObject.GetComponent<Collider2D>(), true, 0.5f);
        }
        var contact = collision.contacts[0].normal;
        var inNormal = ((Vector2)transform.position - contact).normalized;
        MovementVector = Vector2.Reflect(lastVelocity, inNormal).normalized;
        PayWorld.Particles.InstantiateParticles(collisionEffect, contact, Quaternion.identity, collisionEffectDuration);
    }
    private IEnumerator CollsionManipulation(Collider2D col1, Collider2D col2, bool ignore, float timeToEnable)
    {
        if(ignore == true)
        {
            Physics2D.IgnoreCollision(col1, col2, true);
            yield return new WaitForSeconds(timeToEnable);
            Physics2D.IgnoreCollision(col1, col2, false);
        }
        else Physics2D.IgnoreCollision(col1, col2, false);
    }
    public void CollsionsManipulation(Collider2D col1, Collider2D col2, bool ignore, float timeToEnable) =>
        StartCoroutine(CollsionManipulation(col1, col2, ignore, timeToEnable));
}

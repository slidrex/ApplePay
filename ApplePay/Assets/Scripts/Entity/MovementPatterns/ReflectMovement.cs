using UnityEngine;

public class ReflectMovement : MovementPattern
{
    [SerializeField] private GameObject collisionEffect;
    [SerializeField] private float collisionEffectDuration;
    private Vector2 lastVelocity;
    public override void OnSpeedUpdate()
    {
        if(MovementVector == Vector2.zero) MovementVector = Pay.Functions.Math.GetRandomFixedVector();
        lastVelocity = Movement.Rigidbody.velocity;
        UpdateRigidbodyVector();
    }
    public override void OnCollision(Collision2D collision)
    {
        foreach(ContactPoint2D contact in collision.contacts)
        {
            HandleCollision(contact.normal);
        }
    }
    public override void OnHitDetected(HitInfo hitInfo)
    {
        Vector2 contact = hitInfo.normal;
        HandleCollision(contact);
    }
    private void HandleCollision(Vector2 contactNormal)
    {
        Vector2 inNormal = ((Vector2)transform.position - contactNormal).normalized;
        MovementVector = Vector2.Reflect(lastVelocity, inNormal).normalized;
        UpdateRigidbodyVector();
        PayWorld.Particles.InstantiateParticles(collisionEffect, contactNormal, Quaternion.identity, collisionEffectDuration);
    }
}

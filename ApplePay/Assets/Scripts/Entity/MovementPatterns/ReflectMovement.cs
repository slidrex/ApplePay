using UnityEngine;

public class ReflectMovement : MovementPattern
{
    [SerializeField] private GameObject collisionEffect;
    [SerializeField] private float collisionEffectDuration;
    protected override float CollisionTimeTreshold => 0.25f;
    private Vector2 lastVelocity;
    public override void OnSpeedUpdate()
    {
        if(MovementVector == Vector2.zero) MovementVector = Pay.Functions.Math.GetRandomFixedVector();
        lastVelocity = Movement.Rigidbody.velocity;
        UpdateRigidbodyVector();
    }
    protected override void OnCollisionBegin(Collision2D collision)
    {
        Vector2 inNormal = collision.contacts[0].normal;
        MovementVector = Vector2.Reflect(MovementVector, inNormal);
        PayWorld.Particles.InstantiateParticles(collisionEffect, CurrentTransform.position - (Vector3)inNormal/2, Quaternion.identity, collisionEffectDuration);
    }
    protected override void OnCollisionTimeOut()
    {
        MovementVector = Pay.Functions.Math.GetRandomFixedVector();
    }
}

using UnityEngine;
public class ReflectMovement : MovePatterns
{
    [SerializeField] private GameObject collisionEffect;
    [SerializeField] private float collisionEffectDuration;
    public override void OnStart() => MovementVector = Pay.Functions.Math.GetRandomFixedVector();
    public override void OnSpeedUpdate() => UpdateRigidbodyVector();
    public override void OnCollision(Collision2D collision)
    {
        MovementVector = Vector2.Reflect(MovementVector, collision.contacts[0].normal);
        PayWorld.Particles.InstantiateParticles(collisionEffect, collision.contacts[0].point, Quaternion.identity, collisionEffectDuration);
    }
}

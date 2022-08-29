using UnityEngine;

public class Missile : Projectile
{
    [Header("Tracking Settings")]
    public Transform TrackingObject;
    [SerializeField] private float AngularVelocity;
    protected override void Start()
    {
        base.Start();
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, Pay.Functions.Math.Atan3(GetDistance().y, GetDistance().x));
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        float endRotation = Pay.Functions.Math.Atan3(GetDistance().y, GetDistance().x);

        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, Mathf.MoveTowardsAngle(transform.eulerAngles.z, endRotation, Time.fixedDeltaTime * AngularVelocity));

        rb.velocity = transform.up * Speed;
    }
    private Vector2 GetDistance() => TrackingObject.position - transform.position;
}
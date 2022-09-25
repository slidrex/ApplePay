using UnityEngine;

public abstract class MovePatterns : MonoBehaviour
{
    protected Transform CurrentTransform;
    protected MobMovement Movement;
    protected Transform Target;
    protected Vector2 TargetDistance;
    public Vector2 MovementVector {get => Movement.MoveVector; protected set => Movement.MoveVector = value; }
    public void Init(MobMovement movement, Transform current, Transform target)
    {
        Movement = movement;
        Target = target;
        CurrentTransform = current;
        OnStart();
    }
    private void Update()
    {
        if(Target == null) return;
        UpdateTargetDistance();
        UpdateMovementAnimator();
    }
    protected void UpdateTargetDistance() => TargetDistance = Target.transform.position - CurrentTransform.position;
    protected void UpdateMovementAnimator()
    {
        Movement.animator.SetInteger("Vertical", (int)TargetDistance.y);
        Movement.animator.SetInteger("Horizontal", (int)TargetDistance.x);
    }
    protected void UpdateRigidbodyVector() => SetRigidbodyVelocity(MovementVector * Movement.CurrentSpeed);
    protected void SetRigidbodyVelocity(Vector2 velocity) => Movement.Rigidbody.velocity = velocity;
    public virtual void OnUpdate() { }
    public virtual void OnStart() { }
    public virtual void OnSpeedUpdate() { }
    public virtual void OnCollision(Collision2D collision) { }
}
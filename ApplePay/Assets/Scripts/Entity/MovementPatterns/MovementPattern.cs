using UnityEngine;

public abstract class MovementPattern : MonoBehaviour
{
    public Transform Target => Movement.Target;
    protected Transform CurrentTransform;
    protected MobMovement Movement;
    protected Vector2 TargetDistance => Target.transform.position - CurrentTransform.position;
    public Vector2 MovementVector {get => Movement.MoveVector; protected set => Movement.MoveVector = value; }
    public void Init(MobMovement movement, Transform current)
    {
        Movement = movement;
        CurrentTransform = current;
        OnStart();
    }
    private void Update()
    {
        if(Target == null) return;
        UpdateMovementAnimator();
    }
    protected virtual void UpdateMovementAnimator()
    {
        Movement.animator.SetInteger("Vertical", (int)TargetDistance.y);
        if(transform.position.x < Target.position.x)
        {
            transform.eulerAngles = new Vector2(0, 0);
            Movement.animator.SetInteger("Horizontal", (int)TargetDistance.x);
        }
        else
        {
            transform.eulerAngles = new Vector2(0, 180);
            Movement.animator.SetInteger("Horizontal", (int)TargetDistance.x);
        }
    }
    protected void UpdateRigidbodyVector() => SetRigidbodyVelocity(MovementVector * Movement.CurrentSpeed);
    protected void SetRigidbodyVelocity(Vector2 velocity) => Movement.Rigidbody.velocity = velocity;
    public virtual void OnUpdate() { }
    public virtual void OnStart() { }
    public virtual void OnSpeedUpdate() { }
    public virtual void OnCollision(Collision2D collision) { }
}
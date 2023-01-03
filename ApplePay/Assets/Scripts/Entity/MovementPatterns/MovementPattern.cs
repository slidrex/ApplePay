using UnityEngine;

public abstract class MovementPattern : MonoBehaviour, IHitResponder
{
    public Transform Target => Movement.Target;
    protected Transform CurrentTransform;
    protected MobMovement Movement;
    protected Vector2 TargetDistance => Target.transform.position - CurrentTransform.position;
    public Vector2 MovementVector {get => Movement.MoveVector; protected set => Movement.MoveVector = value; }
    protected virtual float CollisionTimeTreshold { get; }
    private float currentCollisionTime;
    public void Init(MobMovement movement, Transform transform)
    {
        Movement = movement;
        OnStart();
        CurrentTransform = transform;
        movement.Entity.HitShape.AddResponder(this);
    }
    public void Init(MobMovement movement)
    {
        Movement = movement;
        CurrentTransform = movement.Entity.transform;
    }
    private void Update()
    {
        if(Target == null) return;
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
    public void SetRigidbodyVelocity(Vector2 velocity) => Movement.Rigidbody.velocity = velocity;
    public virtual void OnUpdate() {UpdateMovementAnimator(); }
    public virtual void OnStart() { }
    public virtual void OnSpeedUpdate() { }
    public void CollisionBegin(Collision2D collision)
    {
        OnCollisionBegin(collision);
    }
    public void CollisionState(Collision2D collision)
    {
        if(CollisionTimeTreshold > 0.0f)
        {
            if(currentCollisionTime <= CollisionTimeTreshold) currentCollisionTime += Time.deltaTime;
            else OnCollisionTimeOut();
        }
        currentCollisionTime += Time.deltaTime;
        OnCollisionState(collision);
    }
    public void CollisionEnd(Collision2D collision)
    {
        currentCollisionTime = 0.0f;
        OnCollisionEnd(collision);
    }
    protected virtual void OnCollisionTimeOut() { }
    protected virtual void OnCollisionBegin(Collision2D collision) { }
    protected virtual void OnCollisionEnd(Collision2D collision) { }
    protected virtual void OnCollisionState(Collision2D collision) { }
    ///<summary>Calls if entity collides with object.</summary>
    public virtual void OnHitDetected(HitInfo hitInfo) { }
}
using UnityEngine;

public class CrossMovement : MovementPattern
{
    [SerializeField] private float directionChangeFrequency;
    private float currentDirectionChangeFrequency;
    public override void OnUpdate()
    {
        base.OnUpdate();
        if(directionChangeFrequency > currentDirectionChangeFrequency)
        {
            currentDirectionChangeFrequency += Time.deltaTime;
        }
        else 
        {
            MovementVector = Mathf.Abs(TargetDistance.x) > Mathf.Abs(TargetDistance.y) ? Vector2.right * Mathf.Sign(TargetDistance.x) : Vector2.up * Mathf.Sign(TargetDistance.y);
            currentDirectionChangeFrequency = 0;
        }
    }
    protected override void UpdateMovementAnimator()
    {
        Movement.animator.SetInteger("Vertical", (int)MovementVector.y);
        if(CurrentTransform.position.x < Target.position.x)
        {
            CurrentTransform.eulerAngles = new Vector2(0, 0);
            Movement.animator.SetInteger("Horizontal", (int)MovementVector.x);
        }
        else
        {
            CurrentTransform.eulerAngles = new Vector2(0, 180);
            Movement.animator.SetInteger("Horizontal", (int)MovementVector.x);
        }
    }
    public override void OnSpeedUpdate() => UpdateRigidbodyVector();
}
using UnityEngine;

public class PointMovement : MovePatterns
{
    protected Vector2 MovePoint;
    public override void OnUpdate() => MoveTowardsPoint();
    protected virtual void MoveTowardsPoint()
    {
        CurrentTransform.position = Vector2.MoveTowards(CurrentTransform.position, MovePoint, Mathf.Abs(Movement.CurrentSpeed) * Time.deltaTime);
        if((Vector2)CurrentTransform.position == MovePoint)
            OnPointReached();
    }
    protected virtual void OnPointReached() {}
}

using UnityEngine;

public class PointMovement : MovementPattern
{
    private Vector2 MovePoint;
    protected Vector2 GetMovementPoint() => MovePoint;
    protected void SetMovementPoint(Vector2 point) => MovePoint = point;
    protected virtual void OnPointReached() {}
    public override void OnSpeedUpdate()
    {
        MovementVector = (MovePoint - (Vector2)CurrentTransform.position).normalized;
        if(((Vector2)CurrentTransform.position - MovePoint).SqrMagnitude() < 1)
            OnPointReached();
        UpdateRigidbodyVector();
    }
}

using UnityEngine;

public class RandomPointMovement : PointMovement
{
    public override void OnStart() => SetMovementPoint(GetRandomPoint());
    protected override void OnPointReached() => SetMovementPoint(GetRandomPoint());
    private Vector2 GetRandomPoint()
    {
        if(Movement.Entity.CurrentRoom != null)
            return Movement.Entity.CurrentRoom.GetRandomFreeRoomSpace();
        return CurrentTransform.position;
    }
}

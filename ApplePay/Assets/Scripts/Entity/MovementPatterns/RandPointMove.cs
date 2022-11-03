using UnityEngine;

public class RandPointMove : PointMovement
{
    public override void OnStart() => MovePoint = GetRandCoord();
    protected override void OnPointReached() => MovePoint = GetRandCoord();
    private Vector2 GetRandCoord()
    {
        if(CurrentTransform.GetComponent<Creature>().CurrentRoom != null)
            return CurrentTransform.GetComponent<Creature>().CurrentRoom.GetRandomFreeRoomSpace();
        return CurrentTransform.position;
    }
}

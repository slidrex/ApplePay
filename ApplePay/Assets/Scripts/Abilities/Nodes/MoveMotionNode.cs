using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Ability Node/Movement Vector Motion Node")]

public class MoveMotionNode : MotionNode
{
    public override void SetupMotionVector(Creature entity, out Vector2 motionVector)
    {
        EntityMovement movement = entity.Movement;
        Vector2 movementVector = movement.GetMovementVector(); 
        if(movementVector != Vector2.zero) motionVector = movementVector;
        else motionVector = movement.GetFacing();
    }
}

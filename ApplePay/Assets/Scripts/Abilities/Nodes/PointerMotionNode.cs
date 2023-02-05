using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Ability Node/Pointer Motion Node")]
public class PointerMotionNode : MotionNode
{
    public override void SetupMotionVector(Creature entity, out Vector2 motionVector)
    {
        motionVector = Pay.Functions.Generic.GetMousePos(Camera.main) - (Vector2)entity.transform.position;
    }
}

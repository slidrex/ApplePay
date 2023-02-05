using UnityEngine;

public abstract class MotionNode : AbilityNode
{
    private Vector2 motionVector;
    private float speed;
    public abstract void SetupMotionVector(Creature entity, out Vector2 motionVector);
    protected override void OnNodeBegin(Creature entity)
    {
        SetupMotionVector(entity, out motionVector);
        speed = GetNodeValue("Speed");
    }
    protected override void OnNodeUpdate(Creature entity)
    {
        entity.transform.position += (Vector3)motionVector * speed * Time.deltaTime;
    }
}

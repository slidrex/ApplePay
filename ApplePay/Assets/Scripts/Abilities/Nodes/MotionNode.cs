using UnityEngine;

public abstract class MotionNode : AbilityNode
{
    [SerializeField] private MotionSourceSpeed speedSource;
    private Vector2 motionVector;
    private float speed;
    public abstract void SetupMotionVector(Creature entity, out Vector2 motionVector);
    protected override void OnNodeBegin(Creature entity)
    {
        SetupMotionVector(entity, out motionVector);
        entity.Movement.SetFacing(entity.Movement.GetClosestFacing(motionVector));
        entity.Movement.AddDisable(NodeTime, true, true);
        EntityAttribute attribute = entity.FindAttribute("movementSpeed");
        float sourceSpeed = speedSource == MotionSourceSpeed.SourceValue ? attribute.GetSourceValue() : attribute.GetAttributeValue();
        speed = sourceSpeed * 2 * (1 + GetNodeValue("SpeedModifier"));
        entity.ForceHandler.BindForce(motionVector * speed, 0.0f, NodeTime);
    }
    private enum MotionSourceSpeed
    {
        ///<summary>Motion speed will be multiplied with source speed value.</summary>
        SourceValue,
        ///<summary>Motion speed will be multiplied with current speed value.</summary>
        CurrentValue
    }
}

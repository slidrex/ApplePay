public class TargetMovement : MovementPattern
{
    public override void OnUpdate() => CurrentTransform.position = UnityEngine.Vector2.MoveTowards(CurrentTransform.position, Target.position, Movement.CurrentSpeed * UnityEngine.Time.deltaTime);
}
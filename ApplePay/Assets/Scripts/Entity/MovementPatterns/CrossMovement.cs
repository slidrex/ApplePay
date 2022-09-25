using UnityEngine;

public class CrossMovement : MovePatterns
{
    [SerializeField] private float directionChangeFrequency;
    private float currentDirectionChangeFrequency;
    public override void OnUpdate()
    {
        if(directionChangeFrequency > currentDirectionChangeFrequency)
        {
            currentDirectionChangeFrequency += Time.deltaTime;
        } else 
        {
            MovementVector = Mathf.Abs(TargetDistance.x) > Mathf.Abs(TargetDistance.y) ? Vector2.right * Mathf.Sign(TargetDistance.x) : Vector2.up * Mathf.Sign(TargetDistance.y);
            currentDirectionChangeFrequency = 0;
        }
    }
    public override void OnSpeedUpdate() => UpdateRigidbodyVector();
}
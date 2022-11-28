using UnityEngine;

public class HitInfo
{
    public Vector2 normal;
    public Collider2D collider;
    public Entity entity;
}
public interface IHitResponder
{
    public void OnHitDetected(HitInfo hitInfo);
}
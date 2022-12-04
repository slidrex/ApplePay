using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class PayHitCircle : PayHitShape
{
    public CircleCollider2D Collider;
    public override Collider2D M_Collider => Collider;
    public override void CheckHit()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll((Vector2)transform.position + Collider.offset, Collider.radius, Vector2.zero);
        HandleHit(hits, Collider);
    }
}

using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class PayHitCircle : PayHitShape
{
    public CircleCollider2D Collider;
    public override Collider2D M_Collider => Collider;
    public override void CheckHit()
    {
        Transform tr = transform;
        float multiplier = Mathf.Max(Mathf.Abs(tr.localScale.x), Mathf.Abs(tr.localScale.y));
        RaycastHit2D[] hits = Physics2D.CircleCastAll(tr.position + (Vector3)(Collider.offset * tr.localScale), Collider.radius * multiplier, Vector2.zero);
        HandleHit(hits, Collider);
    }
}

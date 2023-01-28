using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class PayHitBox : PayHitShape
{
    public BoxCollider2D Collider;
    public override Collider2D M_Collider => Collider;
    public override void CheckHit()
    {
        Transform tr = transform;
        Vector3 localScale = tr.localScale;
        RaycastHit2D[] hits = Physics2D.BoxCastAll((Vector2)tr.position + Collider.offset * localScale, Collider.size * localScale, tr.eulerAngles.z, Vector2.zero);
        HandleHit(hits, Collider);
    }
}

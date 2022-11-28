using UnityEngine;

public class PayHitBox : PayHitShape
{
    public BoxCollider2D Collider;
    public override Collider2D M_Collider => Collider;
    public override void CheckHit()
    {
        RaycastHit2D[] hits = Physics2D.BoxCastAll((Vector2)transform.position + Collider.offset, Collider.size, transform.eulerAngles.z, Vector2.zero);
        HandleHit(hits, Collider);
    }
}

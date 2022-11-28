using UnityEngine;

public abstract class PayHitShape : MonoBehaviour
{
    public Entity Owner;
    public abstract Collider2D M_Collider {get;}
    public IHitResponder Responder;
    public abstract void CheckHit();
    public void HandleHit(RaycastHit2D[] hits, Collider2D collider)
    {
        foreach(RaycastHit2D hit in hits)
        {
            if(hit.collider != collider)
                if(hit.collider.GetComponent<PayHitShape>() != null)
                {
                    PayHitShape hitBox = hit.collider.GetComponent<PayHitShape>();
                    
                    if(hitBox.Owner != Owner && hitBox.Owner != null)
                    {
                        HitInfo hitInfo = new HitInfo();
                        hitInfo.entity = hitBox.Owner;
                        hitInfo.collider = hitBox.M_Collider;
                        hitInfo.normal = hit.normal;
                        Responder?.OnHitDetected(hitInfo);
                    }
                }
        }
    }
}

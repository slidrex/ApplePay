using UnityEngine;
using System.Linq;

public abstract class PayHitShape : MonoBehaviour
{
    public System.Collections.Generic.Dictionary<Collider2D, float> IgnoreShapes = new System.Collections.Generic.Dictionary<Collider2D, float>();
    public Entity Owner;
    public abstract Collider2D M_Collider {get;}
    private IHitResponder responder;
    ///<summary>Sets the object that will be triggered if hit is detected. </summary>
    public void SetResponder(IHitResponder responder) => this.responder = responder;
    public abstract void CheckHit();
    public void HandleHit(RaycastHit2D[] hits, Collider2D collider)
    {
        foreach(RaycastHit2D hit in hits)
        {
            if(hit.collider != collider)
                if(hit.collider.GetComponent<PayHitShape>() != null && IgnoreShapes.ContainsKey(hit.collider) == false)
                {
                    PayHitShape hitBox = hit.collider.gameObject.GetComponent<PayHitShape>();
                    
                    if(hitBox.Owner != null && hitBox.Owner != Owner && hitBox.M_Collider == hit.collider)
                    {
                        HitInfo hitInfo = new HitInfo();
                        hitInfo.entity = hitBox.Owner;
                        hitInfo.collider = hitBox.M_Collider;
                        hitInfo.normal = hit.normal;
                        responder?.OnHitDetected(hitInfo);
                    }
                }
        }
    }
    private void Update()
    {
        for(int i = 0; i < IgnoreShapes.Count; i++)
        {
            if(IgnoreShapes.ElementAt(i).Value > 0)
            {
                IgnoreShapes[IgnoreShapes.ElementAt(i).Key] -= Time.deltaTime;
            }
            else
            {
                IgnoreShapes.Remove(IgnoreShapes.ElementAt(i).Key);
            }
        }
    }
}

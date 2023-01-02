using UnityEngine;

public abstract class PayHitShape : MonoBehaviour
{
    private System.Collections.Generic.List<IgnoreShapeObject> IgnoreShapes = new System.Collections.Generic.List<IgnoreShapeObject>();
    public Entity Owner;
    public Collider2D[] collisionColliders;
    public abstract Collider2D M_Collider {get;}
    private System.Collections.Generic.List<IHitResponder> responders = new System.Collections.Generic.List<IHitResponder>();
    ///<summary>Sets the object that will be triggered if hit is detected. </summary>
    public void AddResponder(IHitResponder responder) => responders.Add(responder);
    public void RemoveResponder(IHitResponder responder) => responders.Remove(responder);
    public abstract void CheckHit();
    public void HandleHit(RaycastHit2D[] hits, Collider2D collider)
    {
        foreach(RaycastHit2D hit in hits)
        {
            if(hit.collider != collider)
                if(hit.collider.GetComponent<PayHitShape>() != null && IsIgnore(hit.collider) == false)
                {
                    PayHitShape hitBox = hit.collider.gameObject.GetComponent<PayHitShape>();
                    
                    if(hitBox.Owner != null && hitBox.Owner != Owner && hitBox.M_Collider == hit.collider)
                    {
                        HitInfo hitInfo = new HitInfo();
                        hitInfo.entity = hitBox.Owner;
                        hitInfo.collider = hitBox.M_Collider;
                        hitInfo.normal = hit.normal;
                        for(int i = 0; i < responders.Count; i++)
                        {
                            responders[i].OnHitDetected(hitInfo);
                        }
                    }
            }
        }
        for(int i = 0; i < IgnoreShapes.Count; i++)
        {
            if(IgnoreShapes[i].shape.M_Collider == null) IgnoreShapes.RemoveAt(i);
        }
    }
    public bool IsIgnore(Collider2D collider)
    {
        foreach(IgnoreShapeObject shapeObject in IgnoreShapes)
        {
            if(shapeObject.shape.M_Collider == collider) return true;
            foreach(Collider2D _collider in shapeObject.shape.collisionColliders)
            {
                if(collider == _collider) return true;
            }
        }
        return false;
    }
    public bool IsIgnore(PayHitShape shape)
    {
        foreach(IgnoreShapeObject shapeObject in IgnoreShapes)
        {
            if(shapeObject.shape == shape) return true;
        }
        return false;
    }
    private void Update()
    {
        for(int i = 0; i < IgnoreShapes.Count; i++)
        {
            IgnoreShapeObject current = IgnoreShapes[i];
            if(current.time > 0)
            {
                current.time -= Time.deltaTime;
                IgnoreShapes[i] = current;
            }
            else if(current.endless == false)
            {
                IgnoreShapes.RemoveAt(i);
            }
        }
    }
    ///<summary>
    ///Ignores all shapes collisions and interactions.
    ///</summary>
    public void IgnoreShape(PayHitShape shape)
    {
        IgnoreShapeObject ignoreObject = new IgnoreShapeObject(shape, 0.0f, true);
        IgnoreShapes.Add(ignoreObject);
        Physics2D.IgnoreCollision(shape.M_Collider, M_Collider);
        foreach(Collider2D collisionCollider in collisionColliders) Physics2D.IgnoreCollision(shape.M_Collider, collisionCollider);
        foreach(Collider2D secondCollider in shape.collisionColliders)
        {
            foreach(Collider2D collider in collisionColliders) Physics2D.IgnoreCollision(secondCollider, collider);
            Physics2D.IgnoreCollision(secondCollider, M_Collider);
        }
        shape.IgnoreShapes.Add(ignoreObject);
    }
    ///<summary>
    ///Ignores all shapes collisions and interactions.
    ///</summary>
    public void IgnoreShape(PayHitShape shape, float time)
    {
        print("ignore");
        IgnoreShapeObject ignoreObject = new IgnoreShapeObject(shape, time, false);
        IgnoreShapes.Add(ignoreObject);
        Pay.Functions.Physics.IgnoreCollision(time, shape.M_Collider, M_Collider);
        foreach(Collider2D collisionCollider in collisionColliders) Pay.Functions.Physics.IgnoreCollision(time, shape.M_Collider, collisionCollider);
        foreach(Collider2D secondCollider in shape.collisionColliders)
        {
            foreach(Collider2D collider in collisionColliders) Pay.Functions.Physics.IgnoreCollision(time, secondCollider, collider);
            Pay.Functions.Physics.IgnoreCollision(time, secondCollider, M_Collider);
        }
        shape.IgnoreShapes.Add(ignoreObject);
    }
    public struct IgnoreShapeObject
    {
        public PayHitShape shape;
        public float time;
        public bool endless;
        public IgnoreShapeObject(PayHitShape shape, float time, bool endless)
        {
            this.shape = shape;
            this.time = time;
            this.endless = endless;
        }
    }
}

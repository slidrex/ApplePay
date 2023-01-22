using UnityEngine;

[UnityEngine.RequireComponent(typeof(PayForceHandler))]
public abstract class CollectableMillestone : MonoBehaviour, IHitResponder
{
    public GameObject CollectEffect;
    public TrackAnim TrackAnim;
    public PayForceHandler ForceHandler;
    public PayHitShape HitShape;
    protected virtual void Awake() => HitShape.AddResponder(this);
    public void OnHitDetected(HitInfo hitInfo)
    {
        bool requestStatus = CollectRequest(hitInfo);
        if(requestStatus) OnCollect(hitInfo);
        else OnCollectFail(hitInfo);
    }
    protected virtual void Update() => HitShape.CheckHit();
    protected virtual bool CollectRequest(HitInfo hitInfo) => true;
    protected virtual void OnCollect(HitInfo hitInfo) 
    {
        PayWorld.Particles.InstantiateParticles(CollectEffect, transform.position, Quaternion.identity, 5.0f);
        Destroy(gameObject);
    }
    protected virtual void OnCollectFail(HitInfo hitInfo) {}
}

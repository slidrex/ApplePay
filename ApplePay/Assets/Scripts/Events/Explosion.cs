using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private AnimationClip startAnimation;
    [SerializeField] private Transform explosionPoint;
    [Header("Explosion Settings")]
    public float Radius;
    [SerializeField] private float delay;
    private float passedTime;
    protected virtual void Start() => Invoke("ExecuteExplosion", delay);
    protected virtual void ExecuteExplosion()
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(explosionPoint.position, Radius);
        foreach(Collider2D current in cols)
        {
            OnInsideExplosion(current);
        }
        Invoke("DestroyExplosion", startAnimation.length);
    }
    public void DestroyExplosion() => Destroy(gameObject);
    protected virtual void OnInsideExplosion(Collider2D collision) { }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(explosionPoint.position, Radius);
    }
}
using UnityEngine;
public class HealFountain : MonoBehaviour
{
    [SerializeField] private GameObject healAura;
    [SerializeField] private float radius;
    private const float effectDuration = 5;
    private float curEffectTime;
    private Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        EffectUpdater();
    }
    private void EffectUpdater()
    {
        if(curEffectTime < effectDuration)
        {
            curEffectTime += Time.deltaTime;
            if(curEffectTime > effectDuration)
            {
                animator.SetTrigger("Glow");
                PayWorld.Particles.InstantiateParticles(healAura, new Vector2(transform.position.x, transform.position.y - 0.8f), Quaternion.identity, 1.85f, transform);
                curEffectTime = 0;
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);
                foreach (Collider2D collider in colliders)
                {
                    if(collider.GetComponent<Creature>() != null)
                    {
                        PayWorld.EffectController.AddEffect(collider.GetComponent<Creature>(), "instant_heal", 2, 0);
                    }
                }
            }
        }
    }
    private void OnDrawGizmosSelected() => Gizmos.DrawWireSphere(transform.position, radius);
}
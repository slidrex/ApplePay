using UnityEngine;

public class WoodenBox : Entity
{
    [SerializeField] protected CharmDatabase charmDatabase;
    [SerializeField] private float droppedItemVelocity;
    private Animator animator;
    protected override void Start()
    {
        animator = GetComponent<Animator>();
        base.Start();
    }
    protected override void ApplyDamage(Creature handler)
    {
        animator.SetTrigger("TakeDamage");    
        base.ApplyDamage(handler);
    }
    protected override void Die(Creature killer)
    {
        int rand = Random.Range(0, charmDatabase.GetLength() - 1);
        var item = Instantiate(charmDatabase.GetItem(rand), transform.position, Quaternion.identity);
        item.GetComponent<Rigidbody2D>().velocity = droppedItemVelocity * Pay.Functions.Math.GetRandomFixedVector();
        base.Die(killer);
    }
}

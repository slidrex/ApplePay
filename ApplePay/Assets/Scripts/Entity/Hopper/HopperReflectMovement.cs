using UnityEngine;
/*
public class HopperReflectMovement : ReflectMovement
{
    private Hopper time;
    private Hopper.States states;
    [SerializeField] private GameObject collisionEffect;
    private int collisionDamage = 15;
    protected override void Start()
    {
        base.Start();
        time = GetComponent<Hopper>();
    }
    protected override void Update()
    {
        base.Update();
        if(time.curTime > time.maxTime)
            Direction = (GameObject.FindGameObjectWithTag("Player").transform.position - transform.position).normalized;
        if(states == Hopper.States.Rush) AdditionalSpeed = 8;
        else if(states == Hopper.States.Shooting) AdditionalSpeed = 6;
    }
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<Creature>() != null)
        {
            collision.gameObject.GetComponent<Creature>().ChangeHealth(-collisionDamage);
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.gameObject.GetComponent<Collider2D>(), true);
            Invoke("EnableCollision", 1f);
        }
        if(collision.gameObject.layer == 9)
        {
            base.OnCollisionEnter2D(collision);
            AdditionalSpeed += Time.deltaTime * 1.5f;
            var obj = Instantiate(collisionEffect, collision.contacts[0].point, Quaternion.identity);
            Destroy(obj, 0.4f);
        }
    }
    private void EnableCollision() => Physics2D.IgnoreCollision(GetComponent<Collider2D>(), FindObjectOfType<PlayerEntity>().gameObject.GetComponent<Collider2D>(), false);
}
*/
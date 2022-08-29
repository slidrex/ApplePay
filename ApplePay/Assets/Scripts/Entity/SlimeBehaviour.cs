using UnityEngine;

public class SlimeBehaviour : AttackingMob
{
    [SerializeField] private GameObject explosion;
    private Rigidbody2D rb;
    private Animator anim;
    private Vector3 dist;
    public bool isJumping;
    protected override void Start()
    {
        base.Start();
        SetTarget(FindObjectOfType<PlayerEntity>());
        RotateEulerAngles();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    private void FixedUpdate()
    {
        if(isJumping)
            rb.velocity = Pay.Functions.Math.ClampVectorComponents(dist, 40, 40) * dist.normalized;
    }
    private void RotateEulerAngles()
    {
        dist = Target.transform.position - transform.position;
        if(dist.x < 0) transform.eulerAngles = new Vector2(0, 180);
        else transform.eulerAngles = new Vector2(0, 0);
    }
    private Vector2 CalcDist() => dist = Target.transform.position - transform.position;
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(isJumping)
        {
            PayWorld.Particles.InstantiateParticles(explosion, new Vector2(transform.position.x, transform.position.y + 0.3f), Quaternion.identity, 1.5f);
            Destroy(gameObject);
        }
    }
}

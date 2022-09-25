using UnityEngine;
public class HunterSpearBehaviour : Projectile
{
    private bool returnToOwner = false;
    private bool canCollison = true;
    private Vector2 dist;
    private const int obstacleLayer = 6;
    protected override void Start()
    {
        base.Start();
    }
    protected override void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.layer == obstacleLayer && canCollison == true)
        {
            Animator.SetTrigger("Collision");
            Pay.Camera.CameraShake.FlatShake(0, 4.5f, 0.05f);
            Invoke("Extract", Random.Range(0.3f, 0.5f));
            SetMoveVector(Vector2.zero);
        }
        if(other.gameObject.layer != obstacleLayer && other.gameObject.GetComponent<Entity>() == null)
        {
            Extract();
        }
        if(other.gameObject.GetComponent<Entity>() != null)
        {
            other.gameObject.GetComponent<Entity>().Damage(Damage, DamageType.Physical, ProjectileOwner);
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), other.gameObject.GetComponent<Collider2D>(), true);
            Extract();
        }
    }
    protected override void Update()
    {
        base.Update();
        dist = ProjectileOwner.transform.position - transform.position;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, Pay.Functions.Math.Atan3(dist.y, dist.x));
        ReturnToOwner();
    }
    private void ReturnToOwner()
    {
        if(returnToOwner)
        {
            dist = ProjectileOwner.transform.position - transform.position;
            SetMoveVector(dist);
            Animator.SetTrigger("ReturnToOwner");
            if(Vector2.Distance(transform.position, ProjectileOwner.transform.position) <= 0.5f)
            {
                ProjectileOwner.GetComponent<Animator>().SetBool("TakeSpear", true);
                Destroy(gameObject, 0.15f);
            }
        }
    }
    private void Extract()
    {
        canCollison = false;
        returnToOwner = true;
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        ProjectileOwner.GetComponent<Animator>().SetBool("TakeSpear", false);
    }
}
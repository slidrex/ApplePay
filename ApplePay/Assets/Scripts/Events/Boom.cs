using UnityEngine;

public class Boom : Explosion
{
    [SerializeField] private int damage;
    protected override void OnInsideExplosion(Collider2D collision)
    {
        if(collision.GetComponent<Entity>() != null)
            collision.GetComponent<Entity>().ChangeHealth(-damage);
    }
}

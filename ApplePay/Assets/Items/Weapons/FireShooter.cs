using System.Collections;
using UnityEngine;

public class FireShooter : RangeWeapon
{
    [SerializeField] private float spread;
    protected override GameObject[] OnActivate(Creature attacker, Vector2 originPosition, Vector2 attackPosition, Transform target)
    {
        StartCoroutine(Shoot(attacker, originPosition, attackPosition, target, Projectile));
        return new GameObject[1] { Projectile.gameObject };
    }
    private IEnumerator Shoot(Creature attacker, Vector2 originPosition, Vector2 attackPosition, Transform target, Projectile projectile)
    {
        for(float i = 0; i < 0.7f; i+= Time.deltaTime)
        {
            Vector2 distance = attackPosition - originPosition;
            float angel = Pay.Functions.Math.Atan3(distance.y, distance.x);
            projectile = Instantiate(Projectile, GetFirePointPos(), Quaternion.identity);
            Vector3 rand = projectile.transform.eulerAngles;
            rand.z = angel + Random.Range(-spread, spread);
            projectile.transform.eulerAngles = rand;
            Vector2 moveVector = projectile.transform.up;
            projectile.Setup(moveVector, attacker, target);
            projectile.DisableOwnerCollisions();
            yield return new WaitForSeconds(0.1f);
        }

    }
}

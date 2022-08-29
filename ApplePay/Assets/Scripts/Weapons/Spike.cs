using UnityEngine;

public class Spike : Projectile
{
    [SerializeField] private Projectile pieceOfSpike;
    protected override void OnDestroy()
    {
        for (int i = 0; i < 4; i++)
        {
            Projectile obj = Instantiate(pieceOfSpike.gameObject, transform.position, Quaternion.identity).GetComponent<Projectile>();
            obj.Setup(GetVector(i), null, null);
        }
        base.OnDestroy();
    }
    private Vector2 GetVector(int a)
    {
        switch(a)
        {
            case 0 : return Vector2.up;
            case 1 : return Vector2.right;
            case 2 : return Vector2.down;
            case 3 : return Vector2.left;
            default : return Vector2.zero;
        }
    }
}

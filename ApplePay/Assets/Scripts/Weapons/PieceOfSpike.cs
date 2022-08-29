using UnityEngine;

public class PieceOfSpike : Projectile
{
    protected override void Start()
    {
        base.Start();
        transform.eulerAngles = Vector3.forward * Pay.Functions.Math.Atan3(MoveVector.y, MoveVector.x);
    }
}

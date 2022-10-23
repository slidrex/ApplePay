using UnityEngine;

[System.Serializable]

public class PayCollisionHandler
{
    public Rigidbody2D rb;
    [Range(0, 1f)] public float resistance;
    public void AddForce(Vector2 force) => rb.AddForce(rb.mass * force * (1 - resistance), ForceMode2D.Impulse);
}
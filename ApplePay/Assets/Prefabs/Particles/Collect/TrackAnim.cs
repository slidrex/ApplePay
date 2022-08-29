using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TrackAnim : MonoBehaviour
{
    [HideInInspector] public GameObject TrackObject;
    private Rigidbody2D rb;
    [HideInInspector] public Transform DestinationPoint;
    [Header("Track settings")]
    public float DelayBeforeTransform;
    private float currentDelay;
    public float ImpulseForce;
    public float Acceleration;
    private float speed;
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(Random.Range(-1f,1f) * ImpulseForce,Random.Range(-1f,1f) * ImpulseForce);

    }
    protected virtual void Update()
    {
        if(currentDelay < DelayBeforeTransform)
            currentDelay += Time.deltaTime;
        else 
            TrackMoving();
    }
    private void TrackMoving()
    {
        speed += Acceleration;
        transform.position = Vector2.MoveTowards(transform.position, DestinationPoint.position, speed * Time.deltaTime);
        if((Vector2)transform.position == (Vector2)DestinationPoint.position) AimDestinate();
    }
    protected virtual void AimDestinate()
    {
        Destroy(gameObject);
    }
}

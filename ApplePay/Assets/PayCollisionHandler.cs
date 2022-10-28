using UnityEngine;

[System.Serializable]

public class PayCollisionHandler
{
    public Rigidbody2D rb;
    [Range(0f, 1f)] public float resistance;
    public float AddDrag = 1f;
    [ReadOnly] public bool disabled;
    public System.Collections.Generic.List<PayKnock> Forces = new System.Collections.Generic.List<PayKnock>();
    public bool Knockbacked {get => Forces.Count != 0;}
    public void Knock(Vector2 startSpeed, float drag) => Knock(startSpeed, 0, drag);
    public void Knock(Vector2 startSpeed, float disableTime, float drag)
    {
        Vector2 resultSpeed = Vector2.zero;
        resultSpeed = rb.velocity;
        resultSpeed += startSpeed * (1 - resistance);
        Forces.Add(new PayKnock(startSpeed, disableTime, drag * AddDrag));
    }
    public void OnUpdate()
    {
        HandleKnockback();
        ApplyKnockback();   
    }
    private void HandleKnockback()
    {
        disabled = false;
        for(int i = 0; i < Forces.Count; i++)
        {
            PayKnock force = Forces[i];
            if(force.RemainingDisableTime > 0) 
            {
                force.RemainingDisableTime -= Time.deltaTime;
                disabled = true;
            }
            force.CurrentSpeed = Vector2.MoveTowards(force.CurrentSpeed, Vector2.zero, force.Drag * Time.deltaTime);
            if(force.CurrentSpeed != Vector2.zero || force.RemainingDisableTime != 0f)
                Forces[i] = force;
            else Forces.RemoveAt(i);
        }

    }
    private void ApplyKnockback()
    {
        if(Forces.Count > 0)
        {
            rb.velocity = Vector2.zero;
            for(int i = 0; i < Forces.Count; i++) 
                rb.velocity += Forces[i].CurrentSpeed;
        }
    }
}
[System.Serializable]
public struct PayKnock
{
    public Vector2 CurrentSpeed;
    public float RemainingDisableTime;
    public float Drag;
    public PayKnock(Vector2 speed, float disableTime, float drag) 
    {
        CurrentSpeed = speed;
        RemainingDisableTime = disableTime;
        Drag = drag;
    }
}
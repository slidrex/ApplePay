using UnityEngine;

[System.Serializable]

public class PayCollisionHandler
{
    [HideInInspector] public Rigidbody2D rb;
    [Range(0f, 1f)] public float resistance;
    public float DragIntensity = 1f;
    [ReadOnly] public bool disabled;
    public System.Collections.Generic.List<PayKnock> Forces = new System.Collections.Generic.List<PayKnock>();
    public bool Knockbacked { get => Forces.Count != 0; }
    public void Knock(Vector2 startSpeed, float decceleration)
    {
        Vector2 resultSpeed = rb.velocity + startSpeed * (1 - resistance);
        Forces.Add(new PayKnock(startSpeed, decceleration * DragIntensity));
    }
    public void OnUpdate()
    {
        HandleKnockback();
        ApplyKnockback();   
    }
    private void HandleKnockback()
    {
        if(disabled == true && Knockbacked == false) disabled = false;
        for(int i = 0; i < Forces.Count; i++)
        {
            disabled = true;
            PayKnock force = Forces[i];
            force.CurrentSpeed = Vector2.MoveTowards(force.CurrentSpeed, Vector2.zero, force.Drag * Time.deltaTime);
            if(force.CurrentSpeed != Vector2.zero)
            {
                Forces[i] = force;
            }
            else Forces.RemoveAt(i);
        }

    }
    private void ApplyKnockback()
    {
        if(Forces.Count > 0)
        {
            Vector2 resultSpeed = Vector2.zero;
            for(int i = 0; i < Forces.Count; i++) 
                resultSpeed += Forces[i].CurrentSpeed;
            rb.velocity = resultSpeed;
        }
    }
}
[System.Serializable]
public struct PayKnock
{
    public Vector2 CurrentSpeed;
    public float Drag;
    public PayKnock(Vector2 speed, float drag) 
    {
        CurrentSpeed = speed;
        Drag = drag;
    }
}
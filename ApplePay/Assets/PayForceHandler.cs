using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PayForceHandler : MonoBehaviour
{
    public Rigidbody2D Rigidbody;
    public float PhysicalCollisionsResistance = 5.0f;
    [Range(-1f, 1f)] public float resistance = 0.0f;
    public float DragIntensity = 1f;
    public bool disabled { get; set; }
    private System.Collections.Generic.List<PayForce> Forces = new System.Collections.Generic.List<PayForce>();
    public bool Knockbacked { get => Forces.Count != 0; }
    private void Start()
    {
        Rigidbody.mass = PhysicalCollisionsResistance;        
    }
    public void Knock(Vector2 startSpeed, float decceleration, bool disable)
    {
        Vector2 resultSpeed = Rigidbody.velocity + startSpeed * (1 - resistance);
        Forces.Add(new PayForce(startSpeed, decceleration * DragIntensity, disable));
    }
    public void ResetForces() => Forces.Clear();
    public Vector2 GetResultanForce() => Rigidbody.velocity;
    public PayForce[] GetCurrentForces() => Forces.ToArray();
    public void SetResultanForce(Vector2 startSpeed, float decceleration, bool disable)
    {
        ResetForces();
        Knock(startSpeed, decceleration, disable);
    }
    public void SetResultanForce(PayForce[] forces)
    {
        ResetForces();
        Forces.AddRange(forces);
    }
    public void Update()
    {
        HandleKnockback();
        ApplyForces();   
    }
    private void HandleKnockback()
    {
        for(int i = 0; i < Forces.Count; i++)
        {
            PayForce force = Forces[i];
            force.CurrentSpeed = Vector2.MoveTowards(force.CurrentSpeed, Vector2.zero, force.Drag * Time.deltaTime);
            if(force.CurrentSpeed != Vector2.zero)
            {
                Forces[i] = force;
            }
            else Forces.RemoveAt(i);
            disabled = false;
        }
        foreach(PayForce force in Forces)
        {
            if(force.Disable) 
            {
                disabled = true;
                break;
            }
        }

    }
    private void ApplyForces()
    {
        if(Forces.Count > 0)
        {
            Vector2 resultSpeed = Vector2.zero;
            for(int i = 0; i < Forces.Count; i++) 
                resultSpeed += Forces[i].CurrentSpeed;
            Rigidbody.velocity = resultSpeed;
        }
    }
}
[System.Serializable]
public struct PayForce
{
    public Vector2 CurrentSpeed;
    public float Drag;
    public bool Disable;
    public PayForce(Vector2 speed, float drag, bool disable) 
    {
        CurrentSpeed = speed;
        Drag = drag;
        Disable = disable;
    }
}
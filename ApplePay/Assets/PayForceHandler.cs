using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PayForceHandler : MonoBehaviour
{
    public Rigidbody2D Rigidbody;
    public KnockbackResponse knockbackResponse;
    public float PhysicalCollisionsResistance = 5.0f;
    [Range(-1f, 1f)] public float resistance = 0.0f;
    public float DragIntensity = 1f;
    public bool disabled { get; set; }
    private System.Collections.Generic.List<PayForce> Forces = new System.Collections.Generic.List<PayForce>();
    private System.Collections.Generic.List<float> blockedForces = new System.Collections.Generic.List<float>();
    internal struct ExternalForce
    {
        internal bool isForceBinded;
        internal bool nullOnForceUnbind;
    }
    public bool AreIncomingForcesDisabled() => blockedForces.Count != 0;
    public bool Knockbacked { get => Forces.Count != 0; }
    private void Start()
    {
        Rigidbody.mass = PhysicalCollisionsResistance;        
    }
    public void Knock(Vector2 startSpeed, float decceleration, float disableTime = 0.0f)
    {
        if(AreIncomingForcesDisabled()) return;
        Vector2 resultSpeed = Rigidbody.velocity + startSpeed * (1 - resistance);
        Forces.Add(new PayForce(startSpeed, decceleration * DragIntensity, disableTime));
    }
    private void ResetForces() => Forces.Clear();
    public Vector2 GetResultanForce() => Rigidbody.velocity;
    public PayForce[] GetCurrentForces() => Forces.ToArray();
    public void SetResultanForce(Vector2 startSpeed, float decceleration, float disableTime = 0.0f)
    {
        ResetForces();
        Knock(startSpeed, decceleration, disableTime);
    }
    public void SetResultanForce(PayForce[] forces)
    {
        ResetForces();
        Forces.AddRange(forces);
    }
    public void BindForce(Vector2 force, float damping, float time)
    {
        SetResultanForce(force, damping, time);
        BlockIncomingForces(time);
    }
    private void BlockIncomingForces(float time)
    {
        blockedForces.Add(time);
    }
    public void Update()
    {
        HandleKnockback();
        ApplyForces();   
    }
    private void HandleKnockback()
    {
        for(int i = 0; i < blockedForces.Count; i++)
        {
            if(blockedForces[i] > 0.0f) blockedForces[i] -= Time.deltaTime;
            else blockedForces.RemoveAt(i);
        }
        for(int i = 0; i < Forces.Count; i++)
        {
            PayForce force = Forces[i];
            if(force.DisableTime > 0) force.DisableTime -= Time.deltaTime;
            force.CurrentSpeed = Vector2.MoveTowards(force.CurrentSpeed, Vector2.zero, force.Damping * Time.deltaTime);
            if(force.CurrentSpeed != Vector2.zero)
            {
                Forces[i] = force;
            }
            else 
            {
                Forces.RemoveAt(i);
            }
            disabled = true;
        }
        float disableTime = 0.0f;
        foreach(PayForce force in Forces)
        {
            disableTime += force.DisableTime;
        }
        if(disabled && disableTime <= 0.0f) disabled = false;

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
    public enum KnockbackResponse
    {
        Disable,
        NonDisable
    }
}
[System.Serializable]
public struct PayForce
{
    public Vector2 CurrentSpeed;
    public float Damping;
    public float DisableTime;
    public PayForce(Vector2 speed, float damping, float disableTime) 
    {
        CurrentSpeed = speed;
        Damping = damping;
        DisableTime = disableTime;
    }
}
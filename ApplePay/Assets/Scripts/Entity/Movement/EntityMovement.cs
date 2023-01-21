using UnityEngine;
using System.Linq;

abstract public class EntityMovement : MonoBehaviour
{
    public System.Collections.Generic.Dictionary<byte, PayDisable> Disables {get; private set;} = new System.Collections.Generic.Dictionary<byte, PayDisable>();
    public bool isDisabled {get; private set;}
    public Animator animator {get; private set;}
    [Header("Entity Movement")]
    [SerializeField] private float CurrentSpeed = Mathf.PI;
    protected Vector2 MoveVector;
    [HideInInspector] public Rigidbody2D Rigidbody;
    [HideInInspector] public bool FacingBlock;
    private float curConstraintDuration;
    public Creature Entity {get; set;}
    protected virtual void Start()
    {
        if(Rigidbody == null) Rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    public float GetCurrentSpeed() => CurrentSpeed;
    public Vector2 GetMovementVector() => MoveVector;
    public void SetMovementVector(Vector2 vector, bool disableChanges) => MoveVector = vector;
    public void SetCurrentSpeed(float speed, bool disableChanges) => CurrentSpeed = speed;
    public bool ContainsDisable(byte id) => Disables.ContainsKey(id);
    public bool RemoveDisable(byte id) => Disables.Remove(id);
    public byte AddDisable(float time) => AddDisable(time, false);
    public byte AddDisable() => AddDisable(0, true);
    private byte AddDisable(float time, bool everlasting)
    {
        byte id = Pay.Functions.Math.GetUniqueByte(Disables.Keys.ToArray(), 0);
        Disables.Add(id, new PayDisable(everlasting, time));
        return id;
    }
    protected virtual void Update() 
    {
        RotationConstraintHandler();
        DisableHandler();
    }
    protected virtual void FixedUpdate() => OnSpeedUpdate();
    virtual protected void OnSpeedUpdate() {}
    private void DisableHandler()
    {
        if(Entity.IsBlocked())
        {
            isDisabled = true;
            return;
        }
        if(Disables.Count == 0 && isDisabled) isDisabled = false;
        for(int i = 0; i < Disables.Count; i++)
        {
            isDisabled = true;
            PayDisable current = Disables.ElementAt(i).Value;
            if(current.Everlasting == false) 
            {
                current.RemainingTime -= Time.deltaTime;
                
                Disables[Disables.ElementAt(i).Key] = current;
                if(current.RemainingTime <= 0) Disables.Remove(Disables.ElementAt(i).Key);   
            }
        }
    }
    protected void RotationConstraintHandler()
    {
        if(curConstraintDuration > 0)
        {
            FacingBlock = true;
            curConstraintDuration -= Time.deltaTime;
            if(curConstraintDuration <= 0) FacingBlock = false;
        }
    }
    public void SetMoveMod(bool isMoving) => animator.SetBool("isMoving", isMoving);
    public void SetRotationConstraint(float duration) => curConstraintDuration = duration;
    public void SetFacing(Vector2 state, float bindTime, params StateParameter[] parameters)
    {   
        SetRotationConstraint(bindTime);
        animator.SetInteger("Horizontal", (int)state.x);
        animator.SetInteger("Vertical", (int)state.y);
        foreach(StateParameter param in parameters)
        {
            switch(param)
            {
                case StateParameter.MirrorHorizontal:
                    transform.eulerAngles = state.x == 0 ? transform.eulerAngles : (state.x < 0 ? new Vector3(transform.eulerAngles.x, 180, transform.eulerAngles.z) : new Vector3(transform.eulerAngles.x, 0, transform.eulerAngles.z));
                break;
                case StateParameter.MirrorVertical:
                    transform.eulerAngles = state.y == 0 ? transform.eulerAngles : (state.y < 0 ? new Vector3(180, transform.eulerAngles.y, transform.eulerAngles.z) : new Vector3(0 , transform.eulerAngles.y, transform.eulerAngles.z));
                break;
            }
        }
    }
}
public enum StateParameter
{
    MirrorHorizontal,
    MirrorVertical
}
public struct PayDisable
{
    public bool Everlasting;
    public float RemainingTime;
    public PayDisable(bool everlasting, float time)
    {
        RemainingTime = time;
        Everlasting = everlasting;
    }
}
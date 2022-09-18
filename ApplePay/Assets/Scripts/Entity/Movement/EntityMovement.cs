using UnityEngine;
abstract public class EntityMovement : MonoBehaviour
{
    public Animator animator {get; private set;}
    [Header("Entity Movement")]
    public float BaseSpeed;
    public float AdditionalSpeed;
    public float SpeedMultiplier { get; private set; } = 1;
    [HideInInspector] public bool MoveDisable;
    [HideInInspector] public bool OnlyPositive;
    [HideInInspector] public bool Reversed { get; private set; }
    public float CurrentSpeed { get; private set; }
    [ReadOnly] public Vector2 MoveVector;
    [ReadOnly] public Rigidbody2D Rigidbody;
    [HideInInspector] public bool ConstraintRotation;
    private float curConstraintDuration;
    public void ReverseControl() => Reversed = !Reversed;
    public void ChangeSpeedMultiplier(float value) => ChangeSpeedMultiplier(value, out float changedValue);
    public void ChangeSpeedMultiplier(float value, out float changedValue)
    {
        float oldValue = SpeedMultiplier;
        SpeedMultiplier += value;
        changedValue = SpeedMultiplier - oldValue;
    }
    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        Rigidbody = GetComponent<Rigidbody2D>();
    }
    protected virtual void Update() => RotationConstraintHandler();
    protected virtual void FixedUpdate()
    {
        float resultMultiplier = SpeedMultiplier < 0 && OnlyPositive ? 0 : SpeedMultiplier;
        OnBeforeSpeedUpdate();
        CurrentSpeed = (BaseSpeed + AdditionalSpeed) * resultMultiplier;
        if(Reversed) CurrentSpeed = -CurrentSpeed;
        if(MoveDisable) CurrentSpeed = 0;
        OnAfterSpeedUpdate();
    }
    virtual protected void OnBeforeSpeedUpdate() {}
    virtual protected void OnAfterSpeedUpdate() {}
    protected void RotationConstraintHandler()
    {
        if(curConstraintDuration >= 0)
        {
            curConstraintDuration -= Time.deltaTime;
            ConstraintRotation = true;
        }
        else if(ConstraintRotation) ConstraintRotation = false;
    }
    public void SetMoveMod(bool isMoving) => animator.SetBool("isMoving", isMoving);
    public void SetRotationConstraint(float duration) => curConstraintDuration = duration;
    public void SetFacingState(Vector2 state, float duration, params StateParameter[] parameters)
    {   
        SetRotationConstraint(duration);
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
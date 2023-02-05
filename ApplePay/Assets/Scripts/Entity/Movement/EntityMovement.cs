using UnityEngine;
using System.Linq;

abstract public class EntityMovement : MonoBehaviour
{
    [SerializeField] private FacingParameter facingParameter;
    private System.Collections.Generic.Dictionary<byte, MovementDisable> Disables = new System.Collections.Generic.Dictionary<byte, MovementDisable>();
    public bool isMovementDisabled { get; private set; }
    public bool isFacingDisabled { get; private set; }
    public Animator animator {get; private set;}
    [Header("Entity Movement")]
    [SerializeField] private float CurrentSpeed = Mathf.PI;
    protected Vector2 MoveVector;
    [HideInInspector] public Rigidbody2D Rigidbody;
    private Vector2 currentFacing;
    public Creature Entity {get; set;}
    protected virtual void Start()
    {
        currentFacing = Vector2.down;
        if(Rigidbody == null) Rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    public Vector2 GetClosestFacing(Vector2 vector) => Mathf.Abs(vector.y) > Mathf.Abs(vector.x) ? Vector2.up * Mathf.Sign(vector.y) : Vector2.right * vector.x;
    public float GetCurrentSpeed() => CurrentSpeed;
    public Vector2 GetMovementVector() => MoveVector;
    public void SetMovementVector(Vector2 vector, bool disableChanges) => MoveVector = vector;
    public void SetCurrentSpeed(float speed, bool disableChanges) => CurrentSpeed = speed;
    public bool ContainsDisable(byte id) => Disables.ContainsKey(id);
    public bool RemoveDisable(byte id) => Disables.Remove(id);
    public byte AddDisable(bool blockFacing = true, bool blockMovement = true) => AddDisable(0, true, blockFacing, blockMovement);
    public byte AddDisable(float time, bool blockFacing = true, bool blockMovement = true) => AddDisable(time, false, blockFacing, blockMovement);
    private byte AddDisable(float time, bool everlasting, bool blockFacing, bool blockMovement)
    {
        byte id = Pay.Functions.Math.GetUniqueByte(Disables.Keys.ToArray(), 0);
        MovementDisable disable = new MovementDisable() { Everlasting = everlasting, RemainingTime = time, BlockFacing = blockFacing, BlockMovement = blockMovement };
        Disables.Add(id, disable);
        return id;
    }
    protected virtual void Update() 
    {
        DisableHandler();
    }
    protected virtual void FixedUpdate() => OnSpeedUpdate();
    virtual protected void OnSpeedUpdate() {}
    private void DisableHandler()
    {
        if(Entity.IsBlocked())
        {
            isMovementDisabled = true;
            isFacingDisabled = true;
            return;
        }
        bool blockFacing = false;
        bool blockMovement = false;
        for(int i = 0; i < Disables.Count; i++)
        {
            MovementDisable iterationDisable = Disables.ElementAt(i).Value;
            if(iterationDisable.BlockMovement)
                blockMovement = true;
            if(iterationDisable.BlockFacing)
                blockFacing = true;
            
            
            if(iterationDisable.Everlasting == false) 
            {
                iterationDisable.RemainingTime -= Time.deltaTime;
                byte iterationKey = Disables.ElementAt(i).Key;
                
                Disables[iterationKey] = iterationDisable;
                if(iterationDisable.RemainingTime <= 0) Disables.Remove(iterationKey);   
            }
        }
        isMovementDisabled = blockMovement;
        isFacingDisabled = blockFacing;
    }
    public Vector2 GetFacing() => currentFacing;
    public void SetMoveMod(bool isMoving) => animator.SetBool("isMoving", isMoving);
    public bool SetFacing(Vector2 state, float bindTime = 0.0f)
    {
        if(isFacingDisabled) return false;
        
        currentFacing = state; 
        if(bindTime != 0.0f)
            AddDisable(bindTime, true, false);
        animator.SetInteger("Horizontal", (int)state.x);
        animator.SetInteger("Vertical", (int)state.y);
        
        if(facingParameter == FacingParameter.MirrorHorizontal)
            transform.eulerAngles = state.x == 0 ? transform.eulerAngles : (state.x < 0 ? new Vector3(transform.eulerAngles.x, 180, transform.eulerAngles.z) : new Vector3(transform.eulerAngles.x, 0, transform.eulerAngles.z));
        else
            transform.eulerAngles = state.y == 0 ? transform.eulerAngles : (state.y < 0 ? new Vector3(180, transform.eulerAngles.y, transform.eulerAngles.z) : new Vector3(0 , transform.eulerAngles.y, transform.eulerAngles.z));
            
        return true;
    }
    internal struct MovementDisable
    {
        internal bool Everlasting;
        internal float RemainingTime;
        internal bool BlockFacing;
        internal bool BlockMovement;
    }
    public enum FacingParameter
    {
        MirrorHorizontal,
        MirrorVertical
    }
}
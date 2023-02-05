using UnityEngine;

public class PlayerMovement : EntityMovement
{
    public bool cursorFollow;
    [SerializeField] private Transform facingTransform;
    protected override void Update()
    {
        base.Update();
        if(cursorFollow && !isFacingDisabled)
        {
            SetFacing(GetCurrentFacing());
        }
        MoveInput();
    }
    private Vector2 GetCurrentFacing()
    {
        Vector2 facing = Vector2.zero;
        Vector2 distance = Pay.Functions.Generic.GetMousePos(Camera.main) - (Vector2)facingTransform.transform.position;
        if(Mathf.Abs(distance.x) > Mathf.Abs(distance.y)) facing = Vector2.right * Mathf.Sign(distance.x);
        else facing = Vector2.up * Mathf.Sign(distance.y);
        return facing;
    }
    private void MoveInput()
    {
        Vector2 inputVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if(isMovementDisabled == false)
        {
            MoveVector = inputVector;
            if(GetMovementVector() != Vector2.zero) animator.SetBool("isMoving", true);
            else animator.SetBool("isMoving", false);
        }
        if(!isFacingDisabled && !cursorFollow && inputVector != Vector2.zero) SetFacing(inputVector);
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if(GetMovementVector() != Vector2.zero && isMovementDisabled) SetMovementVector(Vector2.zero, false);
        if(!isMovementDisabled)
        Rigidbody.velocity = GetMovementVector() * GetCurrentSpeed();
    }
}
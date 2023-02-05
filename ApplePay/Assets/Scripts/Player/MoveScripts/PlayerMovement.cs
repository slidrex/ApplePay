using UnityEngine;

public class PlayerMovement : EntityMovement
{
    public bool cursorFollow;
    [SerializeField] private Transform facingTransform;
    protected override void Update()
    {
        base.Update();
        if(cursorFollow && !FacingBlock)
        {
            SetFacing(GetCurrentFacing(), 0.0f, StateParameter.MirrorHorizontal);
        }
        if(GetCurrentSpeed() != 0) MoveInput();
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
        MoveVector.x = Input.GetAxisRaw("Horizontal");
        MoveVector.y = Input.GetAxisRaw("Vertical");
        if(GetMovementVector().x != 0 || GetMovementVector().y != 0) animator.SetBool("isMoving", true);
        else if(GetMovementVector() == Vector2.zero) animator.SetBool("isMoving", false);
        if(cursorFollow == false && !FacingBlock && GetMovementVector() != Vector2.zero) SetFacing(MoveVector, 0.0f, StateParameter.MirrorHorizontal);
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if(GetMovementVector() != Vector2.zero && isDisabled) SetMovementVector(Vector2.zero, false);
        Rigidbody.velocity = GetMovementVector() * GetCurrentSpeed();
    }
}
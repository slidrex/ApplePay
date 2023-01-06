using UnityEngine;

public class PlayerMovement : EntityMovement
{
    
    protected override void Update()
    {
        base.Update();
        if(GetCurrentSpeed() != 0) MoveInput();
    }
    private void MoveInput()
    {
        MoveVector.x = Input.GetAxisRaw("Horizontal");
        MoveVector.y = Input.GetAxisRaw("Vertical");
        if(GetMovementVector().x != 0 || GetMovementVector().y != 0) animator.SetBool("isMoving", true);
        else if(GetMovementVector() == Vector2.zero) animator.SetBool("isMoving", false);
        if(!ConstraintRotation) PolarityChange();
    }
    private void PolarityChange()
    {
        animator.SetInteger("Vertical", (int)GetMovementVector().y);
        animator.SetInteger("Horizontal", (int)GetMovementVector().x);

        if(GetMovementVector().x > 0)
            transform.eulerAngles = new Vector2(0, 0);
        else if(GetMovementVector().x < 0)
            transform.eulerAngles = new Vector2(0, -180);
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if(GetMovementVector() != Vector2.zero && isDisabled) SetMovementVector(Vector2.zero, false);
        Rigidbody.velocity = GetMovementVector() * GetCurrentSpeed();
    }
}
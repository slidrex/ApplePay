using UnityEngine;

public class PlayerMovement : EntityMovement
{
    protected override void Update()
    {
        base.Update();
        if(CurrentSpeed != 0) MoveInput();
    }
    private void MoveInput()
    {
        MoveVector.x = Input.GetAxisRaw("Horizontal");
        MoveVector.y = Input.GetAxisRaw("Vertical");
        if(MoveVector.x != 0 || MoveVector.y != 0) animator.SetBool("isMoving", true);
        else if(MoveVector == Vector2.zero) animator.SetBool("isMoving", false);
        if(!ConstraintRotation) PolarityChange();
    }
    private void PolarityChange()
    {
        animator.SetInteger("Vertical", (int)MoveVector.y);
        animator.SetInteger("Horizontal", (int)MoveVector.x);

        if(MoveVector.x > 0)
            transform.eulerAngles = new Vector2(0, 0);
        else if(MoveVector.x < 0)
            transform.eulerAngles = new Vector2(0, -180);
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        Rigidbody.velocity = MoveVector * CurrentSpeed;
    }
}
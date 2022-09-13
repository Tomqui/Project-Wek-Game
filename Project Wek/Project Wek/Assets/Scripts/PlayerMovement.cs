using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 2f;
    public Rigidbody2D rb;
    public Animator animator;

    Vector2 movement;
    
    public void IncreaseMove(float move)
    {
        moveSpeed += move;
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if(movement.x != 0 || movement.y != 0){
            animator.SetBool("Moving", true);
        }
        else{
            animator.SetBool("Moving", false);
        }
    }

    private void FixedUpdate()
    {
        if(movement.x<0){
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (movement.x > 0){
            transform.rotation = Quaternion.Euler(0,180,0);
        }

        rb.MovePosition(rb.position+movement*moveSpeed*Time.fixedDeltaTime);
    }
}

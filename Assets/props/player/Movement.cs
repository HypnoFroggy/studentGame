using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Rigidbody2D rigidBody;
    public Animator animator;
    public float speedMultiplier = 2;
    private float xSpeed = 0;
    private float ySpeed = 0;
    private bool facing = true;
    void Start()
    {

    }
    void Update()
    {
        if (!(Input.GetKeyDown(KeyCode.W) && Input.GetKeyDown(KeyCode.S)))
        {
            if (Input.GetKeyDown(KeyCode.W)) ySpeed = 1;
            else if (Input.GetKeyUp(KeyCode.W)) ySpeed = 0;
            if (Input.GetKeyDown(KeyCode.S)) ySpeed = -1;
            else if (Input.GetKeyUp(KeyCode.S)) ySpeed = 0;
        }
        if (!(Input.GetKeyDown(KeyCode.A) && Input.GetKeyDown(KeyCode.D)))
        {
            if (Input.GetKeyDown(KeyCode.A)) xSpeed = -1;
            else if (Input.GetKeyUp(KeyCode.A)) xSpeed = 0;
            if (Input.GetKeyDown(KeyCode.D)) xSpeed = 1;
            else if (Input.GetKeyUp(KeyCode.D)) xSpeed = 0;
        }

        if (xSpeed == 1 && !facing) Flip();
        if (xSpeed == -1 && facing) Flip();
        if (xSpeed != 0 || ySpeed != 0) animator.SetBool("isMoving", true);
        else animator.SetBool("isMoving", false);

        rigidBody.velocity = new Vector2(xSpeed, ySpeed).normalized * speedMultiplier;
    }
    void Flip()
    {
        facing = !facing;
        transform.Rotate(0f, 180f, 0f);
    }
}

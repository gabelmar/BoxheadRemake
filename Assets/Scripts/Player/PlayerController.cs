using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField]
    private PlayerShoot gun;
    [SerializeField]
    private Player player;
    private float movementSpeed;
    private float maxSpeed = 5f;
    private string horizontalMovement, verticalMovement;
    private string scrollWheel;
    private Vector3 moveVelocity;
    private bool movingUp, movingRight;

    // Start is called before the first frame update
    void Start()
    {
        verticalMovement = "Vertical";
        horizontalMovement = "Horizontal";
        scrollWheel = "Mouse ScrollWheel";
        rb = GetComponent<Rigidbody>();
        movementSpeed = 5f;
    }

    void Update(){
        PlayerMovement();

        if(Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)){
            gun.SetFiring(true);
        }
        if(Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.Space))
        {
            gun.SetFiring(false);
        }

        float scrollAxis = Input.GetAxis(scrollWheel);
        if (scrollAxis < 0.0f)
        {
            player.SwitchWeapon(true);
        }
        else if (scrollAxis > 0.0f)
        {
            player.SwitchWeapon(false);
        }
           
    }
    void FixedUpdate()
    {
       rb.velocity = moveVelocity;
    }

    private void PlayerMovement(){
        float vertical = Input.GetAxisRaw(verticalMovement);
        float horizontal = Input.GetAxisRaw(horizontalMovement);

        // determine if moving up or down for handling double button pressing (w and s at teh same time)
        if(vertical == 1)
            movingUp = true;
        else if (vertical == -1)
            movingUp = false;

        // determine if moving right or left for handling double button pressing (a and d at the same time)
        if (horizontal == 1)
            movingRight = true;
        else if (horizontal == -1)
            movingRight = false;

        // if both up and down are pressed the input will return 0 for vertical axis, if so check if the previous movement direction was up.
        // if it was and the s button is pressed then move down (set vertical to -1),
        // if the previous movemnt direction was not up (so down :D) and w is pressed, move up (vertical = 1)
        // that way whenever you press w and upon that press s at the same time, we want the character to move down. Same goes for the other way around.
        if (vertical == 0){
            if(movingUp && Input.GetKey("s")){
                vertical = -1;
            }
            else if(!movingUp && Input.GetKey("w")){
                vertical = 1;
            }
        }

        // do the same for left and right (horizontal)
        if(horizontal == 0){
            if(movingRight && Input.GetKey("a")){
                horizontal = -1;
            }
            else if(!movingRight && Input.GetKey("d")){
                horizontal = 1;
            }
        }

        Vector3 direction = new Vector3(horizontal, 0, vertical);
        moveVelocity = direction * movementSpeed;
        // clamp the speed (the velocities magnitude) at max speed to avoid faster movement for walking diagonally
        moveVelocity = Vector3.ClampMagnitude(moveVelocity, maxSpeed);
        
        if (Math.Abs(horizontal) <= 0.1f && Math.Abs(vertical) <= 0.1f) 
            return;

        //make the character look in the direction he wants to move to
        transform.LookAt(transform.position + direction);
    }

}

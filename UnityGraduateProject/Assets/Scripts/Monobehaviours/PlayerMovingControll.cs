using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovingControll : BaseMonoBehaviour
{
    [SerializeField] float movespeed;
    [SerializeField] float gravity;
    [SerializeField] float turnSpeed;
    [SerializeField] float rotSmooth;
    [SerializeField] CharacterController charcontr;
    float inputX, inputY, angle;
    public bool canMove;
    PlayerAnimatorController animcontr;
    Transform player;
    [SerializeField] Transform charRotTarget;
    Vector3 movevector;
    Vector3 rotateVector;
    Quaternion playerRot;

    public bool isMoving;

    private void Start()
    {
        charcontr = GetComponent<CharacterController>();
        player = transform.parent;
        animcontr = GetComponent<PlayerAnimatorController>();
        charRotTarget = Camera.main.transform.GetChild(0);
        canMove = true;

        // Vars start:
        turnSpeed = 2f;
    }

    private void Update()
    {
        if (canMove)
            Moving();       
    }

    void Moving()
    {
        if (charcontr.isGrounded)
        {
            float speed = 0;
            /*movevector = new Vector3(Input.GetAxis("Horizontal"), 
                                     0f, Input.GetAxis("Vertical"));
            movevector = transform.TransformDirection(movevector);
            movevector *= movespeed;*/
                                       
            if (Input.GetButton("Right"))
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, transform.rotation.y + 90, 0), rotSmooth);
                speed = movespeed;
            }
            if (Input.GetButton("Left"))
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, transform.rotation.y - 90, 0), rotSmooth);
                speed = movespeed;
            }
            if (Input.GetButton("Forward"))
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, transform.rotation.y, 0), rotSmooth);
                speed = movespeed;
            }
            if (Input.GetButton("Back"))
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, transform.rotation.y + 180, 0), rotSmooth);
                speed = movespeed;
            }
            if (speed != 0)
                isMoving = true;
            else
                isMoving = false;
            movevector = transform.TransformDirection(Vector3.forward);
            movevector *= speed;
        }
        else
        {
            movevector.y -= gravity * Time.deltaTime;
        }        
        charcontr.Move(movevector * Time.deltaTime);
    }
}

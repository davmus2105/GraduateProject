﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovingControll : BaseMonoBehaviour, IDie
{
    [SerializeField] float movespeed;
    [SerializeField] float gravity;
    [SerializeField] float turnSpeed;
    [SerializeField] float rotSmooth;
    [SerializeField] CharacterController charcontr;
    float inputX, inputY, angle;
    public bool canMove;
    public bool isBlocking
    {
        get
        {
            return isBlocking;
        }
        set
        {
            isBlocking = value;
            animcontr.BlockAnimation(value);
        }
    }
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
        player = transform.Find("Model");
        animcontr = GetComponentInChildren<PlayerAnimatorController>();
        charRotTarget = Camera.main.transform.GetChild(0);
        canMove = true;

        // Vars start:
        turnSpeed = 2f;
    }

    private void Update()
    {
        if (canMove)
            Moving();
        Inputs();
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
                player.rotation = Quaternion.Lerp(player.rotation, Quaternion.Euler(0, player.rotation.y + 90, 0), rotSmooth);
                speed = movespeed;
            }
            if (Input.GetButton("Left"))
            {
                player.rotation = Quaternion.Lerp(player.rotation, Quaternion.Euler(0, player.rotation.y - 90, 0), rotSmooth);
                speed = movespeed;
            }
            if (Input.GetButton("Forward"))
            {
                player.rotation = Quaternion.Lerp(player.rotation, Quaternion.Euler(0, player.rotation.y, 0), rotSmooth);
                speed = movespeed;
            }
            if (Input.GetButton("Back"))
            {
                player.rotation = Quaternion.Lerp(player.rotation, Quaternion.Euler(0, player.rotation.y + 180, 0), rotSmooth);
                speed = movespeed;
            }
            if (speed != 0)
                isMoving = true;
            else
                isMoving = false;
            movevector = player.TransformDirection(Vector3.forward);
            movevector *= speed;
        }
        else
        {
            movevector.y -= gravity * Time.deltaTime;
        }        
        charcontr.Move(movevector * Time.deltaTime);
    }
    void Inputs()
    {
        if (Input.GetButtonDown("Quests"))
        {
            HUD_Controller.Instance.ShowHideQuestPanel();
            Debug.Log("Show quest panel");
        }
        if (Input.GetButtonDown("Block"))
            isBlocking = true;
        else if (Input.GetButtonUp("Block"))
            isBlocking = false;           
    }
    public void Die()
    {
        canMove = false;
    }
}

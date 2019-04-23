using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    Animator animator;
    PlayerMovingControll playerMoving;
    GameObject backplace, inhandplace;
    int speed_hash = Animator.StringToHash("speed");
    int jump_hash = Animator.StringToHash("jump");
    int arm = Animator.StringToHash("arm_disarm");
    int slash = Animator.StringToHash("slash");    
    bool isArmored;

    void Start()
    {
        animator = GetComponent<Animator>();
        playerMoving = GetComponent<PlayerMovingControll>();
        backplace = transform.Find("Armature").Find("Hips").Find("LowerSpine").
                    Find("Chest").Find("WeaponOnBackPlace").gameObject;
        inhandplace = transform.Find("Armature").Find("Hips").Find("LowerSpine").
                      Find("Chest").Find("ShoulderConnector.R").Find("Shoulder.R").
                      Find("UpperArm.R").Find("LowerArm.R").
                      Find("Hand.R").Find("WeaponInHandPlace").gameObject;
        isArmored = false;
        inhandplace.SetActive(false);
    }
    void Update()
    {
        MoveAnimations();
        KeyInputControll();
    }
    void MoveAnimations()
    {
        animator.SetFloat(speed_hash, Convert.ToInt32(playerMoving.isMoving));
    }
    public void JumpAnimation()
    {
        animator.SetTrigger(jump_hash);
    }
    void ChangeArmoredState()
    {
        animator.SetTrigger(arm);
    }
    void KeyInputControll()
    {
        if (Input.GetButtonDown("Jump"))
        {
            JumpAnimation();
        }
        if (Input.GetButtonDown("Attack"))
        {
            Attack();
        }
        if (Input.GetButtonDown("Arm/Disarm"))
        {
            ArmDisarm();
        }
    }
    void Attack()
    {
        if (isArmored)
        {
            animator.SetTrigger(slash);
        }
        else
        {
            Debug.Log("Warning!!!");
            HUD_Controller.Instance.ShowInfoMessage("You are not armored");
        }
    }
    void ArmDisarm()
    {
        isArmored = !isArmored;
        animator.SetTrigger(arm);
    }
    void SetCanMoveTrue()
    {
        playerMoving.canMove = true;
    }
    void SetCanMoveFalse()
    {
        playerMoving.canMove = false;
    }
    void EquipWeapon()
    {
        inhandplace.SetActive(true);
        HUD_Controller.Instance.WeaponIsReady(true);
        // backplace.SetActive(false);
        // Show weapon and sounds
    }
    void HideWeapon()
    {
        inhandplace.SetActive(false);
        HUD_Controller.Instance.WeaponIsReady(false);
        // backplace.SetActive(true);
        // Hide weapon and sounds
    }
    void PlayFootstepSound()
    {
        // Make audiomanager and add to it footsteps
    }
}

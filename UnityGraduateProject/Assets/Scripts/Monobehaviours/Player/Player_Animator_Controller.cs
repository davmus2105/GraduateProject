using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using GraduateAudio;

public class Player_Animator_Controller : MonoBehaviour
{
    Animator animator;
    PlayerBehaviour player;
    GameObject backplace, inhandplace;
    Collider weaponCollider;
    const float AFTER_DEATH_WAIT = 6f;
    int speed_hash = Animator.StringToHash("speed");
    int jump_hash = Animator.StringToHash("jump");
    int arm = Animator.StringToHash("arm_disarm");
    int slash = Animator.StringToHash("slash");
    int block = Animator.StringToHash("block");
    int die = Animator.StringToHash("die");
    int[] triggers;
    bool isArmored;
    // ----- instances of managers to work with -----
    HUD_Controller hudController;
    DialogueManager dialogueManager;
    AudioSource audioSource;
    public AudioCollection audio_collection;

    void Start()
    {
        triggers = new int[] { jump_hash, speed_hash, arm, slash, block, die };
        animator = GetComponent<Animator>();
        player = GetComponentInParent<PlayerBehaviour>();
        backplace = transform.Find("Armature").Find("Hips").Find("LowerSpine").
                    Find("Chest").Find("WeaponOnBackPlace").gameObject;
        inhandplace = transform.Find("Armature").Find("Hips").Find("LowerSpine").
                      Find("Chest").Find("ShoulderConnector.R").Find("Shoulder.R").
                      Find("UpperArm.R").Find("LowerArm.R").
                      Find("Hand.R").Find("WeaponInHandPlace").gameObject;
        weaponCollider = inhandplace.GetComponentInChildren<Collider>();
        weaponCollider.enabled = false;
        isArmored = false;
        inhandplace.SetActive(false);
        // ---- Instances ----
        hudController = HUD_Controller.Instance;
        dialogueManager = DialogueManager.Instance;
        audioSource = GetComponent<AudioSource>();
        if (!audioSource)
            audioSource = gameObject.AddComponent<AudioSource>();
    }
    void Update()
    {
        MoveAnimations();
        KeyInputControll();
    }
    void MoveAnimations()
    {        
        animator.SetFloat(speed_hash, player.movespeed, 0.0f, Time.deltaTime);
    }
    public void JumpAnimation()
    {
        ResetAllTriggers();
        animator.SetTrigger(jump_hash);
    }
    public void BlockAnimation(bool isBlocking)
    {
        ResetAllTriggers();
        if (isBlocking)
            SetCanMoveFalse();
        else
            SetCanMoveTrue();
        animator.SetBool(block, isBlocking);
    }
    public void Death()
    {
        // Death animation consist of:            
        // 1. Coroutine "DeathAnimation" and after a few seconds to call destroing
        StartCoroutine("DeathAnimation");
    }
    void ResetAllTriggers()
    {
        for (int i = 0; i < triggers.Length; i++)
        {
            animator.ResetTrigger(triggers[i]);
        }
    }
    void ChangeArmoredState()
    {
        ResetAllTriggers();
        animator.SetTrigger(arm);
    }
    void TurnOnWeaponCollider()
    {
        weaponCollider.enabled = true;
    }
    void TurnOffWeaponCollider()
    {
        weaponCollider.enabled = false;
    }
    void KeyInputControll()
    {
        /*if (Input.GetButtonDown("Jump"))
        {
            JumpAnimation();
        }*/
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
        if (dialogueManager.inDialogue || hudController.inQuestMenu || PauseMenu.GameIsPaused)
            return;
        if (isArmored)
        {
            animator.SetTrigger(slash);
            audioSource.volume = AudioManager.battleEffects_volume;
            audioSource.clip = audio_collection.battle_grunts.GetRandomItem();
            audioSource.Play();
        }
        else
        {
            hudController.ShowInfoMessage("Спочатку дістаньте зброю");
        }
    }

    void ArmDisarm()
    {
        isArmored = !isArmored;
        weaponCollider = inhandplace.GetComponentInChildren<Collider>();
        animator.SetTrigger(arm);
    }
    void SetCanMoveTrue()
    {
        player.canMove = true;
    }
    void SetCanMoveFalse()
    {
        player.canMove = false;
    }
    void EquipWeapon()
    {
        inhandplace.SetActive(true);
        hudController.WeaponIsReady(true);
        audioSource.volume = AudioManager.battleEffects_volume;
        audioSource.clip = audio_collection.get_sword;
        audioSource.Play();
        // backplace.SetActive(false);
        // Show weapon and sounds
    }
    void HideWeapon()
    {
        inhandplace.SetActive(false);
        hudController.WeaponIsReady(false);
        // backplace.SetActive(true);
        // Hide weapon and sounds
    }
    void PlayFootstepSound()
    {
        // Make audiomanager and add to it footsteps
        if (audioSource == null)
        {
            if (GetComponent<AudioSource>() == null)
                gameObject.AddComponent<AudioSource>();
            else
                audioSource = GetComponent<AudioSource>();
        }
        if (audio_collection != null)
        {
            audioSource.clip = audio_collection.footsteps_gravel.GetRandomItem();
            audioSource.volume = AudioManager.footstep_volume;
            audioSource.Play();
        }
        else
        {
            Debug.Log("There are no any AudioCollection component");
        }
    }

    // ------ Coroutines -------
    IEnumerator DeathAnimation()
    {
        animator.SetTrigger(die);
        yield return new WaitForSeconds(AFTER_DEATH_WAIT);
        // Call method Death() from ai_controller

        player.Death();
    }
}

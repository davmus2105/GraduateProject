using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour, IDie
{    
    [SerializeField] float movespeed;
    [SerializeField] float gravity;
    [SerializeField] float turnSpeed;
    [SerializeField] float rotSmooth;
    [SerializeField] float inBattleDist;
    [SerializeField] CharacterController charcontr;
    float inputX, inputY, angle;
    public bool canMove, isDead;
    public bool isBlocking
    {
        get
        {
            return isblocking;
        }
        set
        {
            isblocking = value;
            actor.isBlocking = isblocking;
            animcontr.BlockAnimation(isblocking);
        }
    }
    bool isblocking, in_battle;

    public static PlayerBehaviour Instance;
    private static PlayerBehaviour instance;
    public bool inBattle
    {
        get { return in_battle; }
        set
        {
            if (value != in_battle)
            {
                if (value == false)
                {
                    audioManager.isInBattle = false;
                    audioManager.BackgroundChoose();
                    in_battle = value;
                }
                else
                {
                    audioManager.isInBattle = true;
                    audioManager.BackgroundChoose();
                    in_battle = value;
                }
            }
        }
    }
    PlayerAnimatorController animcontr;
    Actor actor;
    Transform playeModel;
    [SerializeField] Transform charRotTarget;
    Vector3 movevector;
    Vector3 rotateVector;
    Quaternion playerRot;
    public GraduateAudio.AudioManager audioManager;

    public bool isMoving;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        charcontr = GetComponent<CharacterController>();
        playeModel = transform.Find("Model");
        animcontr = GetComponentInChildren<PlayerAnimatorController>();
        charRotTarget = Camera.main.transform.GetChild(0);
        canMove = true;
        isblocking = false;
        inBattle = false;
        actor = GetComponent<Actor>();
        audioManager = GraduateAudio.AudioManager.Instance;
        // Vars start:
        turnSpeed = 2f;
    }

    private void Update()
    {
        if (canMove)
            Moving();
        Inputs();
        BattleControll();
    }

    void BattleControll()
    {
        if (AI.AI_Manager.Instance.IsEnemyNear(transform.position, inBattleDist))
            inBattle = true;
        else
            inBattle = false;


    }

    void Moving()
    {
        if (DialogueManager.Instance.inDialogue)
        {
            isMoving = false;
            return;
        }
        else if (charcontr.isGrounded)
        {
            float speed = 0;
            /*movevector = new Vector3(Input.GetAxis("Horizontal"), 
                                        0f, Input.GetAxis("Vertical"));
            movevector = transform.TransformDirection(movevector);
            movevector *= movespeed;*/

            if (Input.GetButton("Right"))
            {
                playeModel.rotation = Quaternion.Lerp(playeModel.rotation, Quaternion.Euler(0, playeModel.rotation.y + 90, 0), rotSmooth);
                speed = movespeed;
            }
            if (Input.GetButton("Left"))
            {
                playeModel.rotation = Quaternion.Lerp(playeModel.rotation, Quaternion.Euler(0, playeModel.rotation.y - 90, 0), rotSmooth);
                speed = movespeed;
            }
            if (Input.GetButton("Forward"))
            {
                playeModel.rotation = Quaternion.Lerp(playeModel.rotation, Quaternion.Euler(0, playeModel.rotation.y, 0), rotSmooth);
                speed = movespeed;
            }
            if (Input.GetButton("Back"))
            {
                playeModel.rotation = Quaternion.Lerp(playeModel.rotation, Quaternion.Euler(0, playeModel.rotation.y + 180, 0), rotSmooth);
                speed = movespeed;
            }
            if (speed != 0)
                isMoving = true;
            else
                isMoving = false;
            movevector = playeModel.TransformDirection(Vector3.forward);
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
        isDead = true;
        animcontr.Death();
    }
    public void Death()
    {
        EventManager.TriggerEvent("PlayerDeath");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour, IDie
{
    [SerializeField] float speed;
    [HideInInspector]public float movespeed;
    [SerializeField] float gravity;
    [SerializeField] float turnSpeed;
    [SerializeField] float rotSmooth;
    [SerializeField] float inBattleDist;
    [Tooltip("standard is ")] [SerializeField] float diffToTarget;
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
    Transform playerModel;
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
        playerModel = transform.Find("Model");
        animcontr = GetComponentInChildren<PlayerAnimatorController>();
        charRotTarget = Camera.main.transform.GetChild(0);
        canMove = true;
        isblocking = false;
        inBattle = false;
        actor = GetComponent<Actor>();
        audioManager = GraduateAudio.AudioManager.Instance;
    }

    private void Update()
    {
        if (canMove)
            Moving();
        Inputs();
        BattleControll();
    }

    void RotateToCam()
    {        
        transform.rotation = Quaternion.Euler(0f, charRotTarget.rotation.eulerAngles.y, 0);
        Quaternion newRotation = Quaternion.LookRotation(new Vector3(movevector.x, 0f, movevector.z));
        playerModel.rotation = Quaternion.Slerp(playerModel.rotation, newRotation, turnSpeed * Time.deltaTime);
        Debug.Log($"Rotation is {playerModel.rotation.y}");
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
        else 
        {
            float yStore = movevector.y;
            inputX = Input.GetAxisRaw("Horizontal");
            inputY = Input.GetAxisRaw("Vertical");
            movevector = (transform.forward * inputY) + (transform.right * inputX);
            movevector = movevector.normalized * movespeed;
            movevector.y = yStore;
        }
        if (charcontr.isGrounded)
        {
            movevector.y = 0;
        }
        movespeed = Mathf.Clamp((Mathf.Abs(inputX) + Mathf.Abs(inputY)), 0, 1);
        if (movespeed > 0)
            isMoving = true;
        else
            isMoving = false;
        movevector.y = movevector.y + (Physics.gravity.y * gravity * Time.deltaTime);
        charcontr.Move(movevector * Time.deltaTime * speed);
        if (isMoving)
            RotateToCam();
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

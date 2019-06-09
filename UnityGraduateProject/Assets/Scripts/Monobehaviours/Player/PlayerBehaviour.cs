using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour, IDie
{
    [SerializeField] float speed;
    [HideInInspector] public float movespeed;
    [SerializeField] float gravity;
    [SerializeField] float turnSpeed;
    public float allowPlayerRotation;
    [SerializeField] float inBattleDist;
    [Tooltip("standard is ")] [SerializeField] float diffToTarget;
    [SerializeField] CharacterController charcontr;
    float inputX, inputY, verticalVel;
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
    bool isblocking, in_battle, isGrounded;

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
    Player_Animator_Controller animcontr;
    Actor actor;
    Transform playerModel;
    Camera cam;
    [SerializeField] Transform charRotTarget;
    public Vector3 movevector, desiredMoveDirection;
    Vector3 rotateVector;
    Quaternion playerRot;
    public GraduateAudio.AudioManager audioManager;

    public bool isMoving, blockRotationPlayer;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        charcontr = GetComponent<CharacterController>();
        playerModel = transform.Find("Model");
        animcontr = GetComponentInChildren<Player_Animator_Controller>();
        //charRotTarget = Camera.main.transform.GetChild(0);
        canMove = true;
        isblocking = false;
        inBattle = false;
        isMoving = false;
        actor = GetComponent<Actor>();
        audioManager = GraduateAudio.AudioManager.Instance;
        cam = Camera.main;
        ExM.SetActiveCursor(false);
    }

    private void Update()
    {
        isGrounded = charcontr.isGrounded;
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
        if (DialogueManager.Instance.inDialogue || HUD_Controller.Instance.inQuestMenu)
        {
            isMoving = false;
            movespeed = 0;
            return;
        }
        InputMagnitude();
        if (isGrounded)
        {
            movespeed = Mathf.Clamp(movespeed, 0, 1f);
            verticalVel -= 0;
            movevector = new Vector3(0, 0, movespeed);            
        }
        else
        {
            verticalVel -= 2;
        }        
        movevector.y = verticalVel;
        movevector = transform.TransformDirection(movevector * movespeed * speed * Time.deltaTime);
        charcontr.Move(movevector);
    }
    void InputMagnitude()
    {            
        inputX = Input.GetAxis("Horizontal");
        inputY = Input.GetAxis("Vertical");

        movespeed = new Vector2(inputX, inputY).sqrMagnitude;
        if (movespeed > allowPlayerRotation)
            PlayerMoveAndRotation();
    }
    void PlayerMoveAndRotation()
    {        
        var forward = cam.transform.forward;
        var right = cam.transform.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();
        

        desiredMoveDirection = forward * inputY + right * inputX;
        if (!blockRotationPlayer)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), turnSpeed);
        }
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
        UnityEngine.SceneManagement.SceneManager.LoadScene(3);
    }
}

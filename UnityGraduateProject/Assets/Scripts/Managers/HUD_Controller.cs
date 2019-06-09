using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using QuestSystem;

public class HUD_Controller : MonoBehaviour, Initializable
{
    const float STD_MESSAGE_TERM = 3f;
    GameObject hud;
    // ------ Info message
    GameObject infomessage_panel;
    Text infomessage_text;
    // ------ Inventory
    GameObject inventory_panel;
    GameObject weapon;
    // ------ Quest
    [SerializeField] List<Quest> quests;
    [SerializeField] GameObject quest_panel;
    [SerializeField] Transform[] b_quests; // array of quest buttons
    [SerializeField] Text[] t_quests; // array of quest titles
    [SerializeField] Text quest_description;
    const int MAX_ACTIVE_QUEST_AMOUNT = 3;
    public bool inQuestMenu;
    // ----- health
    Transform healthpanel;
    Slider health_slider;
    Actor player;
    PlayerBehaviour playerBehaviour;

    

    private static HUD_Controller _instance;
    public static HUD_Controller Instance => _instance;
    private void Awake()
    {
        _instance = this;
    }
    private void Start()
    {
        Initialize();
    }
    private void Update()
    {
        if (!player)
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Actor>();
        
        health_slider.value = player.Health;
    }
    public void Initialize()
    {
        hud = GameObject.Find("[UI]").transform.Find("HUD").gameObject;
        infomessage_panel = hud.transform.Find("InfoMessagePanel").gameObject;
        infomessage_text = infomessage_panel.GetComponentInChildren<Text>();
        infomessage_panel.SetActive(false);
        inventory_panel = hud.transform.Find("InventoryPanel").gameObject;
        weapon = inventory_panel.transform.Find("Weapon").gameObject;
        weapon.SetActive(false);
        // ---- Quests -----
        quest_panel = hud.transform.Find("QuestPanel").gameObject;
        quest_panel.SetActive(false);
        inQuestMenu = false;
        Transform temp_panel = quest_panel.transform.Find("buttons_panel");
        b_quests = new Transform[] { temp_panel.Find("b_quest_1"),
                                        temp_panel.Find("b_quest_2"),
                                        temp_panel.Find("b_quest_3") };
        t_quests = new Text[] { b_quests[0].GetComponentInChildren<Text>(),
                                b_quests[1].GetComponentInChildren<Text>(),
                                b_quests[0].GetComponentInChildren<Text>() };
        quest_description = quest_panel.transform.Find("active_quest_description").Find("description").GetComponent<Text>();
        // --------------- Health ----------------
        healthpanel = hud.transform.Find("HealthPanel");
        health_slider = healthpanel.Find("healthSlider").GetComponent<Slider>();
        player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Actor>();
        playerBehaviour = player.GetComponent<PlayerBehaviour>();
        health_slider.value = player?.Health ?? 100f;
    }
    #region Methods
    public void ShowInfoMessage(string message, float sec = STD_MESSAGE_TERM)
    {            
        infomessage_text.text = message;        
        StartCoroutine("WaitForSec", sec);        
    }
    public void StopInfoMessage()
    {
        StopCoroutine("WaitForSec");
        infomessage_panel.SetActive(false);
    }

    public void WeaponIsReady(bool isready)
    {
        weapon.SetActive(isready);
    }
    public void SetActiveHUD(bool isactive)
    {
        hud.SetActive(isactive);
    }

    // ----------------- Quest Methods -------------------
    public void ShowHideQuestPanel()
    {
        if (quest_panel.activeSelf)
        {
            inQuestMenu = false;
            quest_panel.SetActive(false);
            ExM.SetActiveCursor(false);
            Camera.main.transform.GetComponent<Cinemachine.CinemachineBrain>().enabled = true;
            return;
        }            
        quests = QuestManager.Instance.GetActiveQuests();
        if (quests == null || quests.Count >= MAX_ACTIVE_QUEST_AMOUNT)
        {
            return;
        }
        ExM.SetActiveGameObjects(b_quests, false);
        for (int i = 0; i < quests.Count; i++)
        {
            t_quests[i].text = quests[i].title;
            b_quests[i].gameObject.SetActive(true);
        }
        GetQuestDescription(0);
        inQuestMenu = true;
        quest_panel.SetActive(true);
        ExM.SetActiveCursor(true);
        Camera.main.transform.GetComponent<Cinemachine.CinemachineBrain>().enabled = false;
    }
    public void GetQuestDescription(int num)
    {
        quest_description.text = quests[num].description;
    }


    #endregion

    // Coroutines
    IEnumerator WaitForSec(float sec)
    {
        infomessage_panel.SetActive(true);
        yield return new WaitForSeconds(sec);
        infomessage_panel.SetActive(false);
    }
}


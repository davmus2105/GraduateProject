using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using QuestSystem;

public class HUD_Controller : MonoBehaviour
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
    List<Quest> quests;
    GameObject quest_panel;
    Transform[] b_quests; // array of quest buttons
    Text[] t_quests; // array of quest titles
    

    

    private static HUD_Controller _instance;
    public static HUD_Controller Instance => _instance;
    private void Awake()
    {
        _instance = this;
    }
    private void Start()
    {
        hud = GameObject.Find("[UI]").transform.Find("HUD").gameObject;
        infomessage_panel = hud.transform.Find("InfoMessagePanel").gameObject;
        infomessage_text = infomessage_panel.GetComponentInChildren<Text>();
        infomessage_panel.SetActive(false);
        inventory_panel = hud.transform.Find("InventoryPanel").gameObject;
        weapon = inventory_panel.transform.Find("Weapon").gameObject;
        weapon.SetActive(false);
    }
    #region Methods
    public void ShowInfoMessage(string message, float sec = STD_MESSAGE_TERM)
    {            
        infomessage_text.text = message;        
        StartCoroutine("WaitForSec", sec);        
    }

    public void WeaponIsReady(bool isready)
    {
        weapon.SetActive(isready);
    }

    public void ShowQuestPanel()
    {
        quests = QuestManager.Instance.GetActiveQuests();
        if (quests == null)
        {
            return;
        }

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


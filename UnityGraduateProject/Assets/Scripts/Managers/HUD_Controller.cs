﻿using System.Collections;
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
    [SerializeField] List<Quest> quests;
    [SerializeField] GameObject quest_panel;
    [SerializeField] Transform[] b_quests; // array of quest buttons
    [SerializeField] Text[] t_quests; // array of quest titles
    [SerializeField] Text quest_description;
    const int MAX_ACTIVE_QUEST_AMOUNT = 3;
    

    

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
        // ---- Quests -----
        quest_panel = hud.transform.Find("QuestPanel").gameObject;
        quest_panel.SetActive(false);
        Transform temp_panel = quest_panel.transform.Find("buttons_panel");
        b_quests = new Transform[] { temp_panel.Find("b_quest_1"),
                                     temp_panel.Find("b_quest_2"),
                                     temp_panel.Find("b_quest_3") };
        t_quests = new Text[] { b_quests[0].GetComponentInChildren<Text>(),
                                b_quests[1].GetComponentInChildren<Text>(),
                                b_quests[0].GetComponentInChildren<Text>() };
        quest_description = quest_panel.transform.Find("active_quest_description").Find("description").GetComponent<Text>();
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

    // ----------------- Quest Methods -------------------
    public void ShowHideQuestPanel()
    {
        if (quest_panel.activeSelf)
        {
            quest_panel.SetActive(false);
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
        quest_panel.SetActive(true);
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


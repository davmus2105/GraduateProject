using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestSystem
{
    public class QuestManager : MonoBehaviour
    {
        #region Fields and properties


        List<Quest> activequests;
        List<Quest> comletedquests;
        Dictionary<int, Quest> questdict; // here must be the list of all quests in game. Quests id is a key and the Quest is the value

        public static QuestManager Instance => _instance;
        private static QuestManager _instance;

        #endregion
        #region Monobehaviours methods    
        private void Awake()
        {
            _instance = this;
        }
        void Start()
        {

        }
        void Update()
        {

        }
        #endregion
        #region Methods
        public void AddQuest(int id)
        {
            if (activequests == null)
            {
                activequests = new List<Quest>();
            }

        }
        public void CompleteQuest()
        {

        }
        // --------------- private methods ---------------
        // FindQuest should find quest in dictionary and return its value
        void Clear()
        {
            int length = activequests.Count;
            for (int i = 0; i < length; i++)
            {
                if (!activequests[i].isActive)
                    activequests.RemoveAt(i);
            }
        }
        Quest FindQuest(int questid)
        {
            if (questid <= 0)
            {
                Debug.Log("<color=red>Error : </color> id of quest can't be negative or equal 0");
                return null;
            }
            Quest quest;
            if (!questdict.TryGetValue(questid, out quest))
                Debug.Log("<color=red>Error : </color> there is no quest with id [" + questid + "] in quest dictionary");
            return quest;
        }

        // ---- Quests dictionary methods ----
        void Load()
        {
            // Load using Load Method from QuestGenerator class
        }
        #endregion

    }
}


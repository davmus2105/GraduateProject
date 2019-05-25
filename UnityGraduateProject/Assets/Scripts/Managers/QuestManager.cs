using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestSystem
{
    public class QuestManager : MonoBehaviour
    {
        #region Fields and properties


        public List<Quest> activequests;
        List<int> completedquests; // store id of comleted quests
        Dictionary<int, Quest> questdict; // here must be the list of all quests in game. Quests id is a key and the Quest is the value

        public static QuestManager Instance => _instance;
        private static QuestManager _instance;

        #endregion
        #region Monobehaviours methods    
        private void Awake()
        {
            _instance = this;
        }        
        #endregion
        #region Methods
        public List<Quest> GetActiveQuests()
        {
            if (activequests == null || activequests.Count == 0)
                return null;
            return activequests;
        }
        public void AddQuest(int id)
        {
            Quest thisQuest, questindict;
            if (activequests == null)
            {
                activequests = new List<Quest>();
            }
            else if (activequests.Count >= 3)
            {
                // ERROR: You cant have more then 3 quests simultaniously
                HUD_Controller.Instance.ShowInfoMessage("You already have 3 quests");
                return;
            }
            if (questdict == null)
                Load();
            if (!questdict.TryGetValue(id, out questindict))
            {
                Debug.LogWarning("There no quest with id [" + id + "] in quest dictionary.");
                return;
            }
            if (activequests.Count > 0) // Check for containing quest to add in lists of active quests and comleted
            {                
                foreach(var quest in activequests)
                {
                    if (quest.id == id)                    
                        return;                    
                }
                if (completedquests == null)
                {
                    completedquests = new List<int>();
                }
                else
                {
                    foreach (var questid in completedquests)
                    {
                        if (questid == id && questindict.one_time)                           
                            return;                           
                    }
                }                
            }
            // Create a new object
            thisQuest = new Quest(_id: id, _title: questindict.title,
                                  _description: questindict.description,
                                  isactive: true, onetime: questindict.one_time);
            // goal 
            thisQuest.goal.goaltype = questindict.goal.goaltype;
            thisQuest.goal.idGoal = questindict.goal.idGoal;
            thisQuest.goal.requeredAmount = questindict.goal.requeredAmount;
            activequests.Add(thisQuest);
            if (thisQuest.goal.goaltype == GOALTYPE.Kill && thisQuest.goal.idGoal == 0)
            {
                EventManager.StartListening("EnemyDeath", thisQuest.IncrementGoal);
            }
        }
        public void CompleteQuest(int id)
        {
            if (completedquests == null)
                completedquests = new List<int>();
            if (activequests == null)
            {
                activequests = new List<Quest>();
                return;
            }            
            foreach (var quest in activequests)
            {
                if (quest.id == id)
                {
                    if (!completedquests.Contains(id))
                        completedquests.Add(id);
                    activequests.Remove(quest);
                    return;
                }
            }
        }
        // --------------- private methods ---------------
        void Clear()
        {
            int length = activequests.Count;
            for (int i = 0; i < length; i++)
            {
                if (!activequests[i].isActive)
                    activequests.RemoveAt(i);
            }
        }
        // FindQuest should find quest in dictionary and return its value
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
            List<Quest> quests = QuestGenerator.Instance.LoadQuestList();
            // Get new or clear dictionary for new keys and values
            if (questdict == null)
                questdict = new Dictionary<int, Quest>();
            else
                questdict.Clear();
            try
            {
                foreach (var quest in quests) // going through quest list
                {
                    questdict.Add(quest.id, quest);
                }
            }
            catch
            {
                Debug.LogWarning("ERROR: some error in the filling of the quest dictionary");
            }
            
        }
        #endregion
        #region Listeners of QuestManager        
        
        #endregion

    }
}


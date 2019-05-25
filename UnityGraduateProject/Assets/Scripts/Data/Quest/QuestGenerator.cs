using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;
namespace QuestSystem
{
    public class QuestGenerator : MonoBehaviour
    {
        //public Dictionary<int, Quest> questDict;
        public string filename = "quest_dictionary";
        public string _temp_lang_path_ = "English";
        public List<Quest> quests;

        public static QuestGenerator Instance => _instance ;
        private static QuestGenerator _instance;

        Quest quest;
        QuestGoal goal;

        private void Awake()
        {
            _instance = this;
        }

        public void Generate()
        {
            _temp_lang_path_ = LocalizationManager.Instance.Language;
            string path = Application.streamingAssetsPath + "/Localisation/" 
                          + _temp_lang_path_ + "/Quests/" + filename + ".xml";

            XmlNode usernode;
            XmlElement element;

            XmlDocument xmlDoc = new XmlDocument();
            XmlNode rootNode = xmlDoc.CreateElement("quests");
            XmlAttribute attribute;
            XmlText text;
            xmlDoc.AppendChild(rootNode);
            for (int i = 0; i < quests.Count; i++)
            {
                usernode = xmlDoc.CreateElement("quest");
                // attribute id
                attribute = xmlDoc.CreateAttribute("id");
                attribute.Value = quests[i].id.ToString();
                usernode.Attributes.Append(attribute);
                // attribute one-time
                if (quests[i].one_time)
                {
                    attribute = xmlDoc.CreateAttribute("one_time");
                    attribute.Value = quests[i].one_time.ToString();
                    usernode.Attributes.Append(attribute);
                }                
                // title element
                element = xmlDoc.CreateElement("title");
                text = xmlDoc.CreateTextNode(quests[i].title);
                element.AppendChild(text);
                usernode.AppendChild(element);
                // description element
                element = xmlDoc.CreateElement("description");
                text = xmlDoc.CreateTextNode(quests[i].description);
                element.AppendChild(text);
                usernode.AppendChild(element);                
                // goal element
                element = xmlDoc.CreateElement("goal");
                if (quests[i].goal.goaltype != GOALTYPE.None)
                    element.SetAttribute("goaltype", quests[i].goal.goaltype.ToString("g")); // goaltype attribute
                if (quests[i].goal.requeredAmount > 1) // required amount attribute
                    element.SetAttribute("required_amount", quests[i].goal.requeredAmount.ToString());
                if (quests[i].goal.idGoal > 0) // id of the goal attribute
                    element.SetAttribute("idgoal", quests[i].goal.idGoal.ToString());
                usernode.AppendChild(element);                
                rootNode.AppendChild(usernode);
            }
            xmlDoc.Save(path);
            Debug.Log(this + " Створено XML файл діалогу [" + filename + "] за адресою: " + path);
        }

        public void Load()
        {            
            quests = new List<Quest>();
            try //XML elements reading and loading attributes values to collections
            {
                string path = Application.streamingAssetsPath + "/Localisation/" 
                              + _temp_lang_path_ + "/Quests/" + filename + ".xml";
                Debug.Log($"language of quest is: {_temp_lang_path_}");
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(path);
                XmlElement xmlroot = xmlDoc.DocumentElement; // Reading root node in document
                foreach (XmlNode xnode in xmlroot)   // Reading all nodes 'quest'
                {
                    quest = new Quest();
                    if (xnode.Attributes.Count > 0) // Get attribute 'id' and 'one_time' in 'quest'
                    {
                        int tempint;
                        bool o_time;
                        XmlNode attr = xnode.Attributes.GetNamedItem("id");
                        if (attr != null && int.TryParse(attr.Value, out tempint))
                            quest.id = tempint;
                        attr = xnode.Attributes.GetNamedItem("one_time");
                        if (attr != null && bool.TryParse(attr.Value, out o_time))
                        {
                            quest.one_time = o_time;
                        }
                        else
                            quest.one_time = false;
                    }
                    foreach (XmlNode childnode in xnode.ChildNodes) // Get nodes 'goal'            
                    {
                        if (childnode.Name == "title")
                        {
                            quest.title = childnode.InnerText;
                        }
                        if (childnode.Name == "description")
                        {
                            quest.description = childnode.InnerText;
                        }
                        if (childnode.Name == "goal")
                        {
                            int tempint;  // Temp variable to store integer values
                            if (childnode.Attributes.Count > 0)         // Get attributes
                            {
                                XmlNode attr = childnode.Attributes.GetNamedItem("goaltype");       // 'goaltype'
                                if (attr != null)
                                {
                                    GOALTYPE type;
                                    try
                                    {
                                        type = (GOALTYPE)System.Enum.Parse(typeof(GOALTYPE), attr.Value);
                                    }
                                    catch
                                    {
                                        Debug.LogWarning("An error occurred while reading the goaltype attribute in the quest with id " + quest.id);
                                        type = GOALTYPE.None;
                                    }
                                    quest.goal.goaltype = type;
                                }
                                else
                                    quest.goal.goaltype = GOALTYPE.None;

                                attr = childnode.Attributes.GetNamedItem("required_amount");             // 'required amount'
                                if (attr != null && int.TryParse(attr.Value, out tempint) && tempint > 1)
                                    quest.goal.requeredAmount = tempint;
                                else quest.goal.requeredAmount = 1;

                                attr = childnode.Attributes.GetNamedItem("idgoal");        // 'idgoal'     
                                if (attr != null && int.TryParse(attr.Value, out tempint) && tempint > 0)
                                    quest.goal.idGoal = tempint;
                                else quest.goal.idGoal = 0;
                            }
                        }
                        
                    }
                    quests.Add(quest);                    
                }
            }
            catch (System.Exception error)
            {
                Debug.Log(this + " Error of dialogue file reading: " + filename + ".xml >> Error: " + error.Message);
            }
        }
        
        public List<Quest> LoadQuestList()
        {
            _temp_lang_path_ = LocalizationManager.Instance.Language;
            //if (quests == null)
            //    Load();
            //return quests;
            Load();
            return quests;
        }
    }

    [System.Serializable]
    public class Quest
    {
        public string title;
        public int id;
        public string description;
        public bool isActive;
        public bool one_time;
        public QuestGoal goal;

        public Quest(int _id = 0, string _title = "Default quest title", string _description = "Default description of the quest", bool isactive = false, bool onetime = false, QuestGoal qGoal = null)
        {
            id = _id;
            title = _title;
            description = _description;
            isActive = isactive;
            one_time = onetime;
            if (qGoal == null)
                goal = new QuestGoal(this);
            else
                goal = qGoal;
        }

        public void IncrementGoal()
        {
            if (goal.Reach())
            {
                QuestManager.Instance.CompleteQuest(id);
            }
        }
    }

    [System.Serializable]
    public class QuestGoal
    {        
        Quest parentquest;
        public GOALTYPE goaltype;
        public int currentAmount;        
        public int requeredAmount;
        public int idGoal; // id of character to interact with to obtain the goal (if 0, then there no special character) (this will be used to kill somebody, or to speak with somebody)
        
        bool CompareId(int id)
        {
            if (idGoal == 0 || id == idGoal)
                return true;
            else
                return false;
        }

        public bool Reach(int id = 0) // Increment currentamount and return true if goal is reached
        {
            if (!CompareId(id))
                return false;
            currentAmount++;
            if (currentAmount >= requeredAmount)
            {
                return true;
            }
            return false;
        }

        // -------- Constructors --------
        public QuestGoal(Quest _parentquest, GOALTYPE _goal = GOALTYPE.None, int _reqamount = 1, int _idgoal = 0)
        {
            parentquest = _parentquest;
            goaltype = GOALTYPE.None;
            currentAmount = 0;
            requeredAmount = _reqamount;
            idGoal = _idgoal;

        }
    }
    public enum GOALTYPE
    {
        None,
        Kill,
        Get,
        Speak,
        
    }
}


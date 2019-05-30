using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Xml;
using System.IO;
using QuestSystem;

[RequireComponent(typeof(ResultManager))]
public class DialogueManager : MonoBehaviour, Initializable
{
    #region Variables
    // -------------------- Objects for UI -----------------
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private RectTransform npctextPanel, answerPanel;
    [SerializeField] private Transform b_answ1, b_answ2, b_answ3, b_answ4; // Buttons with answer
    [SerializeField] private Text t_answ1, t_answ2, t_answ3, t_answ4; // text of answer buttons
    [SerializeField] private Text npctext;
    // ------------------ Script Variables -----------------
    public string currentlang = "English",  // Temp field
                   //file_name = "peasant_dialogue_01", // the name of the file that have to be written
                   lastfilename; // the name of the file that was used the last one
    public int chosenans; // chosen answer
    public bool inDialogue;
    public int rightResult; // Result of the dialogue
    bool isanswerchosed; // if answer is chosed then it is true
    // --------- Constants ---------
    public const int MAX_LETTERS_IN_NPCTEXT = 270;
    public const float SEC_WAIT_IN_DIALOGUE = 5f; // How long the npc text will be shown
    List<DialogueElement> dialogue;
    string currentDialogue;
    DialogueElement dialogue_el;
    Answer answer;
    Transform[] b_answers;
    Text[] t_answers;

    // ---- Instances ----
    HUD_Controller hudController;

    private static DialogueManager _instance;
    public static DialogueManager Instance => _instance;
    #endregion
    #region Monobehaviours methods
    private void Awake()
    {
        _instance = this;
    }
    private void Start()
    {
        Initialize();
    }
    public void Initialize()
    {
        lastfilename = string.Empty;
        dialoguePanel = GameObject.Find("[UI]").transform.Find("DialoguePanel").gameObject;
        npctextPanel = dialoguePanel.transform.Find("NPCtext_panel").GetComponent<RectTransform>();
        answerPanel = dialoguePanel.transform.Find("PlayerAnswers_panel").GetComponent<RectTransform>();
        // answer buttons transforms
        b_answ1 = answerPanel.transform.Find("Answer_1");
        b_answ2 = answerPanel.transform.Find("Answer_2");
        b_answ3 = answerPanel.transform.Find("Answer_3");
        b_answ4 = answerPanel.transform.Find("Answer_4");
        b_answers = new Transform[] { b_answ1, b_answ2, b_answ3, b_answ4 }; // Put answer buttons in array
        t_answers = new Text[] { t_answ1, t_answ2, t_answ3, t_answ4 };
        for (int i = 0; i <= 3; i++)
        {
            t_answers[i] = b_answers[i].GetComponentInChildren<Text>();
        }
        for (int i = 0; i <= 3; i++)
        {
            int param = i + 1;
            Button button = b_answers[i].GetComponent<Button>();
            button.onClick.AddListener(delegate { SetAnswer(param); });
        }
        npctext = npctextPanel.transform.Find("npcText").GetComponent<Text>();
        // ---- instances ----
        hudController = HUD_Controller.Instance;
    }
    #endregion
    #region DialogueManagers Methods
    public List<DialogueElement> Load(string filename) // Read xml file to get dialogs from it
    {
        currentlang = LocalizationManager.Instance.Language;
        currentDialogue = filename;
        string path = Application.streamingAssetsPath + "/Localisation/" + currentlang + "/Dialogues/" + filename + ".xml";
        if (!File.Exists(path))
        {
            return null;
        }
        if (currentlang + filename == lastfilename)
        {
            return dialogue;
        }
        dialogue = new List<DialogueElement>();
        try //XML elements reading and loading attributes values to collections
        {            
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            XmlElement xmlroot = xmlDoc.DocumentElement; // Reading root node in document
            foreach (XmlNode xnode in xmlroot)   // Reading all nodes 'node'
            {
                dialogue_el = new DialogueElement();
                if (xnode.Attributes.Count > 0) // Get attribute 'npcText' in 'node'
                {
                    XmlNode attr = xnode.Attributes.GetNamedItem("npcText");
                    if (attr != null)
                        dialogue_el.npcText = attr.Value;
                }
                foreach (XmlNode xmlanswer in xnode.ChildNodes) // Get nodes 'answer'            
                {
                    answer = new Answer();
                    int tempint;
                    bool exit;
                    if (xmlanswer.Attributes.Count > 0)         // Get attributes 'text' & 'toNode' | 'exit'
                    {
                        XmlNode attr = xmlanswer.Attributes.GetNamedItem("text");       // 'text'
                        if (attr != null)
                            answer.text = attr.Value;
                        attr = xmlanswer.Attributes.GetNamedItem("toNode");             // 'toNode'
                        if (attr != null && int.TryParse(attr.Value, out tempint))
                            answer.toNode = tempint;
                        else answer.toNode = 0;
                        attr = xmlanswer.Attributes.GetNamedItem("questid");             // 'toNode'
                        if (attr != null && int.TryParse(attr.Value, out tempint))
                            answer.questid = tempint;
                        else answer.questid = 0;
                        attr = xmlanswer.Attributes.GetNamedItem("value");               // 'exit'
                        if (attr != null && bool.TryParse(attr.Value, out exit))
                            answer.value = exit;
                        else answer.exit = false;
                        attr = xmlanswer.Attributes.GetNamedItem("exit");               // 'exit'
                        if (attr != null && bool.TryParse(attr.Value, out exit))
                            answer.exit = exit;
                        else answer.exit = false;
                    }
                    dialogue_el.answers.Add(answer);
                }
                dialogue.Add(dialogue_el);
            }
            return dialogue;
        }
        catch(System.Exception error)
        {
              Debug.Log(this + " Error of dialogue file reading: " + filename + ".xml >> Error: " + error.Message);
              lastfilename = string.Empty;
              return null;
        }
    }

    public void StartDialogue(string file_name, string language = "Ukrainian")
    {
        if (Load(file_name) == null)
            return;
        inDialogue = true;
        rightResult = 0;
        hudController.SetActiveHUD(false);
        SetActiveDialoguePanel(true);
        HideAllAnswers();          
        StartCoroutine("BuildDialogue", 0);
    }
    public void SetActiveDialoguePanel(bool state)
    {
        dialoguePanel.SetActive(state);
        if (state) // if active
        {
            ExM.SetActiveCursor(true);
            Camera.main.transform.GetComponent<Cinemachine.CinemachineBrain>().enabled = false;
        }
        else
        {
            ExM.SetActiveCursor(false);
            Camera.main.transform.GetComponent<Cinemachine.CinemachineBrain>().enabled = true;
        }
    }
    void EndDialogue()
    {
        ResultManager.Instance.SaveResult(currentDialogue, rightResult);
        SetActiveDialoguePanel(false);
        hudController.SetActiveHUD(true);
        inDialogue = false;
        StopCoroutine("BuildDialogue");
    }    
    public void GetAnswers(DialogueElement dial_el) // Show all answer buttons and to set text in them
    {
        int count = dial_el.answers.Count;
        for (int i = 0; i < count; i++)
        {
            t_answers[i].text = dial_el.answers[i].text;
            b_answers[i].gameObject.SetActive(true);
        }
    }
    void HideAllAnswers()
    {
        foreach (var but in b_answers)
            but.gameObject.SetActive(false);
    }
    public void SetAnswer(int num) // For buttons with answers to get its value
    {
        chosenans = num-1;
//      Debug.Log("answer " + num + "is chosed");
        isanswerchosed = true;
    }
    #endregion
    #region Coroutines
    IEnumerator BuildNpcText(DialogueElement dial_el)
    {
        string text = dial_el.npcText;
        int len = text.Length;
        if (len > MAX_LETTERS_IN_NPCTEXT)
        {
            int count = Mathf.FloorToInt(len / MAX_LETTERS_IN_NPCTEXT);
            // ----------- Splitting up ---------
            for (int i = 0; i < count; i++)
            {
                int pos = MAX_LETTERS_IN_NPCTEXT * (i+1); // Current position to place '/'
                if (text[pos] != ' ')
                {
                    for (int c = pos; c >= pos - 15; c--)
                    {
                        if (text[c] == ' ')
                        {
                            text = text.Remove(c, 1);
                            text = text.Insert(c, "@");
                            break;
                        }
                        else
                            continue;
                    }
                }
                else
                {
                    text = text.Remove(pos, 1);
                    text = text.Insert(pos, "@");
                }
            }
            string[] splittedText = text.Split('@');
            // ------------ Splitting up is ended --------------
            foreach(string sptext in splittedText)
            {
                npctext.text = sptext;
                yield return new WaitForSeconds(SEC_WAIT_IN_DIALOGUE);
            }            
        }
        else
        {
            npctext.text = dial_el.npcText;
        }
        yield return null;
    }
    IEnumerator BuildDialogue(int number) // Takes number of dialogue_element id in dialogue list
    {
        HideAllAnswers();
        isanswerchosed = false;
        StartCoroutine("BuildNpcText", dialogue[number]); // Show npc text
        GetAnswers(dialogue[number]);
        while (isanswerchosed == false)
        {
            yield return new WaitForEndOfFrame();
        }
        if (dialogue[number].answers[chosenans].questid != 0)
            QuestManager.Instance.AddQuest(dialogue[number].answers[chosenans].questid);
        if (dialogue[number].answers[chosenans].value)
        {
            rightResult++;
        }
        if (dialogue[number].answers[chosenans].exit)
        {
            EndDialogue();
        }
        else
            StartCoroutine("BuildDialogue", dialogue[number].answers[chosenans].toNode);
    }
    #endregion
}



public class DialogueElement
{
    public string npcText;
    public List<Answer> answers;
    public DialogueElement()
    {
        answers = new List<Answer>();
    }
}


public class Answer
{
    public string text;
    public int toNode;
    public int questid;
    public bool value;
    public bool exit;
}

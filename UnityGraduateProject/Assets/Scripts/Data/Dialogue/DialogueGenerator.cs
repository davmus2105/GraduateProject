using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

public class DialogueGenerator : MonoBehaviour
{
    public string filename = "peasant_dialogue_01";
    public string _temp_lang_path_ = "English";
    public List<DialogueNode> node;

    DialogueNode dialogue_el;
    PlayerAnswer answer;
    
    public void Generate()
    {
        string path = Application.streamingAssetsPath + "/Localisation/" + _temp_lang_path_ + "/Dialogues/" + filename + ".xml";

        XmlNode usernode;
        XmlElement element;

        XmlDocument xmlDoc = new XmlDocument();
        XmlNode rootNode = xmlDoc.CreateElement("dialogue");
        XmlAttribute attribute = xmlDoc.CreateAttribute("name");
        attribute.Value = filename;
        rootNode.Attributes.Append(attribute);
        xmlDoc.AppendChild(rootNode);
        for (int i = 0; i < node.Count; i++)
        {
            usernode = xmlDoc.CreateElement("node");
            attribute = xmlDoc.CreateAttribute("id");
            attribute.Value = i.ToString();
            usernode.Attributes.Append(attribute);
            attribute = xmlDoc.CreateAttribute("npcText");
            attribute.Value = node[i].npcText;
            usernode.Attributes.Append(attribute);

            for (int j = 0; j < node[i].playerAnswer.Count; j++)
            {
                element = xmlDoc.CreateElement("answer");
                element.SetAttribute("text", node[i].playerAnswer[j].text);
                if (node[i].playerAnswer[j].toNode > 0)
                    element.SetAttribute("toNode", node[i].playerAnswer[j].toNode.ToString());
                if (node[i].playerAnswer[j].questid > 0)
                    element.SetAttribute("questid", node[i].playerAnswer[j].questid.ToString());
                if (node[i].playerAnswer[j].value)
                    element.SetAttribute("value", node[i].playerAnswer[j].value.ToString());
                if (node[i].playerAnswer[j].exit)
                    element.SetAttribute("exit", node[i].playerAnswer[j].exit.ToString());
                usernode.AppendChild(element);
            }
            rootNode.AppendChild(usernode);
        }
        xmlDoc.Save(path);
        Debug.Log(this + " Створено XML файл діалогу [" + filename + "] за адресою: " + path);
    }
    public void LoadDialogue()
    {    
        node = new List<DialogueNode>();
        try //XML elements reading and loading attributes values to collections
        {
            string path = Application.streamingAssetsPath + "/Localisation/" + _temp_lang_path_ + "/Dialogues/" + filename + ".xml";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            XmlElement xmlroot = xmlDoc.DocumentElement; // Reading root node in document
            foreach (XmlNode xnode in xmlroot)   // Reading all nodes 'node'
            {
                dialogue_el = new DialogueNode();
                if (xnode.Attributes.Count > 0) // Get attribute 'npcText' in 'node'
                {
                    XmlNode attr = xnode.Attributes.GetNamedItem("npcText");
                    if (attr != null)
                        dialogue_el.npcText = attr.Value;
                }
                foreach (XmlNode xmlanswer in xnode.ChildNodes) // Get nodes 'answer'            
                {
                    answer = new PlayerAnswer();
                    int tempint;  // Temp variable to store toNode and questid values
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
                        else answer.value = false;
                        attr = xmlanswer.Attributes.GetNamedItem("exit");               // 'exit'
                        if (attr != null && bool.TryParse(attr.Value, out exit))
                            answer.exit = exit;
                        else answer.exit = false;
                    }
                    dialogue_el.playerAnswer.Add(answer);
                }
                node.Add(dialogue_el);
            }
        }
        catch (System.Exception error)
        {
            Debug.Log(this + " Error of dialogue file reading: " + filename + ".xml >> Error: " + error.Message);
        }
    }
}

[System.Serializable]
public class DialogueNode
{
    [Tooltip("Text the npc says")]
    public string npcText;
    public int node_id;
    public List<PlayerAnswer> playerAnswer;    
    public DialogueNode()
    {
        playerAnswer = new List<PlayerAnswer>();
    }
}

[System.Serializable]
public class PlayerAnswer
{    
    [Tooltip("The text of the answer")]
    public string text;
    [Tooltip("The next node id")]
    public int toNode;
    [Tooltip("If the answer gives a quest, then the field store its id")]
    public int questid;
    [Tooltip("If the answer is right then value is equal to true, and if it is not - false")]
    public bool value;
    [Tooltip("If this answer reach the end of the dialogue, then this value is true")]
    public bool exit;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGenerator : MonoBehaviour
{
    
}

[System.Serializable]
public class Quest
{
    int id;
    string title;
    string description;
    QuestGoal goal;
}

[System.Serializable]
public class QuestGoal
{
    GOALTYPE goaltype;

}
public enum GOALTYPE
{
    Kill,
    Get,
    Speak,
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace QuestSystem
{
    public class QuestGenerator : MonoBehaviour
    {

    }

    [System.Serializable]
    public class Quest
    {
        int id;
        string title;
        string description;
        public bool isActive;
        QuestGoal goal;
    }

    [System.Serializable]
    public class QuestGoal
    {
        GOALTYPE goaltype;
        int currentAmount;        
        int requeredAmount;
        int idGoal; // id of character to interact with to obtain the goal (if 0, then there no special character) (this will be used to kill somebody, or to speak with somebody)
        
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
    }
    public enum GOALTYPE
    {
        Kill,
        Get,
        Speak,
    }
}


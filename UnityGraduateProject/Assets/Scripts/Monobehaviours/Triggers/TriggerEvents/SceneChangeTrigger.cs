using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TES
{
    public class SceneChangeTrigger : TriggerEventComponent
    {
        public int requiredamount;
        public override void ExecuteEvent(Collider collider = null, TriggerEventContainer container = null)
        {
            int result = DialogueManager.Instance.rightResult;
            if (result == requiredamount)
                ChangeScene();
            else
            {
                Debug.Log("You have not completed your quest");
            }
        }
        void ChangeScene()
        {
            //QuestSystem.QuestManager.Instance.CompleteQuest(#quest id);
            // TODO:
            // The way that scene will be changed
            Debug.Log("Your way is finished");
        }
    }
}
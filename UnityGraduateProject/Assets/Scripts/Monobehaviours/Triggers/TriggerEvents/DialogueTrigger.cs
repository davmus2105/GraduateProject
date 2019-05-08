using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TES
{
    public class DialogueTrigger : TriggerEventComponent
    {
        public override void ExecuteEvent(Collider collider = null)
        {
            if (collider.tag != "Player")
            {
                Debug.Log("Player had being entered in dialogue trigger");
                DialogueManager.Instance.StartDialogue("peasant_dialogue_01");
            }
        }
    }
}

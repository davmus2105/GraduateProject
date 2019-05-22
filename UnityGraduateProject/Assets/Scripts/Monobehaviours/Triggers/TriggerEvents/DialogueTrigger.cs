using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TES
{
    [CreateAssetMenu(fileName="DialogueTriggerComponent", menuName="TES/Components/DialogueTrigger")]
    public class DialogueTrigger : TriggerEventComponent
    {
        public string dialogue_name;
        public bool isUsed = false;

        public override void ExecuteEvent(Collider collider = null, TriggerEventContainer container = null)
        {
            if (isUsed)
                return;
            if (collider.tag == "Player")
            {
                DialogueManager.Instance.StartDialogue(dialogue_name);
                isUsed = true;
            }
        }
    }
}

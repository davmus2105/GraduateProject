﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TES
{
    [CreateAssetMenu(fileName="DialogueTriggerComponent", menuName="TES/Components/DialogueTrigger")]
    public class DialogueTrigger : TriggerEventComponent
    {
        public string dialogue_name;        

        public override void ExecuteEvent(Collider collider = null, TriggerEventContainer container = null)
        {
            if (collider.tag == "Player")
            {
                DialogueManager.Instance.StartDialogue(dialogue_name);
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag != "Player")
        {
            Debug.Log("Player had being entered in dialogue trigger");
            DialogueManager.Instance.StartDialogue("peasant_dialogue_01");
        }        
    }
}

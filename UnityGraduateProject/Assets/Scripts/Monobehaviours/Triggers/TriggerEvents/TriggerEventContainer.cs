using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TES // TES - Trigger Event System
{
    public class TriggerEventContainer : MonoBehaviour
    {
        public List<TriggerEventComponent> onTriggerEnter, 
                                           onTriggerStay, 
                                           onTriggerExit;
        public GameObject host;


        private void Start()
        {
            host = transform.parent.gameObject;
            IsUsedUpdate(onTriggerEnter);
        }

        void IsUsedUpdate(List<TriggerEventComponent> list)
        {
            foreach (var item in list)
            {
                if (item as DialogueTrigger != null)
                    ((DialogueTrigger)item).isUsed = false;
            }
        }

        void ExecuteComponents(List<TriggerEventComponent> list, Collider collider)
        {
            if (list != null)
            {
                foreach (var component in list)
                {
                    component.ExecuteEvent(collider, this);
                }
            }           
        }

        bool ConfirmationCheck()
        {
            if (Input.GetButton("Confirm"))
                return true;
            else
                return false;
        }

        private void OnTriggerEnter(Collider col)
        {
            ExecuteComponents(onTriggerEnter, col);
        }
        private void OnTriggerStay(Collider other)
        {
            StartCoroutine(WaitForConfirmation(onTriggerStay, other));
            ExecuteComponents(onTriggerStay, other);
        }

        IEnumerator WaitForConfirmation(List<TriggerEventComponent> list, Collider col)
        {
            // send message on screen that you can press button "E"
            while (!ConfirmationCheck())
            {
                yield return new WaitForEndOfFrame();
            }                         
            ExecuteComponents(list, col);                
        }
        
    }
}

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

        private void OnTriggerEnter(Collider col)
        {
            ExecuteComponents(onTriggerEnter, col);
        }
    }
}

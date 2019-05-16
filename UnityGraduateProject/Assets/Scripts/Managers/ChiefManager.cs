using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChiefManager : MonoBehaviour
{
    [Tooltip("Managers that will be added on start")]
    // managers to add on [SETUP] when game is started
    public List<string> managerTypes;
    private void Awake()
    {
        if (managerTypes == null)
            managerTypes = new List<string>();
    }
    void Start()
    {
        if (managerTypes.Count > 0)
        {
            List<MonoBehaviour> managerexists = new List<MonoBehaviour>();
            var existComp = gameObject.GetComponents<MonoBehaviour>();
            foreach (var manager in managerTypes)
            {
                var component = System.Type.GetType(manager);
                gameObject.AddComponent(component);
            }
        }

    }

    
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    #region Fields an Properties
    private Dictionary<string, UnityEvent> eventDictionary;

    private static EventManager _instance;
    public static EventManager Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(EventManager)) as EventManager;
                if (!_instance)
                {
                    Debug.LogError("There needs to be one active EventManager script on Gameobject in your scene.");
                }
                else
                {
                    _instance.Init();
                }
            }
            return _instance;
        }
    }
    #endregion
    #region Methods
    void Init()
    {
        if(eventDictionary == null)
        {
            eventDictionary = new Dictionary<string, UnityEvent>();
        }
    }
    public static void StartListening(string eventname, UnityAction listener)
    {
        UnityEvent thisEvent = null;
        if (Instance.eventDictionary.TryGetValue(eventname, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(listener);
            Instance.eventDictionary.Add(eventname, thisEvent);
        }
    }
    public static void StopListening(string eventname, UnityAction listener)
    {
        if (_instance == null) return;
        UnityEvent thisEvent = null;
        if (Instance.eventDictionary.TryGetValue(eventname, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }
    public static void TriggerEvent(string eventname)
    {
        UnityEvent thisEvent = null;
        if (Instance.eventDictionary.TryGetValue(eventname, out thisEvent))
        {
            thisEvent.Invoke();
        }
    }
    #endregion
}

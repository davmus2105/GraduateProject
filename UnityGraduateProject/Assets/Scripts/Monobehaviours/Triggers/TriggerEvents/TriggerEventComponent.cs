using UnityEngine;

namespace TES
{
    [System.Serializable]
    public abstract class TriggerEventComponent : ScriptableObject
    {
        public abstract void ExecuteEvent(Collider collider = null, TriggerEventContainer container = null);
    }
}

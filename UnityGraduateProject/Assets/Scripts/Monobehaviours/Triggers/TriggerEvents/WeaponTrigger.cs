using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TES {
    [CreateAssetMenu(fileName="WeaponTriggerComponent", menuName="TES/Components/WeaponTrigger")]
    public class WeaponTrigger : TriggerEventComponent
    {
        [SerializeField] float damage;
        List<string> tagsToDamage = new List<string>{ "Player", "Enemy" };

        public override void ExecuteEvent(Collider collider)
        {
            if (tagsToDamage.Contains(collider.tag))
            {
                collider.GetComponent<Actor>().GetDamage(damage);
            }
        }
    }
}
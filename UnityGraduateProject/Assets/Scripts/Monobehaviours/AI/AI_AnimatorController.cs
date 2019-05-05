using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class AI_AnimatorController : MonoBehaviour
    {
        #region Fields and properties
        AI_CharacterController aicontroller;
        Animator animator;
        GameObject inhandplace;

        // Hashes for animator states
        int speed_hash = Animator.StringToHash("speed");
        int arm = Animator.StringToHash("arm_disarm");
        int slash = Animator.StringToHash("slash");
        #endregion
        #region Monobehaviours methods
        void Start()
        {
            aicontroller = GetComponent<AI_CharacterController>();
            if (!aicontroller)
                aicontroller = gameObject.AddComponent<AI_CharacterController>();
            animator = GetComponent<Animator>();
            inhandplace = transform.Find("Armature").Find("Hips").Find("LowerSpine").
                      Find("Chest").Find("ShoulderConnector.R").Find("Shoulder.R").
                      Find("UpperArm.R").Find("LowerArm.R").
                      Find("Hand.R").Find("WeaponInHandPlace").gameObject;
        }        
        #endregion
        #region Methods
        // Common animation behaviours
        public void Move(float _speed)
        {
            // Moving animation
            animator.SetFloat(speed_hash, _speed);
        }

        public void Death()
        {
            // Death animation consist of:
            // 1. Coroutine "DeathAnimation" and after a few seconds to call destroing
            // 2. Destroing (put to pool the object)
        }

        // ---------- Peasant animations ----------
        public void Toiling() // Animation of work in the field
        {

        }

        // ---------- Martial Animations ----------
        void Slash()
        {
            animator.SetTrigger(slash); // slash animation
        }
        #endregion
    }
}
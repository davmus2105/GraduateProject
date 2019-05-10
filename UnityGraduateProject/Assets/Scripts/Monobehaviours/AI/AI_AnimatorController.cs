using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class AI_AnimatorController : MonoBehaviour
    {
        #region Fields and properties
        AI_CharacterController aicontroller;
        AI_BaseBehaviour ai_controller;
        Animator animator;
        GameObject inhandplace;

        const float AFTER_DEATH_WAIT = 6f;

        // Hashes for animator states
        int speed_hash = Animator.StringToHash("speed");
        int arm = Animator.StringToHash("arm");
        int disarm = Animator.StringToHash("disarm");
        int slash = Animator.StringToHash("slash");
        int die = Animator.StringToHash("die");

        public bool isArmored;
        #endregion
        #region Monobehaviours methods
        void Start()
        {
            ai_controller = GetComponentInParent<AI_BaseBehaviour>();
            if (!ai_controller)
                ai_controller = gameObject.AddComponent<AI_BaseBehaviour>();
            animator = GetComponent<Animator>();
            inhandplace = transform.Find("Armature").Find("Hips").Find("LowerSpine").
                      Find("Chest").Find("ShoulderConnector.R").Find("Shoulder.R").
                      Find("UpperArm.R").Find("LowerArm.R").
                      Find("Hand.R").Find("WeaponInHandPlace").gameObject;

            isArmored = false;
        }
        #endregion
        private void Update()
        {
            Move(ai_controller.agent.speed);
        }
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
            StartCoroutine("DeathAnimation");
        }

        // ---------- Peasant animations ----------
        public void Toiling() // Animation of work in the field
        {

        }

        // ---------- Martial Animations ----------
        public void Slash()
        {
            animator.SetTrigger(slash); // slash animation
        }

        public void Arm()
        {
            isArmored = true;
            animator.SetTrigger(arm);
        }
        public void Disarm()
        {
            isArmored = false;
            animator.SetTrigger(disarm);
        }
        void EquipWeapon()
        {
            inhandplace.SetActive(true);
        }
        void HideWeapon()
        {
            inhandplace.SetActive(false);
        }

        // --------- Animations Events ----------
        void SetCanMoveTrue()
        {
            ai_controller.canMove = true;
        }
        void SetCanMoveFalse()
        {
            ai_controller.canMove = false;
        }
        void PlayFootstepSound()
        {
            // Make audiomanager and add to it footsteps
        }
        #endregion
        IEnumerable DeathAnimation()
        {
            animator.SetTrigger(die);
            yield return new WaitForSeconds(AFTER_DEATH_WAIT);
            // Call method Death() from ai_controller
            ai_controller.Death();
        }
    }
}
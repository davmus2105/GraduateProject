using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

namespace AI {
    public abstract class AI_BaseBehaviour : MonoBehaviour, IDie
    {
        #region Fields and Constructors
        // ---- public fields ----        
        public bool canMove;

        // ---- protected fields ----    
        protected Actor actor; // Actor component for this ai
        protected Transform target; // player transform target
        protected AI_AnimatorController animator;
        protected NavMeshAgent agent;

        // ---- Constructors ----       
        public AI_BaseBehaviour()
        {
            target = GameObject.FindGameObjectWithTag("Player").transform.Find("Model");
            ai_count++;
        }
        #endregion
        #region Static
        private static int ai_count;
        public static int GetAICount => ai_count;

        #endregion
        #region Monobehaviour methods
        private void Start()
        {
            canMove = true;
        }
        private void Update()
        {
            AiBehaviour();
        }
        private void OnEnable()
        {
            ai_count++;
        }
        private void OnDisable()
        {
            ai_count--;
        }
        #endregion
        #region Methods
        protected abstract void AiBehaviour();
        public abstract void Die();
        #endregion
    }

    // Class for work with all ai
    // public class AI_ToolBox
}
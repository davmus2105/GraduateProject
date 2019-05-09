using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

namespace AI {
    public abstract class AI_BaseBehaviour : MonoBehaviour
    {
        #region Fields and Constructors
        // ---- public fields ----        


        // ---- protected fields ----    
        protected Actor actor; // Actor component for this ai
        protected Transform target; // player transform target
        protected AI_AnimatorController animator;
        protected NavMeshAgent agent;

        // ---- Constructors ----       
        public AI_BaseBehaviour(int id, string charname = "")
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
        #endregion
    }

    // Class for work with all ai
    // public class AI_ToolBox
}
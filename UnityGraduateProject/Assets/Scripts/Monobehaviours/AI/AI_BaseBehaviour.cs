using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI {
    public abstract class AI_BaseBehaviour : MonoBehaviour
    {
        #region Fields and Constructors
        // ---- public fields ----
        public int Id => ai_id;
        public float damage;
        public string char_name;
        public string default_name = "Vova";

        // ---- protected fields ----
        protected int ai_id;
        protected Transform target; // player transform target
        protected AI_AnimatorController animator;

        // ---- Constructors ----       
        public AI_BaseBehaviour(int id)
        {
            ai_id = id;
            target = GameObject.FindGameObjectWithTag("Player").transform.Find("Model");
            ai_count++;
        }
        #endregion
        #region Static
        private static int ai_count;
        public static int GetAICount => ai_count;

        #endregion
        #region Monobehaviour methods
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        #endregion
        #region Methods
        
        #endregion
    }
}
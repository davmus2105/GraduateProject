using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

namespace AI {
    public abstract class AI_BaseBehaviour : MonoBehaviour, IDie
    {
        #region Fields and Constructors               
        // fields for moving
        public bool canMove;
        [SerializeField] protected float movSpeed;
        [SerializeField] protected float stoppingDistance;
        protected Vector3 destinationPoint;       
        


        // ---- instances fields ----    
        protected Actor actor; // Actor component for this ai
        protected Transform target; // player transform target
        protected AI_AnimatorController animator;    
        
        public NavMeshAgent agent;

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
            destinationPoint = transform.position;
            agent = GetComponent<NavMeshAgent>();
            agent.speed = movSpeed;
            agent.stoppingDistance = stoppingDistance;
        }
        private void Update()
        {
            AiBehaviour();
            Move();
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
        protected void Move()
        {
            if (canMove)
            {
                agent.SetDestination(destinationPoint);                
            }            
        }
        public void Die()
        {
            canMove = false;
            animator.Death();
        }
        public virtual void Death()
        {
            // 1. Make an event of death.
            // 2. Pooling the AI.
        }
        #endregion
    }

    // Class for work with all ai
    // public class AI_ToolBox
}
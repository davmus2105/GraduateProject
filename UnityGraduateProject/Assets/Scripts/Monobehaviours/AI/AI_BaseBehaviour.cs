﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

namespace AI {
    public abstract class AI_BaseBehaviour : MonoBehaviour, IDie
    {
        #region Fields and Constructors               
        // fields for moving
        public bool canMove;
        [SerializeField] protected float runSpeed;
        [SerializeField] protected float walkSpeed;
        [SerializeField] protected float walkDistance;
        [SerializeField] protected float stoppingDistance;
        [SerializeField] protected float targetDistance;
        protected Vector3 destinationPoint;
        
        


        // ---- instances fields ----    
        protected Actor actor; // Actor component for this ai
        protected Transform target; // player transform target
        protected AI_AnimatorController animator;    
        
        public NavMeshAgent agent;

        // ---- Constructors ----       
        public AI_BaseBehaviour()
        {
            
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
            animator = GetComponentInChildren<AI_AnimatorController>();
            agent = GetComponent<NavMeshAgent>();
            target = GameObject.FindGameObjectWithTag("Player").transform;
            agent.speed = runSpeed;
            agent.stoppingDistance = stoppingDistance;
        }
        private void Update()
        {
            AiBehaviour();
            SpeedRegulation(agent.remainingDistance);
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
        protected void SpeedRegulation(float Distance)
        {
            var smoothSpeedVelocity = (1f - Distance * Time.deltaTime) * 0.1f;
            if (Distance <= stoppingDistance)
            {
                agent.speed = Mathf.Lerp(agent.speed, 0f, smoothSpeedVelocity);

            }
            else if (Distance <= walkDistance)
            {
                agent.speed = Mathf.Lerp(agent.speed, walkSpeed, smoothSpeedVelocity);

            }
            else if (Distance > walkDistance)
            {
                agent.speed = Mathf.Lerp(agent.speed, runSpeed, smoothSpeedVelocity);
            }
            if (Distance <= 0.01f)
            {
                agent.speed = 0f;
            }
        }
        public void Die()
        {
            canMove = false;
            animator.Death();
        }
        public virtual void Death()
        {
            // 1. Call an event of death if this is an enemy (In EventManager)            
            // 2. Pooling the AI.
        }
        #endregion
    }

    // Class for work with all ai
    // public class AI_ToolBox
}
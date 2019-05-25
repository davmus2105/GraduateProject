using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI {
    public class AI_PeasantBehaviour : AI_BaseBehaviour
    {
        public bool isUnderAttack;
        

        [SerializeField] float dangerRadius;
        float enemyValue;
        [SerializeField] Vector3 dirToFlee;

        [SerializeField] List<Transform> huntingEnemies;

        private void OnEnable()
        {
            AI_Manager.Instance?.AddPeasant(this);
        }
        private void OnDisable()
        {
            AI_Manager.Instance.RemovePeasant(this);
        }

        protected override void Start()
        {
            base.Start();
            isUnderAttack = false;
            huntingEnemies = new List<Transform>();
        }

        protected override void AiBehaviour()
        {
            if (isUnderAttack)
            {
                Escape();
            }
            else
            {
                Idle();
            }
            BehaviourSelect();
            
        }        

        public override void Death()
        {
            PoolManager.Instance.PoolObj("peasants", gameObject);
        }

        void BehaviourSelect()
        {
            // fill out the hunting enemies list
            EnemiesSearch();
            if (huntingEnemies != null && huntingEnemies.Count > 0)
                isUnderAttack = true;
            else
                isUnderAttack = false;
            
        }

        void EnemiesSearch()
        {
            // Find all enemies in danger radius
            huntingEnemies = AI_Manager.Instance.FindEnemiesFromPoint(transform.position, dangerRadius, out enemyValue); 
        }

        void Escape()
        {
            // 1) Find direction of enemy or enemies 
            // 2) calculate their average value
            // 3) inverse it
            foreach(var enemy in huntingEnemies)
            {
                dirToFlee += transform.position - enemy.position;
            }
            dirToFlee /= huntingEnemies.Count;
            var distance = dirToFlee.magnitude;
            dirToFlee = dirToFlee / distance;
            agent.SetDestination(dirToFlee * enemyValue);
        }
        void Idle()
        {
            agent.SetDestination(transform.position);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI {
    public class AI_PeasantBehaviour : AI_BaseBehaviour
    {
        bool isUnderAttack;

        float dangerRadius;

        List<Transform> huntingEnemies;

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
            if (huntingEnemies.Count > 0)            
                isUnderAttack = true;           
        }

        void EnemiesSearch()
        {
            // Find all enemies in danger radius
            
        }

        void Escape()
        {
            // 1) Find direction of enemy or enemies 
            // 2) calculate their average value
            // 3) inverse it
        }
    }
}
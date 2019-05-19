using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class AI_Manager : MonoBehaviour
    {
        public List<AI_EnemyBehaviour> enemies;
        public List<AI_PeasantBehaviour> peasants;

        public static AI_Manager Instance => instance;
        private static AI_Manager instance;

        private void Awake()
        {
            instance = this;
            enemies = new List<AI_EnemyBehaviour>();
            peasants = new List<AI_PeasantBehaviour>();
        }
        
        public bool AddEnemy(AI_EnemyBehaviour enemy)
        {
            if (enemy == null)
                return false;
            enemies.Add(enemy);
            return true;
        }
        public bool RemoveEnemy(AI_EnemyBehaviour enemy)
        {
            if (enemy == null)
                return false;
            enemies.Remove(enemy);
            return true;
        }
        public bool AddPeasant(AI_PeasantBehaviour peasant)
        {
            if (peasant == null)
                return false;
            peasants.Add(peasant);
            return true;
        }
        public bool RemovePeasant(AI_PeasantBehaviour peasant)
        {
            if (peasant == null)
                return false;
            peasants.Remove(peasant);
            return true;
        }

        public bool IsEnemyNear(Vector3 point, float distance)
        {
            if (enemies == null || enemies.Count == 0)
                return false;
            foreach (var enemy in enemies)
            {
                if (Vector3.Distance(point, enemy.transform.position) < distance)
                {
                    return true;                    
                }
            }
            return false;
        }
        public List<Transform> FindEnemiesFromPoint(Vector3 point, float distance, out float enemyValue)
        {
            if (enemies == null || enemies.Count == 0)
            {
                enemyValue = 0;
                return null;
            }                
            float tmpDist;
            enemyValue = distance;
            List<Transform> listToReturn = new List<Transform>();
            foreach (var enemy in enemies)
            {
                tmpDist = Vector3.Distance(point, enemy.transform.position);
                if (tmpDist < distance)
                {
                    listToReturn.Add(enemy.transform);
                    if (tmpDist < enemyValue)
                        enemyValue = tmpDist;
                }
            }
            enemyValue = 5 - enemyValue;
            return listToReturn;
        }
        public Transform FindNearestPeasant(Vector3 point, float distance)
        {
            if (peasants == null || peasants.Count == 0)
                return null;

            float nearestDist = Vector3.Distance(peasants[0].transform.position, point);
            float tempDist;
            Transform nearestPeasant = peasants[0].transform;
            for (int i = 1; i < peasants.Count; i++)
            {
                tempDist = Vector3.Distance(point, peasants[i].transform.position);
                if (tempDist < nearestDist)
                {
                    nearestDist = tempDist;
                    nearestPeasant = peasants[i].transform;
                }
            }
            if (nearestDist > distance)
                return null;
            return nearestPeasant;
        }
    }
}
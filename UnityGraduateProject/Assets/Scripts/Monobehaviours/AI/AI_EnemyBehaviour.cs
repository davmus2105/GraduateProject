using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI {
    public class AI_EnemyBehaviour : AI_BaseBehaviour
    {
        #region Fields and Constructors
        [SerializeField] float huntDistance;
        [SerializeField] float attackDistance;
        [SerializeField] bool isAttacking;
        Transform peasant;
        #endregion
        #region MonoBehaviours methods
        private void OnEnable()
        {
            AI_Manager.Instance.AddEnemy(this);
        }
        private void OnDisable()
        {
            AI_Manager.Instance.RemoveEnemy(this);
        }
        #endregion
        #region Methods
        public override void Death()
        {
            EventManager.TriggerEvent("EnemyDeath");
            PoolManager.Instance.PoolObj("enemies", gameObject);
        }
        protected override void AiBehaviour()
        {
            ChooseTarget();
            targetDistance = Vector3.Distance(target.transform.position, transform.position);
            if (targetDistance < huntDistance)
            {
                isAttacking = true;
                animator.Arm();
                if (targetDistance <= attackDistance)
                {
                    agent.SetDestination(transform.position);
                    //CanMove = false;
                    if (canMove)
                        Attack();
                }
                else
                {
                    //CanMove = true;
                    agent.SetDestination(target.transform.position);
                }

                if (!canMove)
                {
                    agent.updateRotation = false;
                    Vector3 rotToPlayer = target.transform.position - transform.position;
                    rotToPlayer.y = 0;
                    transform.rotation = Quaternion.LookRotation(rotToPlayer, transform.up);
                    agent.updateRotation = true;
                }

            }
            else
            {
                isAttacking = false;
            }
            agent.isStopped = !this.canMove;
        }

        void Attack()
        {
            canMove = false; // Block moving to slash the target
            agent.isStopped = true;
            animator.Slash();
        }

        void Hunt()
        {
            agent.isStopped = false;
            destinationPoint = target.position;            
        }    
        void ChooseTarget()
        {
            FindPeasant();
            if (peasant != null && Vector3.Distance(transform.position, player.position) > huntDistance)
            {
                target = peasant;
            }
            else
                target = player;
        }
        void FindPeasant()
        {
            peasant = AI_Manager.Instance.FindNearestPeasant(transform.position, huntDistance);
        }
        void Idle()
        {
            
        }

        #endregion
    }
}
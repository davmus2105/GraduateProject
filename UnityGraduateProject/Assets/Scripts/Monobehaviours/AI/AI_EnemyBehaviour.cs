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
        #endregion
        #region MonoBehaviours methods

        #endregion
        #region Methods
        public override void Death()
        {
            EventManager.TriggerEvent("EnemyDeath");
            PoolManager.Instance.PoolObj("enemies", gameObject);
        }
        protected override void AiBehaviour()
        {
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
        
        void Idle()
        {
            
        }

        #endregion
    }
}
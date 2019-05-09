using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    public class AI_CharacterController : MonoBehaviour
    {
        #region Fields and properties and constructors
        public float damage;
        public string char_name;
        public string default_name = "Vova";
        public AI_TYPE ai_type;

        // ----------- AI controll fields -----------
        private NavMeshAgent agent;

        // fields for targeting
        [SerializeField] private Transform target;

        // fields for moving
        [SerializeField] private float movSpeed;

        private Vector3 destinationPoint;


        [SerializeField] private bool canMove;

        // ------------------------------------------

        private int id;

        delegate void AI_Behaviour(); // delegate for behaviour methods of current ai
        AI_Behaviour ai_behaviour;
        // --- Instances to work with ---
        Actor actor;
        AI_AnimatorController anim_contr;

        // ----- Constructors -----
        public AI_CharacterController(int _id, AI_TYPE _ai_type, string _name = null)
        {
            id = _id;
            ai_type = _ai_type;
            AIBehaviourSelect(ai_type);
            if (string.IsNullOrWhiteSpace(_name))
                char_name = default_name;
            else
                char_name = _name;
        }
        #endregion
        #region Monobehaviours methods
        void Start()
        {
            // --------------------- Instances Init ----------------------
            // initialize actor components and if it is not present in gameObject then add it
            actor = gameObject.GetComponent<Actor>();
            if (actor == null)
                actor = gameObject.AddComponent<Actor>();
            // the same with animator that I have done with 'actor'
            anim_contr = gameObject.GetComponent<AI_AnimatorController>();
            if (anim_contr == null)
                anim_contr = gameObject.AddComponent<AI_AnimatorController>();
            target = GameObject.FindGameObjectWithTag("player").transform.Find("Model"); // Find player transform
        }

        void Update()
        {
            ai_behaviour();
        }
        #endregion
        #region Methods
        void AIBehaviourSelect(AI_TYPE type)
        {
            switch (type)
            {
                case AI_TYPE.PEASANT:
                    ai_behaviour = AI_Behaviour_Peasant;
                    break;
                case AI_TYPE.MERCHANT:
                    ai_behaviour = AI_Behaviour_Merchant;
                    break;
                case AI_TYPE.GUARD:
                    ai_behaviour = AI_Behaviour_Guard;
                    break;
                case AI_TYPE.ENEMY:
                    ai_behaviour = AI_Behaviour_Enemy;
                    break;
                default:
                    ai_behaviour = AI_Behaviour_Enemy;
                    break;
            }
        }

        #endregion
        #region peasant behaviours
        void AI_Behaviour_Peasant()
        {
            // Will make peasant things
        }
        #endregion
        #region merchant behaviours
        void AI_Behaviour_Merchant()
        {
            // Make merchant things
        }
        #endregion
        #region guard behaviour
        void AI_Behaviour_Guard()
        {
            // Make guardian thins (watch etc.)
        }
        #endregion
        #region enemy behaviour
        void AI_Behaviour_Enemy()
        {
            // Attack player
            
        }
        #endregion
    }

    public enum AI_TYPE
    {
        PEASANT,
        MERCHANT,
        GUARD,
        ENEMY,
    }
}
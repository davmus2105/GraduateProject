using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAI : BaseMonoBehaviour
{
    Transform targetToMove;
    Transform playerTarget;
    CharacterInfo targetToAttack;

    public virtual void Move()
    {

    }
}

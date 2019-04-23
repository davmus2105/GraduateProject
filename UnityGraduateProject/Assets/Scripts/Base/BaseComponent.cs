using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseComponent
{
    public Actor actor;
    protected string component_name;    
    public BaseComponent(Actor addingActor, string componentName)
    {
        actor = addingActor;
        component_name = componentName;
    }
}

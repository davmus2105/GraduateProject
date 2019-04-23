using UnityEngine;
using System.Collections.Generic;



public class HealthComponent : BaseComponent
{
    float health;
    float max_health;

    public HealthComponent(Actor addingActor, string componentName = "health_component")
        : base(addingActor, componentName)
    {
    }

    public float GetHealth()
    {
        return health;
    }
    public void SetHealth(float value)
    {
        health = Mathf.Clamp(value, 0, max_health);
        if (health == 0)
        {
            Debug.Log("Character " + actor.charinfo.GetCharacterName() + "[" + actor.transform.name + "] is dead");
            // Some event Death()
        }
    }
    public float GetMaxHealth()
    {
        return max_health;
    }
    public void SetMaxHealth(float value)
    {
        if (value > 0)
            max_health = value;
    }
}

public class CharacterInfoComponent : BaseComponent
{
    private string character_name;

    public CharacterInfoComponent(Actor addingActor, string charactersName, string componentName = "CharacterInfoComponent")
        : base(addingActor, componentName)
    {      
        character_name = charactersName;
    }

    public string GetCharacterName()
    {
        return character_name;
    }
}

public class InventoryComponent : BaseComponent
{
    public Weapon weapon;
    public Shield shield;
    public Potion potion;

    public InventoryComponent(Actor addingActor, Weapon _weapon = null, 
                              Shield _shield = null, Potion _potion = null, 
                              string componentName = "InventoryComponent")
        : base(addingActor, componentName)
    {
        weapon = _weapon;
        shield = _shield;
        potion = _potion;
    }
}

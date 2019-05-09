using UnityEngine;
using System.Collections.Generic;



[System.Serializable]
public class HealthComponent : BaseComponent
{
    float health;
    float max_health;

    const float STD_HEALTH = 100f;

    public HealthComponent(Actor addingActor, float _health = STD_HEALTH, float maxHealth = STD_HEALTH, string componentName = "health_component")
        : base(addingActor, componentName)
    {
        health = _health;
        max_health = maxHealth;
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

[System.Serializable]
public class CharacterInfoComponent : BaseComponent
{
    public int Id => id;
    private int id;
    private string character_name;
    private string std_name = "Character Without Name";

    public CharacterInfoComponent(Actor addingActor, int _id, string charactersName = "", string componentName = "CharacterInfoComponent")
        : base(addingActor, componentName)
    {
        // set a name
        if (string.IsNullOrEmpty(charactersName))
            character_name = std_name;
        else
            character_name = charactersName;
        // set an id
        id = _id;
    }

    public string GetCharacterName()
    {
        return character_name;
    }
}

[System.Serializable]
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

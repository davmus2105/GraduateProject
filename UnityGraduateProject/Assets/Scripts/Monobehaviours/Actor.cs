using UnityEngine;
using System.Collections.Generic;

public class Actor : MonoBehaviour
{
    public string CharacterName
    {
        get { return character_name; }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                character_name = value;
            }
        }
    }    
    public float Health
    {
        get
        {
            return health;
        }
        set
        {
            if (value <= max_health || value > 0f)                
                health = value;
        }
    }
    public float MaxHealth
    {
        get
        {
            return max_health;
        }
        set
        {
            if (value < health)
                return;
            else
                max_health = value;
        }
    }

    [SerializeField] private string character_name;
    [SerializeField] private float health;
    [SerializeField] private float max_health;
    public bool isBlocking;


    const float STD_HEALTH = 100f;
    const float STD_MAX_HEALTH = 100f;
    const string STD_CHRCTR_NAME = "Новик";
    public IDie connectedBehaviour; // AI behaviour or playerController components
    #region Methods
    // ------- Health methods ----------------
    public float GetHealth()
    {
        return health;
    }
    public void SetHealth(float _health)
    {
        if (_health > max_health || _health <= 0f)
            return;
        else
            health = _health;
    }
    public void AddHealth(float _health)
    {
        if (_health <= 0)
            return;
        health += _health;
        if (health > max_health)
            health = max_health;
    }
    public float GetMaxHealth()
    {
        return max_health;
    }
    public void SetMaxHealth(float maxHealth)
    {
        if (maxHealth < health)
            return;
        else
            max_health = maxHealth;
    }
    public void GetDamage(float _damage)
    {
        if (_damage <= 0 || isBlocking)
            return;
        health -= _damage;
        if (health <= 0)
        {
            Debug.Log(character_name + " is dead");
            connectedBehaviour.Die();
        }
    }
    // ---------------------------------------
    // ------------ Constructors -------------
    public Actor(string char_name = STD_CHRCTR_NAME, float _health = STD_HEALTH, float maxHealth = STD_MAX_HEALTH)
    {
        character_name = char_name;
        health = _health;
        max_health = maxHealth;
    }
    // ---------------------------------------
    private void Start()
    {
        connectedBehaviour = transform.GetComponent<IDie>();
        isBlocking = false;
    }
    #endregion
}


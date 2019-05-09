using UnityEngine;
using System.Collections.Generic;

public class Actor : BaseMonoBehaviour
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

    private string character_name;
    private float health;
    private float max_health;

    private IDie target;
    #region Methods
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
    public void GetDamage(float _damage)
    {
        health -= _damage;
        if (health <= 0)
        {
            target.Die();
        }
    }


    private void Start()
    {
        target = transform.GetComponent<IDie>();
    }
    #endregion
}


using UnityEngine;
using System.Collections;

public abstract class InventoryItem
{
    public string item_name;
    public Sprite item_sprite;
    public GameObject item_model;
    public InventoryItem(string name, Sprite sprite, GameObject model)
    {
        item_name = name;
        item_sprite = sprite;
        item_model = model;
    }
}

public class Weapon : InventoryItem
{
    public float weapondamage
    {
        get => weapondamage;
        set
        {
            if (value > 0)            
                weapondamage = value;            
        }
    }
    public Weapon(string name, float weapon_damage, Sprite sprite, GameObject model)
        : base(name, sprite, model)
    {
        weapondamage = weapon_damage;
    }
}

public class Shield : InventoryItem
{
    public float defense
    {
        get => defense;
        set
        {
            if (value > 0)
                defense = value;          
        }
    }
    public Shield(string name, float _defense, Sprite sprite, GameObject model)
        : base(name, sprite, model)
    {
        defense = _defense;
    }
}

public class Potion : InventoryItem
{
    private float healpower;
    private int count;

    public Potion(string name, float _healpower, int _count, Sprite sprite, GameObject model)
        : base(name, sprite, model)
    {
        healpower = _healpower;
        count = _count;
    }

    public int GetCount() => count;
    public void AddPotion(int amount) => count += amount;
}



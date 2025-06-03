using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Item
{
    public Sprite image;
    public string name;
    public string description;

    public ItemUsePhase phase;
    public bool isUsed;
}

[Serializable]
public class ItemManager 
{
    [SerializeField] Item[] items;

    public Item[] GetAllItems() 
    {
        return items;
    }

    public Item[] GetItemsWithType(ItemUsePhase phase)
    { 
        List<Item> list = new List<Item>();

        foreach (Item item in items)
        {
            if (item.phase == phase)
            { 
                list.Add(item);
            }
        }

        return list.ToArray();
    }
}

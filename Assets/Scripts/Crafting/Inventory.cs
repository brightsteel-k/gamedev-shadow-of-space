using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEditor;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Item> items;

    public void addItem(Item item)
    {
        //Adds to the end of the list (bottom of inventory)
        items.Add(item); 
    }

    public Item removeTopItem()
    {
        if (items.Count <= 0)
        {
            return null;
        }
        Item first = items[0];
        items.RemoveAt(0);
        return first;
    }

    //Checks if recipe is makeable (for use in UI)
    public bool canMakeRecipe(Recipe recipe)
    {

        foreach (Recipe.Pair pair in recipe.needed)
        {
            if (countIn(pair.item) < pair.amount)
            {
                return false;
            }
        }

        return true;
    }

    public bool makeRecipe(Recipe recipe)
    {
        if (!canMakeRecipe(recipe))
        {
            return false;
        }
        
        foreach (Recipe.Pair pair in recipe.needed)
        {
            remove(pair.item, pair.amount);

        }
        
        
        
        addItem(recipe.created);
        
        return true;
    }
    
    //Removes a number of item of the same type given 
    public void remove(Item toRemove, int num)
    {
        int count = num;
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].type == toRemove.type)
            {
                num--;
                items.RemoveAt(i);
                i--;

                if (num <= 0)
                {
                    return;
                }
            }
        }
    }

    //Counts how many of one item is in the inventory.
    public int countIn(Item item)
    {
        int count = 0;
        foreach (Item held in items)
        {
            if (held.type == item.type)
            {
                count += 1;
            }
        }

        return count;
    }
    
    //Gets the number of items held in the inventory
    public int itemCount()
    {
        return items.Count;
    }
    
}

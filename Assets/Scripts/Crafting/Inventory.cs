using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    // All item objects in the game
    public static Dictionary<string, Item> ALL_ITEMS;
    //List of currently held items.
    public List<Item> items = new List<Item>();
    
    //Discovered items
    public List<Item> discovered = new List<Item>();

    //For updating the window
    public CraftingUI craftUI;
        
    //To work with the UI, it must be connected to an inventory bar.
    public InvBar itemBar;
    

    //To see which items are selected.
    private ItemSelector select;
    //The bar lenght determines how many items can be held.
    public int barLength;
    //Gets the bar

    private void Awake()
    {
        initializeItems();
    }
    void Start()
    {
        select = itemBar.selector;
        updateBar();
    }

    // Maps all item objects in the Scriptables/Items folder to their IDs
    private void initializeItems()
    {
        Item[] itemsArray = Resources.LoadAll<Item>("Scriptables/Items");
        ALL_ITEMS = new Dictionary<string, Item>();
        foreach (Item i in itemsArray)
        {
            ALL_ITEMS.Add(i.id, i);
        }
    }

    public bool isDiscovered(Item item)
    {
        bool known = false;
        foreach (Item desc in discovered)
        {
            if (desc.id == item.id)
            {
                known = true;
            }
        }

        return known;
    }
    
    // Adds given item object to inventory
    public bool addItem(Item item)
    {
        if (items.Count >= barLength)
            return false;

        if (!isDiscovered(item))
        {
            discovered.Add(item);
        }
        
        //Adds to the end of the list (bottom of inventory)
        items.Add(item);
        updateBar();

        return true;
    }

    // Adds item to inventory by ID
    public bool addItem(string item)
    {
        return addItem(ALL_ITEMS[item]);
    }

    //Returns which item is selected
    public Item getSelectedItem()
    {
        if (select.pos >= items.Count)
        {
            return null;
        }
        
        return items[select.pos];

    }
    
    //Removes item selected by the selector
    public Item removeSelectedItem()
    {
        if (select.pos >= items.Count)
        {
            return null;
        }
        
        Item toRemove = items[select.pos];
        items.RemoveAt(select.pos);
        
        updateBar();
        
        return toRemove;
    }
    
    //Removes item at given position;
    public Item removeItemAt(int position)
    {
        if (position >= items.Count)
        {
            return null;
        }
        Item toRemove = items[position];
        items.RemoveAt(position);
        
        updateBar();
        return toRemove;
    }


    
    
    //Checks if recipe is makeable (for use in UI)
    public bool canMakeRecipe(Recipe recipe)
    {

        foreach (Recipe.Pair1 pair in recipe.needed)
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
        
        foreach (Recipe.Pair1 pair in recipe.needed)
        {
            remove(pair.item, pair.amount);
        }
        
       
        
        
        updateBar();
        if (!recipe.created.isLiquid)
        {
            for (int i = 0; i < recipe.amount; i++)
            {
                addItem(recipe.created);
            }
        }

        return true;
    }
    
    //Removes a number of item of the same type given 
    public void remove(Item toRemove, int num)
    {
        int count = num;
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].id == toRemove.id)
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
        updateBar();
    }

    //Counts how many of one item is in the inventory.
    public int countIn(Item item)
    {
        int count = 0;
        foreach (Item held in items)
        {
            if (held.id == item.id)
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

    public void updateBar()
    {
        itemBar.updateBar(items);
        if (craftUI.gameObject.activeSelf)
            craftUI.show();
        Player.WORLD_PLAYER.UpdateSelectedItem();
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ResourceCrafter : MonoBehaviour
{
    //This list holds all the resources
    public List<LiquidResource> resources;

    public CraftingUI crafting;
    public BarManager bar;
    private int selected = 0;

    void Start()
    {
        if (resources.Count > 0)
        {
            selectResource(resources[0].resource.id);
        }
    }
    //Returns how much of the resource (with id Name) the system has.
    public float getAmount(string id)
    {
        for(int i = 0; i < resources.Count; i++) 
        {
            if (resources[i].resource.id == id)
            {
                return resources[i].amount;
            }
        }
        return 0;
    }

    public bool canMakeRecipe(Recipe rec)
    {
        foreach(Recipe.Pair2 pair in rec.liquids)
        {
            if (getAmount(pair.res.name) < pair.amount)
            {
                return false;
            }
        }

        return true;
    }
    
    public void makeRecipe(Recipe rec)
    {
        foreach(Recipe.Pair2 pair in rec.liquids)
        {
            changeAmount(pair.res.name, -pair.amount);
        }

    }

    //adds or removes the given "dif" of a resource with to the bar the given name.
    //Make "dif" positive to add that amount or negative to remove it.
    public float changeAmount(string id, float dif)
    {
        for(int i = 0; i < resources.Count; i++) 
        {
            if (resources[i].resource.id == id)
            {
                resources[i].amount += dif;
                if (resources[i].amount < 0)
                {
                    resources[i].amount = 0;
                }
                else if (resources[i].amount > resources[i].resource.maximum)
                {
                    resources[i].amount = resources[i].resource.maximum;
                }
                updateUI();
                crafting.show();
                return resources[i].amount;
            }
        }

        crafting.show();
        return 0;
    }
    
    //To be used mostly by the buttons:
    public void selectResource(string id)
    {
        for(int i = 0; i < resources.Count; i++) 
        {
            if (resources[i].resource.id == id)
            {
                selected = i;
                updateUI();
            }
        }
    }

    //Sets the bar on the UI to be correct.
    private void updateUI()
    {
        bar.setValue(resources[selected].amount);
        bar.setMaximum(resources[selected].resource.maximum);
        bar.setColor(resources[selected].resource.color);
    }
}

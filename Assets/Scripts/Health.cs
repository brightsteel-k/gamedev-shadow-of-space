using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Health : MonoBehaviour
{
    
    
    
    //Max health, made visible in editor
    [SerializeField]
    private float maxHealth = 100;
    private float value = 100;

    //This "dead" variable could be replaced with a "gameover" function.
    public bool dead = false;
    
    //This must be set to the corresponding bar in the UI.
    //If missing, wont updated
    public BarManager bar;

    //Starting off by initializing the bar (just in case)
    void Start()
    {
        bar.maxValue = maxHealth;
        updateBar();
    }

    public float getHealth()
    {
        return value;
    }    
    
    //Use these instead of changing the amount
    public void removeHealth(float amount)
    {
        value -= amount;
        updateBar();
        if (value <= 0)
        {
            dead = true;
        }
    }
    public void setHealth(float amount)
    {
        value = amount;
        updateBar();

        if (value <= 0)
        {
            dead = true;
        }
        else
        {
            dead = false;
        }
    }
    public void addHealth(float amount)
    {
        value += amount;
        updateBar();
        
        if (amount > 0)
        {
            dead = false;
        }
    }
    public void setMaxHealth(float amount)
    {
        maxHealth = amount;
        bar.maxValue = amount;
    }

    public void updateBar()
    {
        bar.setValue(value);
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Energy : MonoBehaviour
{
    [SerializeField]
    private float maxEnergy = 100;
    private float value = 100;
    
    //This must be set to the corresponding bar in the UI.
    //If missing, wont updated
    public BarManager bar;
    public bool hasBar = true;
    
    //Perhaps we only drain during certain times
    public bool draining = true;
    private bool isDrilling = false;
    [SerializeField] Color normalColor;
    [SerializeField] Color drillingColor;

    float drainSpeed = 0.5f;

    //Starting off by initializing the bar (just in case)
    void Start()
    {
        bar.maxValue = maxEnergy;
        bar.setColor(normalColor);
        bar.setValue(value);
    }
    
    //For now, it will slowly drain energy as long as main is running.
    //Perhaps energy will change by different amounts later.
    void Update()
    {
        if (draining)
        {
            //Multiplaying by deltaTime to make the drain smooth with framerate.
            value -= drainSpeed * Time.deltaTime;
            
            updateBar();
            if (value <= 0)
            {
                value = 0;
                energyEmpty();
            }
        }
    }

    public float getEnergy()
    {
        return value;
    }
    
    //Use these add/remove/set functions instead of just changing the value so it updates the bar.
    public void removeEnergy(float amount)
    {
        value -= amount;
        updateBar();
    }
    public void setEnergy(float amount)
    {
        value = amount;
        
        if (value > maxEnergy)
        {
            value = maxEnergy;
        }
        
        updateBar();
    }
    public void addEnergy(float amount)
    {
        value += amount;
        if (value > maxEnergy)
        {
            value = maxEnergy;
        }
        
        updateBar();
        
    }
    public void setMaxEnergy(float amount)
    {
        maxEnergy = amount;
        bar.maxValue = amount;
        if (value > maxEnergy)
        {
            value = maxEnergy;
        }
        
        updateBar();
    }

    public void SetDrilling(bool drilling)
    {
        if (isDrilling == drilling)
            return;

        if (drilling)
        {
            drainSpeed = 2f;
            bar.setColor(drillingColor);
        }
        else
        {
            drainSpeed = 0.5f;
            bar.setColor(normalColor);
        }
        isDrilling = drilling;
    }

    public void updateBar()
    {
        if (bar)
        {
            bar.setValue(value);
        }
        else if (hasBar)
        {
            Debug.Log("Error with a resource bar, resource should have health bar set. To disable this, uncheck 'hasBar' for health, energy, etc");
        }
    }

    public Item SwitchBatteries(Item newBattery)
    {
        Item oldBattery = new Item(Inventory.ALL_ITEMS["battery"], ("power", value));
        setEnergy(newBattery.GetTag("power"));
        updateBar();
        return oldBattery;
    }
    
    void energyEmpty()
    {
        //Gameover? Other consequences? Put a "GameOver" call to a game manager?
    }
}

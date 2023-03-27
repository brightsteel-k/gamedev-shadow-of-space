using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private bool lowEnergy = false;
    private AudioSource audioSource;
    private AudioClip powerDownClip;
    private Image barBack;
    [SerializeField] Color normalColor;
    [SerializeField] Color drillingColor;
    [SerializeField] Color lowEnergyColor;

    float drainSpeed = 0.5f;

    //Starting off by initializing the bar (just in case)
    void Start()
    {
        bar.maxValue = maxEnergy;
        bar.setColor(normalColor);
        bar.setValue(value);
        audioSource = bar.GetComponent<AudioSource>();
        powerDownClip = Resources.Load<AudioClip>("Sounds/SuitPowerDown");
        barBack = bar.GetComponent<Image>();
        EventManager.OnPlayerDying += Deactivate;
        EventManager.OnGameWinning += Deactivate;
    }
    
    //For now, it will slowly drain energy as long as main is running.
    //Perhaps energy will change by different amounts later.
    void Update()
    {
        if (draining)
        {
            //Multiplaying by deltaTime to make the drain smooth with framerate.
            value -= drainSpeed * Time.deltaTime;

            VerifyEnergy();
            updateBar();
        }

        if (lowEnergy)
            barBack.color = Color.Lerp(Color.white, lowEnergyColor, Mathf.Cos(3f * Time.time));
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

        VerifyEnergy();
        updateBar();
    }
    public void addEnergy(float amount)
    {
        value += amount;

        VerifyEnergy();
        updateBar();
        
    }
    public void setMaxEnergy(float amount)
    {
        maxEnergy = amount;
        bar.maxValue = amount;
        VerifyEnergy();
        updateBar();
    }

    public void SetDrilling(bool drilling)
    {
        if (isDrilling == drilling)
            return;

        if (drilling)
        {
            drainSpeed = 0.75f;
            bar.setColor(drillingColor);
        }
        else
        {
            drainSpeed = 0.25f;
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
        Item oldBattery = ScriptableObject.CreateInstance<Item>().Initialize(Inventory.ALL_ITEMS["battery"], ("power", value));
        setEnergy(newBattery.GetTag("power"));
        updateBar();
        return oldBattery;
    }

    private void VerifyEnergy()
    {
        if (value > maxEnergy)
        {
            value = maxEnergy;
        }
        else if (value <= 0)
        {
            EnergyEmpty();
            value = 0f;
        }
        else if (value <= 20)
        {
            if (!lowEnergy)
            {
                audioSource.Play();
                lowEnergy = true;
            }
        }
        else if (lowEnergy)
        {
            audioSource.Stop();
            barBack.color = Color.white;
            lowEnergy = false;
        }
    }

    void EnergyEmpty()
    {
        Player.PlaySound(powerDownClip, 2f);
        EventManager.PlayerDying();
    }

    void Deactivate()
    {
        audioSource.Stop();
        draining = false;
    }
}

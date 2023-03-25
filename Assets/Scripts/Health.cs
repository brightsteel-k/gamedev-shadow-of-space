using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    //Max health, made visible in editor
    [SerializeField]
    private float maxHealth = 100;
    private float value = 100;
    private bool lowHealth = false;

    //This "dead" variable could be replaced with a "gameover" function.
    public bool dead = false;
    
    //This must be set to the corresponding bar in the UI.
    //If missing, wont updated
    [SerializeField] private BarManager bar;
    private AudioSource audioSource;
    private Image barBack;
    [SerializeField] private AudioClip playerHurtClip;

    //Starting off by initializing the bar (just in case)
    void Start()
    {
        bar.maxValue = maxHealth;
        audioSource = bar.GetComponent<AudioSource>();
        barBack = bar.GetComponent<Image>();
        EventManager.OnPlayerDying += Deactivate;
        updateBar();
    }

    private void Update()
    {
        if (lowHealth)
            barBack.color = Color.Lerp(Color.white, Color.red, Mathf.Cos(3f * Time.time));
    }

    public float getHealth()
    {
        return value;
    }    
    
    //Use these instead of changing the amount
    public void removeHealth(float amount)
    {
        value -= amount;
        audioSource.PlayOneShot(playerHurtClip, 1.25f);
        VerifyHealth();
        updateBar();
    }
    public void setHealth(float amount)
    {
        value = amount;

        VerifyHealth();
        updateBar();
    }
    public void addHealth(float amount)
    {
        value += amount;
        VerifyHealth();
        updateBar();
    }
    public void setMaxHealth(float amount)
    {
        maxHealth = amount;
        bar.maxValue = amount;
        VerifyHealth();
        updateBar();
    }

    public void updateBar()
    {
        bar.setValue(value);
    }

    private void VerifyHealth()
    {
        if (value > maxHealth)
            value = maxHealth;
        else if (value <= 0)
            HealthRunout();
        else if (value <= 20)
        {
            if (!lowHealth)
            {
                lowHealth = true;
            }
        }
        else if (lowHealth)
        {
            barBack.color = Color.white;
            lowHealth = false;
        }
    }

    private void HealthRunout()
    {
        EventManager.PlayerDying();
    }

    private void Deactivate()
    {
        dead = true;
    }
}

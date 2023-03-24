using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
//This script goes on the parent object for a health bar. It manages everything for the bar.

public class BarManager : MonoBehaviour
{

    //All of the bar's children:

    public TMP_Text text;
    public Transform colorBar;
    public Image barSprite;

    //The bar's parameters:
    [SerializeField] string unitSuffix = "%";
    public float maxValue = 100f;
    public float currentValue = 100f;
    public Color barColor = Color.red;

    
    //Functions to be called to change the bar

    public void setValue(float val)
    {
        currentValue = val;
        setBar();

    }

    public void setMaximum(float max)
    {
        maxValue = max;
        setBar();
    }

    public void setColor(Color col)
    {
        barColor = col;
        setBar();
    }
    
    //The function that actually does the bar setting:
    //We could add here something that changes the color when things get low automatically.
    private void setBar()
    {
        colorBar.localScale = new Vector3(currentValue/maxValue, 1, 1);
        text.text = ((int)(100 * currentValue / maxValue)).ToString("D") + unitSuffix;
        barSprite.color = barColor;
    }
    
}

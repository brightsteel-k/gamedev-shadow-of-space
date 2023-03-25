using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private Volume dayVolume;
    [SerializeField] private Volume eclipseVolume;
    
    [SerializeField] private CustomRenderSettings daySettings;
    [SerializeField] private CustomRenderSettings nightSettings;

    [Serializable]
    public enum TimeState
    {
        Bright,
        Day,
        Penumbra,
        Eclipse
    };

    public TimeState timeState;

    [SerializeField] private float timeStateCounter = 5;

    private Dictionary<TimeState, float> timeStateLengths = new Dictionary<TimeState, float>()
    {
        [TimeState.Day] = 60,
        [TimeState.Bright] = 360,
        [TimeState.Penumbra] = 120,
        [TimeState.Eclipse] = 300
        // [TimeState.Day] = 5,
        // [TimeState.Bright] = 5,
        // [TimeState.Penumbra] = 5,
        // [TimeState.Eclipse] = 5
    };

    private void Update()
    {
        timeStateCounter -= Time.deltaTime;

        if (timeStateCounter < 0)
        {
            AdvanceTimeState();
        }

        UpdatePostProcessing();
        UpdateLighting();
    }

    void AdvanceTimeState()
    {
        timeState = (int)timeState > 2 ? TimeState.Bright : timeState + 1;
        timeStateCounter = timeStateLengths[timeState];
    }

    void UpdatePostProcessing()
    {
        switch (timeState)
        {
            case TimeState.Day:
                dayVolume.weight = 1;
                eclipseVolume.weight = 0;
                break;
            case TimeState.Bright:
                dayVolume.weight = 1 - (timeStateCounter / timeStateLengths[TimeState.Day]);
                eclipseVolume.weight = timeStateCounter / timeStateLengths[TimeState.Day];
                break;
            case TimeState.Penumbra:
                eclipseVolume.weight = 1 - (timeStateCounter / timeStateLengths[TimeState.Penumbra]);
                dayVolume.weight = timeStateCounter / timeStateLengths[TimeState.Penumbra];
                break;
            case TimeState.Eclipse:
                eclipseVolume.weight = 1;
                dayVolume.weight = 0;
                break;
        }
    }

    void UpdateLighting()
    {
        float t = 1 - (timeStateCounter / timeStateLengths[timeState]);
        switch (timeState)
        {
            case TimeState.Bright:
                RenderSettings.fog = true;
                RenderSettings.fogDensity = Mathf.Lerp(nightSettings.fogIntensity, daySettings.fogIntensity, t);
                RenderSettings.ambientLight = Color32.Lerp(nightSettings.colour, daySettings.colour, t);
                break;
            
            case TimeState.Day:
                RenderSettings.fog = false;
                RenderSettings.fogDensity = daySettings.fogIntensity;
                RenderSettings.ambientLight = daySettings.colour;

                break;
            case TimeState.Penumbra:
                RenderSettings.fog = true;
                RenderSettings.fogDensity = Mathf.Lerp(daySettings.fogIntensity, nightSettings.fogIntensity, t);
                RenderSettings.ambientLight = Color32.Lerp(daySettings.colour, nightSettings.colour, t);
                
                break;
            case TimeState.Eclipse:
                RenderSettings.fog = true;
                RenderSettings.fogDensity = nightSettings.fogIntensity;
                RenderSettings.ambientLight = nightSettings.colour;
                break;
        }
    }
}

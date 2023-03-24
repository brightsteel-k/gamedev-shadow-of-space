using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private Volume dayVolume;
    [SerializeField] private Volume eclipseVolume;
    [SerializeField] private LightingSettings lightingSettings;

    [System.Serializable]
    public enum TimeState
    {
        Bright,
        Day,
        Penumbra,
        Eclipse
    };

    public TimeState timeState;

    [SerializeField] private float time;
    [SerializeField] private float timeStateCounter = 5;

    public Dictionary<TimeState, float> timeStateLengths = new Dictionary<TimeState, float>()
    {
        [TimeState.Bright] = 5,
        [TimeState.Day] = 5,
        [TimeState.Penumbra] = 5,
        [TimeState.Eclipse] = 5
    };

    private void Update()
    {
        time += Time.deltaTime;
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
            case TimeState.Bright:
                dayVolume.weight = 1;
                eclipseVolume.weight = 0;
                break;
            case TimeState.Day:
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
        
    }
}

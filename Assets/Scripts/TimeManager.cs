using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private Volume dayVolume;
    [SerializeField] private Volume nightVolume;
    [SerializeField] private LightingSettings lightingSettings;

    [System.Serializable]
    public enum TimeState
    {
        Day,
        Twilight,
        Night,
        Morning
    };

    public TimeState timeState;

    [SerializeField] private float time;
    [SerializeField] private float timeStateCounter = 5;

    public Dictionary<TimeState, float> timeStateLengths = new Dictionary<TimeState, float>()
    {
        [TimeState.Day] = 5,
        [TimeState.Twilight] = 5,
        [TimeState.Night] = 5,
        [TimeState.Morning] = 5
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
        timeState = (int)timeState > 2 ? TimeState.Day : timeState + 1;
        timeStateCounter = timeStateLengths[timeState];
    }

    void UpdatePostProcessing()
    {
        switch (timeState)
        {
            case TimeState.Day:
                dayVolume.weight = 1;
                nightVolume.weight = 0;
                break;
            case TimeState.Twilight:
                nightVolume.weight = 1 - (timeStateCounter / timeStateLengths[TimeState.Twilight]);
                dayVolume.weight = timeStateCounter / timeStateLengths[TimeState.Twilight];
                break;
            case TimeState.Night:
                nightVolume.weight = 1;
                dayVolume.weight = 0;
                break;
            case TimeState.Morning:
                dayVolume.weight = 1 - (timeStateCounter / timeStateLengths[TimeState.Morning]);
                nightVolume.weight = timeStateCounter / timeStateLengths[TimeState.Morning];
                break;
        }
    }

    void UpdateLighting()
    {
        
    }
}

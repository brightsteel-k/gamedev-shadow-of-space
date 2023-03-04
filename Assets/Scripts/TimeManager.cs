using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private Volume dayVolume;
    [SerializeField] private Volume nightVolume;
    [SerializeField] private Volume realTimeVolume;

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
    [SerializeField] private float timeStateCounter = 300;

    public Dictionary<TimeState, float> timeStateLengths = new Dictionary<TimeState, float>()
    {
        [TimeState.Day] = 300,
        [TimeState.Twilight] = 300,
        [TimeState.Night] = 300,
        [TimeState.Morning] = 300
    };

    private void Start()
    {
        throw new NotImplementedException();
    }

    private void Update()
    {
        time += Time.deltaTime;
        timeStateCounter -= Time.deltaTime;

        if (timeStateCounter < 0)
        {
            AdvanceTimeState();
        }
    }

    void AdvanceTimeState()
    {
        timeState = (int)timeState > 2 ? TimeState.Day : timeState + 1;
        timeStateCounter = timeStateLengths[timeState];
    }
}

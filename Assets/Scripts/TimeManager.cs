using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private Volume dayVolume;
    [SerializeField] private Volume eclipseVolume;
    [SerializeField] private Volume dyingVolume;
    [SerializeField] private Image fadeImage;
    
    [SerializeField] private CustomRenderSettings daySettings;
    [SerializeField] private CustomRenderSettings nightSettings;
    private MuseSystem museSystem;

    [Serializable]
    public enum TimeState
    {
        Bright,
        Day,
        Penumbra,
        Eclipse
    };

    public static TimeState TIME_STATE;

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

    void Start()
    {
        museSystem = Player.WORLD_PLAYER.GetComponent<MuseSystem>();
        EventManager.OnPlayerDying += DisplayDyingOverlay;
    }

    private void Update()
    {
        if (EventManager.PLAYER_DYING)
            return;

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
        TIME_STATE = (int)TIME_STATE > 2 ? TimeState.Bright : TIME_STATE + 1;
        timeStateCounter = timeStateLengths[TIME_STATE];
        SendMuseMessage();
    }

    void UpdatePostProcessing()
    {
        switch (TIME_STATE)
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
        float t = 1 - (timeStateCounter / timeStateLengths[TIME_STATE]);
        switch (TIME_STATE)
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

    void SendMuseMessage()
    {
        if (TIME_STATE != TimeState.Bright)
        {
            museSystem.PrintMusing("transition_" + TIME_STATE.ToString().ToLower());
        }
    }

    private void DisplayDyingOverlay()
    {
        LeanTween.value(0f, 1f, 2f)
            .setOnUpdate(c => dyingVolume.weight = c)
            .setOnComplete(FadeToBlack);
    }

    private void FadeToBlack()
    {
        LeanTween.value(0f, 1f, 5f)
            .setOnUpdate(c => fadeImage.color = new Color(0, 0, 0, c));
    }
}

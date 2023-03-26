using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimeManager : MonoBehaviour
{
    [Tooltip("In order: Volume profiles for Bright, Day, Penumbra, and Eclipse.")]
    [SerializeField] private VolumeProfile[] volumeProfiles = new VolumeProfile[4];
    [SerializeField] private Volume currentVolume;
    [SerializeField] private Volume previousVolume;
    [SerializeField] private Volume dyingVolume;
    [SerializeField] private Image fadeImage;
    
    [SerializeField] private CustomRenderSettings daySettings;
    [SerializeField] private CustomRenderSettings nightSettings;
    private MuseSystem museSystem;
    private MusicManager musicManager;

    public static TimeState TIME_STATE;

    private Dictionary<TimeState, float> timeStateLengths = new Dictionary<TimeState, float>()
    {
        [TimeState.Bright] = 180,
        [TimeState.Day] = 180,
        [TimeState.Penumbra] = 120,
        [TimeState.Eclipse] = 300
    };

    [SerializeField] private float timeStateCounter = 180;
    private float transitionLength = 20;
    private float transitionCounter = 0;

    private void Awake()
    {
        TIME_STATE = 0;
    }

    void Start()
    {
        musicManager = GetComponent<MusicManager>();
        museSystem = Player.WORLD_PLAYER.GetComponent<MuseSystem>();
        EventManager.OnPlayerDying += DisplayDyingOverlay;
        timeStateCounter = timeStateLengths[TIME_STATE];
        musicManager.UpdateTimeStateTrack();
    }

    private void Update()
    {
        if (EventManager.PLAYER_DYING)
            return;

        timeStateCounter -= Time.deltaTime;
        if (transitionCounter > 0)
        {
            transitionCounter -= Time.deltaTime;
            if (transitionCounter < 0)
                transitionCounter = 0;
            UpdatePostProcessing();
        }
        UpdateLighting();

        if (timeStateCounter < 0)
            AdvanceTimeState();
    }

    void AdvanceTimeState()
    {
        TIME_STATE = (int)TIME_STATE >= 3 ? TimeState.Bright : TIME_STATE + 1;
        previousVolume.profile = currentVolume.profile;
        previousVolume.weight = 1;
        currentVolume.weight = 0;
        currentVolume.profile = volumeProfiles[(int)TIME_STATE];
        timeStateCounter = timeStateLengths[TIME_STATE];
        transitionCounter = transitionLength;
        HandleLightingShift();

        SendMuseMessage();
        musicManager.UpdateTimeStateTrack();

        if (TIME_STATE == TimeState.Eclipse)
        {
            StygianStalker.WORLD_STALKER.StartTracking();
        }
        else if (TIME_STATE == TimeState.Bright)
        {
            StygianStalker.WORLD_STALKER.BeginFleeing(Player.WORLD_PLAYER.transform.position);
            EndGame();
        }
    }

    void UpdatePostProcessing()
    {
        float decreasingWeight = transitionCounter / transitionLength;
        previousVolume.weight = decreasingWeight;
        currentVolume.weight = 1 - decreasingWeight;
    }

    void HandleLightingShift()
    {
        if (TIME_STATE == TimeState.Penumbra)
        {
            RenderSettings.fog = true;
            RenderSettings.fogMode = FogMode.Exponential;
        }
        else if (TIME_STATE == TimeState.Day)
        {
            RenderSettings.fog = false;
        }
    }

    void UpdateLighting()
    {
        if (TIME_STATE == TimeState.Bright && RenderSettings.fog)
        {
            float t = transitionCounter / transitionLength;
            RenderSettings.fogDensity = Mathf.Lerp(daySettings.fogIntensity, nightSettings.fogIntensity, t);
            RenderSettings.ambientLight = Color32.Lerp(daySettings.colour, nightSettings.colour, t);
        }
        else if (TIME_STATE == TimeState.Penumbra)
        {
            float t = timeStateCounter / timeStateLengths[TimeState.Penumbra];
            RenderSettings.fogDensity = Mathf.Lerp(nightSettings.fogIntensity, daySettings.fogIntensity, t);
            RenderSettings.ambientLight = Color32.Lerp(nightSettings.colour, daySettings.colour, t);
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
        fadeImage.gameObject.SetActive(true);
        LeanTween.value(0f, 1f, 3.5f)
            .setOnUpdate(c => fadeImage.color = new Color(0, 0, 0, c))
            .setOnComplete(e => {
                LeanTween.delayedCall(4f, f => ChangeScene("Dead"));
            });
    }

    private void EndGame()
    {
        if (EventManager.PLAYER_DYING)
            return;
        EventManager.WinGame();
        MuseSystem.SetMusable(false);
        museSystem.PrintMusing("game_ending");
        LeanTween.delayedCall(5f, e => ChangeScene("Win"));
    }

    private void ChangeScene(string location)
    {
        LeanTween.cancelAll();
        Cursor.visible = true;
        SceneManager.LoadScene(location);
    }
}

[Serializable]
public enum TimeState
{
    Bright,
    Day,
    Penumbra,
    Eclipse
}
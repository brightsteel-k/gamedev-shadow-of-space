using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private static MusicManager INSTANCE;
    [SerializeField] private AudioClip dayTrack;
    [SerializeField] private AudioClip penumbraTrack;
    [SerializeField] private AudioClip eclipseTrack;
    [SerializeField] private AudioClip stalkingTrack;
    private AudioSource audioSource;

    private void Awake()
    {
        INSTANCE = this;
        audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        EventManager.OnPlayerDying += OnPlayerDying;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateTimeStateTrack()
    {
        switch (TimeManager.TIME_STATE)
        {
            case TimeState.Bright:
                audioSource.clip = dayTrack;
                audioSource.Play();
                break;
            case TimeState.Penumbra:
                audioSource.clip = penumbraTrack;
                audioSource.Play();
                break;
            case TimeState.Eclipse:
                audioSource.clip = eclipseTrack;
                audioSource.Play();
                break;
        }
    }

    public static void SetAttackingTrack(bool attacking)
    {
        if (attacking)
        {
            AudioSource source = INSTANCE.audioSource;
            source.clip = INSTANCE.stalkingTrack;
            source.Play();
        }
        else
        {
            INSTANCE.UpdateTimeStateTrack();
        }
    }

    public void OnPlayerDying()
    {
        LeanTween.value(gameObject, 1f, 0f, 3f)
            .setOnUpdate(e =>
            {
                audioSource.volume = e;
                audioSource.pitch = e / 2f + 0.5f;
            })
            .setOnComplete(audioSource.Stop);
    }
}

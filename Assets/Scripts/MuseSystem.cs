using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json;

public class MuseSystem : MonoBehaviour
{
    public static Dictionary<string, string[]> allMusings;
    public static bool MUSABLE = true;
    private static Slider MUSE_SLIDER;
    private static MuseSystem INSTANCE;
    private int musableLayerMask;
    [SerializeField] private KeyCode museButton;
    [SerializeField] private RectTransform museSliderTransform;
    [SerializeField] private TextMeshProUGUI museText;
    private Sprite skyIcon;
    private AudioSource audioSource;
    [SerializeField] private AudioClip[] typingClips = new AudioClip[3];

    [SerializeField] private SpriteRenderer iconObject;
    private Image iconEnvironment;
    private string currentSubject = "";
    private Vector3 currentSubjectPos;
    private bool isMusing;
    private float speakingCooldown;
    private bool isSpeaking = false;

    // Start is called before the first frame update
    void Start()
    {
        INSTANCE = this;
        MUSE_SLIDER = museSliderTransform.transform.GetComponent<Slider>();
        iconEnvironment = museSliderTransform.Find("Icon").GetComponent<Image>();
        audioSource = museSliderTransform.transform.GetComponent<AudioSource>();
        skyIcon = Resources.Load<Sprite>("Textures/UI/muse_sky");
        musableLayerMask = LayerMask.GetMask("Musable");

        TextAsset ta = Resources.Load<TextAsset>("Data/musings");
        allMusings = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(ta.text);
        EventManager.OnPlayerDying += Deactivate;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(museButton) && MUSABLE)
            TryMusing();
        
        if (Input.GetKeyUp(museButton) && isMusing)
            StopMusing();
    }

    private void FixedUpdate()
    {
        if (isMusing)
            UpdateMusingBar();

        if (speakingCooldown > 0)
        {
            speakingCooldown -= Time.deltaTime;
        }
        else if (speakingCooldown < 0)
        {
            FadeMusing();
            speakingCooldown = 0;
        }
    }

    private void UpdateMusingBar()
    {
        if (!currentSubject.StartsWith("@E") && Vector3.Distance(transform.position, currentSubjectPos) > 5f)
        {
            StopMusing();
            return;
        }

        MUSE_SLIDER.value += Time.deltaTime / 1.5f;
        if (MUSE_SLIDER.value == 1)
        {
            PrintMusing(currentSubject);
        }
    }

    private void TryMusing()
    {
        speakingCooldown = -1;
        LeanTween.cancel(museText.gameObject);
        Collider[] results = new Collider[1];
        int numColliding = Physics.OverlapSphereNonAlloc(transform.position + Player.WORLD_PLAYER.GetDirection() * 1.5f, 3f, results, musableLayerMask);
        if (numColliding > 0)
            BeginMusingObject(results[0]);
        else
            BeginMusingEnvironment();
    }

    private void BeginMusingObject(Collider c)
    {
        iconObject.transform.position = c.transform.position + Vector3.up;
        iconObject.gameObject.SetActive(true);
        currentSubject = c.GetComponent<WorldObject>().GetID();
        currentSubjectPos = c.transform.position;
        isMusing = true;
    }

    private void BeginMusingEnvironment()
    {
        iconEnvironment.enabled = true;
        if (TimeManager.TIME_STATE == TimeManager.TimeState.Penumbra)
            currentSubject = "@E_penumbra";
        else if (TimeManager.TIME_STATE == TimeManager.TimeState.Eclipse)
            currentSubject = "@E_eclipse";
        else if (Player.WORLD_PLAYER.hasEncounteredMonster)
            currentSubject = "@E_monster";
        else
            currentSubject = "@E_all";
        isMusing = true;
    }

    private void StopMusing()
    {
        iconObject.gameObject.SetActive(false);
        iconEnvironment.enabled = false;
        MUSE_SLIDER.value = 0;
        isMusing = false;
    }

    public void PrintMusing(string subject)
    {
        StopMusing();
        isSpeaking = true;

        LeanTween.cancel(gameObject);
        string[] options = allMusings[subject];
        string message = "\"" + options[RandomGen.Range(0, options.Length - 1)] + "\"";
        museText.maxVisibleCharacters = 0;
        museText.SetText(message);
        museText.CrossFadeAlpha(1, 0, true);
        LeanTween.value(gameObject, 0, message.Length, message.Length * 0.05f)
            .setOnUpdate(c =>
            {
                int intc = (int)c;
                if (intc > museText.maxVisibleCharacters && intc % 2 == 0)
                    PlayRandomTypeSound();
                museText.maxVisibleCharacters = intc;
            });
        speakingCooldown = 5;
    }

    private void PlayRandomTypeSound()
    {
        audioSource.PlayOneShot(typingClips[Random.Range(0, 3)]);
    }

    private void FadeMusing()
    {
        if (!isSpeaking)
            return;

        museText.CrossFadeAlpha(0, 0.5f, false);
        isSpeaking = false;
    }

    public static void SetMusable(bool musable)
    {
        MUSABLE = musable;
        INSTANCE.StopMusing();
        MUSE_SLIDER.gameObject.SetActive(musable);
    }

    private void Deactivate() => SetMusable(false);
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MuseSystem : MonoBehaviour
{
    [SerializeField] RectTransform museSliderTransform;
    private Slider museSlider;

    // Start is called before the first frame update
    void Start()
    {
        museSlider = museSliderTransform.transform.GetComponent<Slider>();
        museSliderTransform.anchoredPosition = new Vector3(-50 * InvBar.GetScale() - 25f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

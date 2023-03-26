using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backdrop : MonoBehaviour
{
    [Tooltip("Determines whether the height or width will be the limiting dimension.")]
    [SerializeField] private bool heightBased;
    [Tooltip("Either the natural height of the backdrop if heightBased is checked, or the natural width if not.")]
    [SerializeField] private float givenValue;
    [SerializeField] private float extraScaleFactor;

    // Start is called before the first frame update
    void Start()
    {
        AdjustSize();
    }

    void AdjustSize()
    {
        float scale = (heightBased ? Screen.height : Screen.width) / givenValue;
        scale *= extraScaleFactor;
        transform.localScale = new Vector3(scale, scale, scale);
    }
}

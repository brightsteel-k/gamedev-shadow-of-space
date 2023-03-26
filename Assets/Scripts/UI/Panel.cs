using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel : MonoBehaviour
{
    [SerializeField] private float naturalHeight;

    // Start is called before the first frame update
    void Start()
    {
        AdjustSize();
    }

    void AdjustSize()
    {
        float scale = (Screen.height - 50) / naturalHeight;
        transform.localScale = new Vector3(scale, scale, scale);
        SetShowing(false);
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetShowing(bool active)
    {
        gameObject.SetActive(active);
    }
}

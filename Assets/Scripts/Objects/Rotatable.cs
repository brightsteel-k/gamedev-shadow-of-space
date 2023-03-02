using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotatable : MonoBehaviour
{
    protected GameObject textureObject;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        textureObject = gameObject;
        InitRotation();
    }

    protected void InitRotation()
    {
        EventManager.OnWorldPivot += Pivot;
        Player.PivotInit(textureObject);
    }

    private void OnDestroy()
    {
        EventManager.OnWorldPivot -= Pivot;
    }

    protected virtual void Pivot(bool clockwise) => Player.Pivot(textureObject, clockwise);
}

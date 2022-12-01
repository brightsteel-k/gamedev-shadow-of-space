using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldObject : MonoBehaviour
{
    private void Start()
    {
        EventManager.OnWorldPivot += Pivot;
    }

    private void OnDestroy()
    {
        EventManager.OnWorldPivot -= Pivot;
    }

    private void Pivot() => Player.Pivot(this.gameObject);

    public void SetActive(bool active) //Properly deactivates world object, in case information about it needs to be saved first
    {
        gameObject.SetActive(active);
    }
}

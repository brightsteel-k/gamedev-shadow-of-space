using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    public delegate void WorldEvent();
    public static event Action<bool> OnWorldPivot;
    public static event WorldEvent OnTilePosChanged;

    public static void WorldPivot(bool clockwise)
    {
        if (OnWorldPivot != null)
            OnWorldPivot.Invoke(clockwise);
    }

    public static void TilePosChanged()
    {
        if (OnTilePosChanged != null)
            OnTilePosChanged.Invoke();
    }
}
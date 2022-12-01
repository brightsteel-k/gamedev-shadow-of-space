using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void WorldEvent();
    public static event WorldEvent OnWorldPivot;
    public static event WorldEvent OnTilePosChanged;

    public static void WorldPivot()
    {
        if (OnWorldPivot != null)
            OnWorldPivot.Invoke();
    }

    public static void TilePosChanged()
    {
        if (OnTilePosChanged != null)
            OnTilePosChanged.Invoke();
    }
}

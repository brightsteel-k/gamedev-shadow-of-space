using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    public static bool PLAYER_DYING = false;

    public delegate void WorldEvent();
    public static event Action<bool> OnWorldPivot;
    public static event WorldEvent OnTilePosChanged;
    public static event WorldEvent OnPlayerDying;

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

    public static void PlayerDying()
    {
        PLAYER_DYING = true;
        if (OnPlayerDying != null)
            OnPlayerDying.Invoke();
    }

    public static void ResetWorldEvents()
    {
        OnWorldPivot = null;
        OnTilePosChanged = null;
        OnPlayerDying = null;
    }
}
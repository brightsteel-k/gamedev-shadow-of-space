using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void WorldEvent();
    public static event WorldEvent OnWorldPivot;

    public void WorldPivot()
    {
        if (OnWorldPivot != null)
        {
            OnWorldPivot.Invoke();
        }
    }
}

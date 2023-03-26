using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTester : MonoBehaviour
{
    //This class is used to simulate function calls for some of the functions. Place this on a "tester" object.

    public Health hp;
    public Energy en;

    public Inventory inv;

    public ResourceCrafter res;

    public Item toAdd;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            hp.removeHealth(20);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTester : MonoBehaviour
{
    //This class is used to simulate function calls for some of the functions. Place this on a "tester" object.

    public Health hp;
    public Energy en;

    void Update()
    {
        //Press K to start the tests
        if (Input.GetKeyDown(KeyCode.K))
        {
            hp.setHealth(10);
            en.draining = false;
            en.setEnergy(80);
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            
            hp.addHealth(30);
            en.draining = true;
        }

        //Test should print "true"
        if (Input.GetKeyDown(KeyCode.L))
        {
            hp.removeHealth(80);
            Debug.Log(hp.dead);
            en.draining = false;
        }
    }
}
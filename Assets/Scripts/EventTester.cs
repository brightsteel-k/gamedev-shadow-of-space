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
        //Test should print "true"
        if (Input.GetKeyDown(KeyCode.L))
        {
            hp.removeHealth(80);
            Debug.Log(hp.dead);
            en.draining = false;
        }
        
        //Just a helpful little bit to change cursor lock for testing;

        if (Input.GetKeyDown(KeyCode.M))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            Item battery = ScriptableObject.CreateInstance<Item>().Initialize(Inventory.ALL_ITEMS["battery"], ("power", 80));
            Player.INVENTORY.addItem(battery);
        }

        if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            Player.INVENTORY.addItem(Inventory.ALL_ITEMS["drill"]);
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            inv.addItem(toAdd);
        }

        
    }
}

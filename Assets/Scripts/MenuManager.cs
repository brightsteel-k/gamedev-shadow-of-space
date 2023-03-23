using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public CraftingUI craft;
    public GameObject HoverBox;
    //For the "inMenu" thing.
    public Player player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    //Menu Manager to manage when UI's start up (if they start inactive, their scripts can't activate themselves
    //I think
    void Update()
    {
        //@TODO change this to the key to open the crafting
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (Player.IN_MENU)
                SetCraftingMenuActive(false);
            else
                SetCraftingMenuActive(true);
        }

        //@TODO use the unity input manager to make this a "cancel". Maybe make it a toggle menu?
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            craft.SetMenuActive(false);
        }
    }
 
    public void SetCraftingMenuActive(bool active)
    {
        craft.SetMenuActive(active);
        MuseSystem.SetMusable(!active);
        Player.WORLD_PLAYER.SetInMenu(active);
    }
}

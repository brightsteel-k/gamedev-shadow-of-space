using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static bool CAN_CRAFT;
    public CraftingUI craft;
    public GameObject HoverBox;
    //For the "inMenu" thing.
    public Player player;

    // Start is called before the first frame update
    void Start()
    {
        CAN_CRAFT = true;
        EventManager.OnPlayerDying += DeactivateCrafting;
        EventManager.OnGameWinning += DeactivateCrafting;
    }

    //Menu Manager to manage when UI's start up (if they start inactive, their scripts can't activate themselves
    //I think
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && CAN_CRAFT)
        {
            if (Player.IN_MENU)
                SetCraftingMenuActive(false);
            else
                SetCraftingMenuActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Escape) && Player.IN_MENU)
        {
            SetCraftingMenuActive(false);
        }
    }
 
    public void SetCraftingMenuActive(bool active)
    {
        craft.SetMenuActive(active);
        MuseSystem.SetMusable(!active);
        Player.WORLD_PLAYER.SetInMenu(active);
    }

    public void DeactivateCrafting()
    {
        craft.SetMenuActive(false);
        CAN_CRAFT = false;
    }
}

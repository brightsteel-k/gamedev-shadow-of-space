using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject CraftingMenu;

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
        if (Input.GetKeyDown(KeyCode.C))
        {
            CraftingMenu.SetActive(true);
            player.inMenu = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        //@TODO use the unity input manager to make this a "cancel". Maybe make it a toggle menu?
        if (Input.GetKeyDown(KeyCode.E))
        {
            CraftingMenu.SetActive(false);
            HoverBox.SetActive(false);
            player.inMenu = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

 
    }
 
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject CraftingMenu;
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
            if (!CraftingMenu.activeSelf)
            {
                CraftingMenu.SetActive(true);
                Player.IN_MENU = true;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                CraftingMenu.SetActive(false);
                Player.IN_MENU = false;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        //@TODO use the unity input manager to make this a "cancel". Maybe make it a toggle menu?
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CraftingMenu.SetActive(false);
            HoverBox.SetActive(false);
            Player.IN_MENU = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        
        //@TODO change this to key that drops or uses items (for crafting menu)
        //@TODO maybe instead have it automatically update thru the inventory.
        if (Input.GetKeyDown(KeyCode.H) || Input.GetKeyDown(KeyCode.U))
        {
            craft.show();
        }
        //Alternatively, could just do this every fram
        //craft.show();
 
    }
 
}

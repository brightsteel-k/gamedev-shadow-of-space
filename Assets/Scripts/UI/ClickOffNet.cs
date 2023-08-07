using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ClickOffNet : MonoBehaviour, IPointerClickHandler
{
    public MenuManager menuManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Player.IN_MENU)
            menuManager.SetCraftingMenuActive(false);
    }
}

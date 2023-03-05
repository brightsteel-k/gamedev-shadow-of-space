using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
public class RecipeUI : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    
    public Recipe recipe;
    public TMP_Text recipeName;
    private Item result;
    public Image icon;
    public HoverBox hover;
    private Inventory inv;
    public void prepareDisplay(Recipe newRecipe, Inventory _inv)
    {
        recipe = newRecipe;
        result = recipe.created;
        icon.sprite = result.sprite;
        inv = _inv;
        if (inv.canMakeRecipe(newRecipe))
        {
            icon.color = Color.white;
        }
        else
        {
            icon.color = Color.gray;
        }
        recipeName.text = newRecipe.created.type; //@TODO if type is changed to an enum, we must put a string here;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject.name == "ItemName")
        {
            return;
        }
        hover.create(recipe.needed, eventData, inv);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hover.disable();
    }

}

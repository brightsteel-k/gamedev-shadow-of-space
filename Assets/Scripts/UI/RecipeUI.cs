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

    private bool inBox;

    public void Start()
    {
        inBox = false;
    }
    
    //Having this in update, maybe not good idea @TODO
    public void Update()
    {
        if (inBox)
        {
            hover.move();
        }
    }
    public void prepareDisplay(Recipe newRecipe, Inventory _inv, ResourceCrafter rec)
    {
        recipe = newRecipe;
        result = recipe.created;
        icon.sprite = result.sprite;
        inv = _inv;
        if (inv.canMakeRecipe(newRecipe) && rec.canMake(newRecipe))
        {
            icon.color = Color.white;
        }
        else
        {
            icon.color = Color.gray;
        }
        recipeName.text = newRecipe.created.id; //@TODO if type is changed to an enum, we must put a string here;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        inBox = true;
        if (eventData.pointerCurrentRaycast.gameObject.name == "ItemName")
        {
            return;
        }
        hover.create(recipe.needed, recipe.liquids, eventData, inv);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        inBox = false;
        hover.disable();
    }

}

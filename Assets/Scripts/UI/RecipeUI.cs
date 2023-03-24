using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
public class RecipeUI : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    // Color modifier for unavailable recipes
    [SerializeField] private Color unavailableColor;
    public Recipe recipe;
    public TMP_Text recipeName;
    private Item result;
    public Image icon;
    public HoverBox hover;
    private Inventory inv;

    private bool pointerInBox;

    public void Start()
    {
        pointerInBox = false;
    }
    
    //Having this in update, maybe not good idea @TODO
    public void Update()
    {
        if (pointerInBox && hover.gameObject.activeSelf)
        {
            hover.move();
        }
    }
    public void prepareDisplay(Recipe newRecipe, Inventory _inv, ResourceCrafter rec)
    {
        recipe = newRecipe;
        result = recipe.created;
        inv = _inv;
        if (inv.canMakeRecipe(newRecipe) && rec.canMakeRecipe(newRecipe))
            icon.color = Color.white;
        else
            icon.color = unavailableColor;


        icon.sprite = result.sprite;
        recipeName.text = newRecipe.created.displayName.Replace("@p", "100");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        pointerInBox = true;
        if (eventData.pointerCurrentRaycast.gameObject.name == "ItemName")
        {
            return;
        }
        hover.create(recipe.needed, recipe.liquids, eventData, inv);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        pointerInBox = false;
        hover.disable();
    }

}

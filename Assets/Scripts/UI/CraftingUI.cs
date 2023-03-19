using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class CraftingUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    public List<Recipe> recipes;
    public TMP_Text recipeName;
    public Image icon;

    //Recipe prefab
    public GameObject recipePrefab;
    
    //The view to insert the recipes into.
    public RectTransform recipeView;

    //For liquid resources
    public ResourceCrafter liquids;
    
    //The tooltip hoverbox
    public GameObject hover;

    //Private fields
    private Recipe selectedRecipe;

    public RecipeUI craftButton;
    public Image buttonImage;
    
    public Inventory inv;

    void Start()
    {
        show();
    }
    
    public void show()
    {
        foreach (Transform child in recipeView.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        hover.SetActive(false);
        
        selectRecipe(recipes[0]);
        
        foreach (Recipe rec in recipes)
        {
            GameObject obj = Instantiate(recipePrefab, recipeView);
            var ui = obj.GetComponent<RecipeUI>();
            ui.prepareDisplay(rec, inv, liquids);
            ui.hover = hover.GetComponent<HoverBox>();
        }
    }

    void OnDisable()
    {
        hover.GetComponent<HoverBox>().disable();
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        //Debug.Log("Clicked: " + eventData.pointerCurrentRaycast.gameObject.name);
        RecipeUI clickedUI = eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<RecipeUI>();
        if (clickedUI == null)
        {
            return;
        }

        selectRecipe(clickedUI.recipe);
        

    }

    private void selectRecipe(Recipe recipe)
    {
        //@TODO may not need to set sprite/text, the button does it
        selectedRecipe = recipe;
        Item result = selectedRecipe.created;
        icon.sprite = result.sprite;
        recipeName.text = result.name;

        if (inv.canMakeRecipe(recipe))
        {
            buttonImage.color = Color.white;
        }
        else
        {
            buttonImage.color = Color.gray;
        }
        
        craftButton.prepareDisplay(recipe, inv, liquids); 
    }
    
    public void craft()
    {
        inv.makeRecipe(selectedRecipe);
        liquids.makeRecipe(selectedRecipe);
        show();
    }

}

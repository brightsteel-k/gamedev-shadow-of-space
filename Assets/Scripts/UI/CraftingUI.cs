using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class CraftingUI : MonoBehaviour, IPointerClickHandler
{
    //Recipe prefab
    public GameObject recipePrefab;
    
    //The view to insert the recipes into.
    public RectTransform recipeView;

    //For liquid resources
    public ResourceCrafter liquids;
    
    //The tooltip hoverbox
    public GameObject hover;

    // Color of craft button for unavailable recipes
    [SerializeField] private Color invalidCraftColor;

    //Private fields
    private List<Recipe> recipes;
    private Recipe selectedRecipe;

    private TMP_Text selectedName;
    private Image selectedIcon;

    private RecipeUI craftButton;
    private Image craftButtonImage;
    
    public Inventory inv;

    void Awake()
    {
        initializeRecipes();
        selectedName = transform.Find("RecipeName").GetComponent<TMP_Text>();
        selectedIcon = transform.Find("RecipeIcon").GetComponent<Image>();
        craftButton = transform.Find("CraftButton").GetComponent<RecipeUI>();
        craftButtonImage = transform.Find("CraftButton").GetComponent<Image>();
    }

    void Start()
    {
        adjustSize();
        show();
    }

    // Deals with logic for resizing to fit screen
    void adjustSize()
    {
        float scale = (Screen.height - 70) / 400f;
        transform.localScale = new Vector3(scale, scale, scale);
    }

    void initializeRecipes()
    {
        recipes = new List<Recipe>();
        recipes.AddRange(Resources.LoadAll<Recipe>("Scriptables/Recipes"));
    }

    public void show()
    {
        foreach (Transform child in recipeView.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        hover.SetActive(false);
        
        foreach (Recipe rec in recipes)
        {
            GameObject obj = Instantiate(recipePrefab, recipeView);
            RecipeUI ui = obj.GetComponent<RecipeUI>();
            ui.prepareDisplay(rec, inv, liquids);
            ui.hover = hover.GetComponent<HoverBox>();
        }
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        RecipeUI clickedUI = eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<RecipeUI>();
        if (clickedUI == null)
            return;

        selectRecipe(clickedUI.recipe);
    }

    private void selectRecipe(Recipe recipe)
    {
        selectedRecipe = recipe;
        Item result = recipe.created;
        selectedIcon.sprite = result.sprite;
        selectedName.text = result.displayName.Replace("@p", "100");

        if (inv.canMakeRecipe(recipe) && liquids.canMakeRecipe(selectedRecipe))
            craftButtonImage.color = Color.white;
        else
            craftButtonImage.color = invalidCraftColor;
        
        craftButton.prepareDisplay(recipe, inv, liquids); 
    }
    
    public void craft()
    {
        if (!inv.canMakeRecipe(selectedRecipe) || !liquids.canMakeRecipe(selectedRecipe))
            return;
        inv.makeRecipe(selectedRecipe);
        liquids.makeRecipe(selectedRecipe);

        if (selectedRecipe.created.isLiquid)
        {
            liquids.changeAmount(selectedRecipe.created.madeLiq.id, selectedRecipe.amount);
            liquids.selectResource(selectedRecipe.created.madeLiq.id);
        }
        
        show();
        selectRecipe(selectedRecipe);
    }

    public void SetMenuActive(bool active)
    {
        gameObject.SetActive(active);
        Player.IN_MENU = active;
        Cursor.visible = active;
        if (active)
        {
            selectRecipe(recipes[0]);
            show();
        }
        else
        {
            hover.GetComponent<HoverBox>().disable();
        }
    }
}

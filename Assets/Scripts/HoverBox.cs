using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverBox : MonoBehaviour
{
    public GameObject hoverObj;
    
    public GameObject componentPrefab;

    private RectTransform hover;

    public Canvas canvas;

    public Vector2 offset;
    //@TODO Setup as a co-routine

    void Start()
    {
       
    }
    
    //@TODO instead of passing in inventory, maybe add it through the editor
    public void create(List<Recipe.Pair> items, PointerEventData eventData, Inventory inv) 
    {
        hoverObj.SetActive(true);
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)canvas.transform, eventData.position, canvas.worldCamera, 
            out position);
        Vector2 output = new Vector2(position.x + 100, position.y - 50);
        hover = hoverObj.GetComponent<RectTransform>();
        hover.position = canvas.transform.TransformPoint(output);
            
        GameObject comp;
            
        foreach(Recipe.Pair item in items)
        {
            comp = Instantiate(componentPrefab, hover);
            ComponentUI compUI = comp.GetComponent<ComponentUI>();

            compUI.setValues(item.item.sprite, item.item.id, item.amount.ToString(), inv.countIn(item.item).ToString()); 
        }
            
    }

    public void move()
    {
        hover.position = new Vector2(Input.mousePosition.x + offset.x, Input.mousePosition.y + offset.y);
    }
    
    public void disable()
    {
            
        hoverObj.SetActive(false);
        foreach (Transform child in hover.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
    
}

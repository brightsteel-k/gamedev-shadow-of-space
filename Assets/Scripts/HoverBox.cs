using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverBox : MonoBehaviour
{
    public GameObject hoverObj;
    
    public GameObject componentPrefab;

    public GameObject resourceBarPrefab;
    
    private RectTransform hover;
    public Canvas canvas;

    public Sprite unknown;
    public Vector2 offset;
    //@TODO Setup as a co-routine

    void Start()
    {
       
    }
    
    //@TODO instead of passing in inventory, maybe add it through the editor
    public void create(List<Recipe.Pair1> items, List<Recipe.Pair2> liquids, PointerEventData eventData, Inventory inv) 
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
        GameObject rec;   
        
        foreach(Recipe.Pair1 item in items)
        {
            comp = Instantiate(componentPrefab, hover);
            ComponentUI compUI = comp.GetComponent<ComponentUI>();

            if(inv.isDiscovered(item.item)){
                compUI.setValues(item.item.sprite, item.item.id, item.amount.ToString(),
                    inv.countIn(item.item).ToString());
            }
            else
            {
                compUI.setValues(unknown, "???", item.amount.ToString(),
                    inv.countIn(item.item).ToString());
            }
        }
        
        foreach(Recipe.Pair2 resource in liquids)
        {
            Instantiate(resourceBarPrefab, canvas.transform);
            rec = Instantiate(resourceBarPrefab, hover);
            BarManager bar = rec.GetComponent<BarManager>();

            bar.setValue(resource.amount);
            bar.setMaximum(resource.res.maximum);
            bar.setColor(resource.res.color);
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

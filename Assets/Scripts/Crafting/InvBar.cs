using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InvBar : MonoBehaviour
{
    public List<Image> images;

    public ItemSelector selector;
    //This sprite should be what an empty inventory slot looks like (maybe just transparent?)
    public Sprite empty;
    
    // Start is called before the first frame update
    private void Start()
    {
        adjustSize();
    }

    // Deals with logic for resizing to fit screen
    void adjustSize()
    {
        float barHeight = Screen.height - 60;
        float scale = barHeight / 545f;

        transform.localScale = new Vector3(scale, scale, scale);
    }


    public void updateBar(List<Item> items)
    {
        for (int i = 0; i < images.Count; i++)
        {
            if (i < items.Count)
            {
                images[i].sprite = items[i].sprite; 
            }
            else
            {
                images[i].sprite = null;
            }
        }
        selector.displayIdentifier();
    }
}

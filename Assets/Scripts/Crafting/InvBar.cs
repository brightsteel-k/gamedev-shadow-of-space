using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InvBar : MonoBehaviour
{
    public List<Image> images;

    public Sprite empty;
    // Start is called before the first frame update
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
                images[i].sprite = empty;
            }
        }
    }
}

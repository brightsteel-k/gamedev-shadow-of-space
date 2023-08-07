using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InvBar : MonoBehaviour
{
    private static float SCALE;
    public List<Image> images;

    public ItemSelector selector;
    //This sprite should be what an empty inventory slot looks like (maybe just transparent?)
    public Sprite empty;
    private AudioSource audioSource;
    private AudioClip clickClip;
    
    private void Awake()
    {
        adjustSize();
    }

    // Deals with logic for resizing to fit screen
    void adjustSize()
    {
        float barHeight = Screen.height - 20;
        SCALE = barHeight / 545f;

        transform.localScale = new Vector3(SCALE, SCALE, SCALE);
    }

    public static float GetScale()
    {
        return SCALE;
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        clickClip = Resources.Load<AudioClip>("Sounds/SwitchInventorySlot");
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
        selector.UpdateIdentifiers();
    }
    
    public void ShiftSelector(bool up)
    {
        PlayUIClick();
        selector.ShiftPosition(up);
        Player.WORLD_PLAYER.UpdateSelectedItem();
    }

    public void PlayUIClick()
    {
        audioSource.PlayOneShot(clickClip);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemSelector : MonoBehaviour
{
    // Inevntory to which this is attached
    [SerializeField] Inventory inventory;

    //The current position of the selector
    public int pos;

    //This is how much it changes at each position (should be roughly the height of a single inventory spot
    public float offset;

    //Position 0 of the bar.
    public float pos0;

    // Text beside selector
    [SerializeField] TextMeshProUGUI identifier;
    float identifierAlpha;
    [SerializeField] float fadeRate;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            setSelectorTo(pos - 1);
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            setSelectorTo(pos + 1);
        }

        updateIdentifierAlpha();
    }

    public void setSelectorTo(int newPos)
    {
        while (newPos >= inventory.barLength)
        {
            newPos -= inventory.barLength;
        }

        if (newPos < 0)
        {
            newPos = inventory.barLength - 1;
        }

        pos = newPos;

        transform.localPosition = new Vector2(transform.localPosition.x, pos0 - pos * offset);
        displayIdentifier();
    }

    public void displayIdentifier()
    {
        Item selectedItem = inventory.getSelectedItem();
        if (selectedItem == null)
        {
            if (identifier.text != "")
                identifier.SetText("");
            return;
        }
        identifier.SetText(selectedItem.displayName);
        identifierAlpha = 255f;
    }

    private void updateIdentifierAlpha()
    {
        if (identifierAlpha == 0)
            return;
        if (identifierAlpha < 0)
        {
            identifier.alpha = 0;
            identifierAlpha = 0;
            return;
        }

        identifierAlpha -= fadeRate * Time.deltaTime / identifierAlpha;
        identifier.color = new Color32(255, 255, 255, (byte)identifierAlpha);
    }
}

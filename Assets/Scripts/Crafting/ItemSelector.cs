using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Newtonsoft.Json;

public class ItemSelector : MonoBehaviour
{
    // Inventory to which this is attached
    [SerializeField] Inventory inventory;

    // Instructions to appear at the bottom of the screen for right-clickable items
    private Dictionary<string, string> allInstructions;
    private string currentInstructions = "";
    [SerializeField] TextMeshProUGUI instructionsText;

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
        LoadInstructions();
    }

    void LoadInstructions()
    {
        TextAsset ta = Resources.Load<TextAsset>("Data/item_instructions");
        allInstructions = JsonConvert.DeserializeObject<Dictionary<string, string>>(ta.text);
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
        updateIdentifiers();
    }

    public void updateIdentifiers()
    {
        // Name of item
        Item selectedItem = inventory.getSelectedItem();
        if (selectedItem == null)
        {
            if (identifier.text != "")
                identifier.SetText("");
            if (currentInstructions != "")
            {
                setInstructionsText("", "");
            }
            return;
        }
        identifier.SetText(formatDisplayName(selectedItem));
        identifierAlpha = 255f;

        // Instructions at bottom of screen
        if (allInstructions.TryGetValue(selectedItem.id, out string output))
        {
            if (selectedItem.id != currentInstructions)
            {
                setInstructionsText(selectedItem.id, output);
            }
        }
        else if (currentInstructions != "")
        {
            setInstructionsText("", "");
        }

        Player.WORLD_PLAYER.UpdateSelectedItem();
    }

    private void setInstructionsText(string id, string text)
    {
        if (id == "")
        {
            instructionsText.gameObject.SetActive(false);
        }
        else
        {
            instructionsText.SetText(text);
            if (currentInstructions == "")
                instructionsText.gameObject.SetActive(true);
        }
        currentInstructions = id;
    }

    private string formatDisplayName(Item item)
    {
        string output = item.displayName;
        if (output.Contains("@p"))
            output = output.Replace("@p", item.GetTag("power").ToString("0"));
        return output;
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

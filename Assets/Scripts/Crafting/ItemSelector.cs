using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSelector : MonoBehaviour
{
    //The current position of the selector
    public int pos;

    //The size of the inventory
    public int invSize;

    //This is how much it changes at each position (should be roughly the height of a single inventory spot
    public float offset;

    //Position 0 of the bar.
    public float pos0;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(transform.localPosition.y);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.mouseScrollDelta.y > 0)
        {
            setSelectorTo(pos + 1);
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            setSelectorTo(pos - 1);
        }
    }

    public void setSelectorTo(int newPos)
    {
        while (newPos >= invSize)
        {
            newPos -= invSize;
        }

        if (newPos < 0)
        {
            newPos = invSize - 1;
        }

        pos = newPos;

        transform.localPosition = new Vector2(transform.localPosition.x, pos0 - pos * offset);

    }
}

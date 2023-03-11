using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class ItemTextures : MonoBehaviour
{
    static Dictionary<string, int> QUANTITY;

    void Start()
    {
        TextAsset t = Resources.Load<TextAsset>("Textures/Items/quantities");
        QUANTITY = JsonConvert.DeserializeObject<Dictionary<string, int>>(t.text);
        Debug.Log(QUANTITY["hematite_pebble"]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static Sprite GetItemTexture(string id)
    {
        int i = 0;
        if (QUANTITY[id] > 1)
        {
            i = Random.Range(0, QUANTITY[id]);
        }
        return Resources.Load<Sprite>("Textures/Items/" + id + "_" + i);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class ItemTextures : MonoBehaviour
{
    static Dictionary<string, int> QUANTITIES;
    static Dictionary<string, float> SIZES;

    void Start()
    {
        TextAsset t1 = Resources.Load<TextAsset>("Textures/Items/quantities");
        QUANTITIES = JsonConvert.DeserializeObject<Dictionary<string, int>>(t1.text);
        TextAsset t2 = Resources.Load<TextAsset>("Textures/Items/sizes");
        SIZES = JsonConvert.DeserializeObject<Dictionary<string, float>>(t2.text);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static Sprite GetItemTexture(string id)
    {
        int i = 0;
        if (QUANTITIES[id] > 1)
        {
            i = Random.Range(0, QUANTITIES[id]);
        }
        return Resources.Load<Sprite>("Textures/Items/" + id + "_" + i);
    }

    public static Sprite[] GetDirectionalTextures(string id, out int initialDirection)
    {
        Sprite[] result = new Sprite[4];
        for (int i = 0; i < 4; i++)
        {
            result[i] = Resources.Load<Sprite>("Textures/Features/" + id + "_" + i);
        }
        initialDirection = Random.Range(0, 4);
        return result;
    }

    public static float GetItemSize(string id)
    {
        return SIZES[id];
    }
}

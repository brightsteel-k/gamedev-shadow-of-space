using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class ItemResources : MonoBehaviour
{
    static Dictionary<string, int> QUANTITIES;
    static Dictionary<string, float> SIZES;
    static Dictionary<string, AudioClip> PICKUP_SOUNDS = new Dictionary<string, AudioClip>();
    public static List<string> RARE_RESOURCES_DROPPED = new List<string>();
    void Start()
    {
        TextAsset t1 = Resources.Load<TextAsset>("Textures/Items/quantities");
        QUANTITIES = JsonConvert.DeserializeObject<Dictionary<string, int>>(t1.text);
        TextAsset t2 = Resources.Load<TextAsset>("Textures/Items/sizes");
        SIZES = JsonConvert.DeserializeObject<Dictionary<string, float>>(t2.text);
        RARE_RESOURCES_DROPPED.Clear();
        if (PICKUP_SOUNDS.Count == 0)
            RegisterSounds();
    }

    private void RegisterSounds()
    {
        RegisterSound("ValuablePickup");
        RegisterSound("RockPickup");
        RegisterSound("ItemPickup");
        RegisterSound("MushroomPickup");
        RegisterSound("RockBreak");
    }

    private void RegisterSound(string name)
    {
        PICKUP_SOUNDS.Add(name, Resources.Load<AudioClip>("Sounds/" + name));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static Sprite GetItemTexture(string id, out float height)
    {
        int i = 0;
        if (QUANTITIES[id] > 1)
        {
            i = Random.Range(0, QUANTITIES[id]);
        }
        Sprite img = Resources.Load<Sprite>("Textures/Items/" + id + "_" + i);
        height = CalculateItemHeight(id, img.texture.height, img.pixelsPerUnit);
        return img;
    }

    private static float CalculateItemHeight(string id, int imgHeight, float imgPixelsPerUnit)
    {
        switch (id)
        {
            case "lithium":
                return 0.283f;
            case "ferritic_ingot":
                return 0.416f;
            default:
                return imgHeight / imgPixelsPerUnit / 12 / Mathf.Sqrt(2);
        }
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

    public static AudioClip GetInteractClip(string id)
    {
        string sound;
        switch (id)
        {
            case "gold_piece":
                sound = "ValuablePickup";
                break;
            case "diamond":
            case "hematite_pebble":
            case "tyranite_pebble":
            case "lithium":
                sound = "RockPickup";
                break;
            case "harvest_mushroom":
                sound = "MushroomPickup";
                break;
            case "shatter_stalagmite":
            case "shatter_tyranite":
            case "shatter_auralite":
                sound = "RockBreak";
                break;
            default:
                sound = "ItemPickup";
                break;
        }

        return PICKUP_SOUNDS[sound];
    }
}

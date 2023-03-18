using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class Environment : MonoBehaviour
{
    static Environment INSTANCE;
    static Quaternion spriteTilt = Quaternion.Euler(45, 0, 0);
    public static Dictionary<string, Biome> Biomes = new Dictionary<string, Biome>() { };
    public static Dictionary<string, GameObject> WORLD_OBJECTS = new Dictionary<string, GameObject>();

    private void Awake()
    {
        LeanTween.init(800);
    }

    // Start is called before the first frame update
    void Start()
    {
        INSTANCE = GetComponent<Environment>();
        InitObjectPrefabs();
    }
    public static void InitBiomes()
    {
        TextAsset[] files = Resources.LoadAll<TextAsset>("Data/Biomes");
        foreach (TextAsset t in files)
            Biomes.Add(t.name, JsonConvert.DeserializeObject<Biome>(t.text));
    }

    private static void InitObjectPrefabs()
    {
        foreach (GameObject g in Resources.LoadAll<GameObject>("Prefabs/WorldObjects"))
            WORLD_OBJECTS.Add(g.name, g);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static WorldObject[] PopulateChunk(Vector3 posIn, List<(string, float)> features)
    {
        List<WorldObject> allFeatures = new List<WorldObject>();
        foreach ((string, float) f in features)
        {
            switch (f.Item1)
            {
                case "diamond":
                    AddRareItems(allFeatures, posIn, f.Item1, f.Item2, 3);
                    break;
                case "stalagmite":
                    AddStalagmites(allFeatures, posIn, "hematite_stalagmite", f.Item2, "hematite_pebble", 3);
                    break;
                case "tyranite":
                    AddStalagmites(allFeatures, posIn, "tyranite", f.Item2, "tyranite_pebble", 1);
                    break;
                default:
                    AddSmallFeatures(allFeatures, posIn, f.Item1, RandomGen.Mercury(f.Item2));
                    break;
            }
        }
        return allFeatures.ToArray();
    }

    public static void AddSmallFeatures(List<WorldObject> allFeatures, Vector3 posIn, string obj, float count)
    {
        for (int k = 0; k < count; k++)
        {
            Vector3 pos = RandomGen.GetPos(GenType.NaiveRandom, posIn.x, posIn.z);
            WorldObject feature = Instantiate(WORLD_OBJECTS[obj], pos, spriteTilt, INSTANCE.transform).GetComponent<WorldObject>();
            feature.InitSprite();
            allFeatures.Add(feature.Place());
        }
    }

    public static void AddRareItems(List<WorldObject> allFeatures, Vector3 posIn, string obj, float rarity, int degree)
    {
        int c = RandomGen.GetCountFromRarity(rarity, degree);
        for (int k = 0; k < c; k++)
        {
            Vector3 pos = RandomGen.GetPos(GenType.NaiveRandom, posIn.x, posIn.z);
            PlaceItem(allFeatures, pos, obj);
        }
    }

    public static void AddStalagmites(List<WorldObject> allFeatures, Vector3 posIn, string obj, float count, string pebbleId, int pebbleCount)
    {
        int c = RandomGen.GetCountFiftyPercent(count);
        for (int k = 0; k < c; k++)
        {
            Vector3 pos = RandomGen.GetPos(GenType.NaiveRandom, posIn.x, posIn.z);
            WorldObject feature = Instantiate(WORLD_OBJECTS[obj], pos, Quaternion.identity, INSTANCE.transform).GetComponent<WorldObject>();
            feature.InitSprite();
            allFeatures.Add(feature.Place());
            AddItemCluster(allFeatures, pos, pebbleId, pebbleCount);
        }
    }

    public static void AddItemCluster(List<WorldObject> allFeatures, Vector3 posIn, string obj, int count)
    {
        int c = RandomGen.MaybeMinusOne(count);
        for (int k = 0; k < c; k++)
        {
            Vector3 pos = RandomGen.GetPos(GenType.Dense, posIn.x, posIn.z);
            PlaceItem(allFeatures, pos, obj);
        }
    }

    public static void PlaceItem(List<WorldObject> allFeatures, Vector3 posIn, string id)
    {
        ItemObject feature = Instantiate(WORLD_OBJECTS["item"], posIn, Quaternion.identity, INSTANCE.transform).GetComponent<ItemObject>();
        feature.InitItem(id, ItemTextures.GetItemSize(id));
        allFeatures.Add(feature.Place());
    }

    public static void DropItem(string item, Vector3 posIn)
    {
        ItemObject feature = Instantiate(WORLD_OBJECTS["item"], posIn, Quaternion.identity, INSTANCE.transform).GetComponent<ItemObject>();
        feature.InitItem(item, ItemTextures.GetItemSize(item));
        feature.GetComponent<Rigidbody>().AddForce(RandomGen.DropItemMomentum(), ForceMode.Impulse);
        AddItem(feature, posIn);
    }

    public static void AddItem(ItemObject item, Vector3 posIn)
    {
        ChunkHandler.GetChunk(posIn).AddFeature(item);
    }
}

public enum GenType
{
    NaiveRandom,
    Dense
}
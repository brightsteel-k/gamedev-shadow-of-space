using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class Environment : MonoBehaviour
{
    public static Environment INSTANCE;
    static Quaternion spriteTilt = Quaternion.Euler(45, 0, 0);
    public static Dictionary<string, Biome> Biomes = new Dictionary<string, Biome>() { };
    public static Dictionary<string, GameObject> WORLD_OBJECTS = new Dictionary<string, GameObject>();

    private void Awake()
    {
        
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

    public static void PopulateChunk(Vector3 posIn, List<(string, float)> toAdd, List<WorldObject> allFeatures, Vector3Int tilePos)
    {
        RandomGen.NewChunk();
        foreach ((string, float) f in toAdd)
        {
            switch (f.Item1)
            {
                case "tyranite":
                    AddTyraniteChunk(allFeatures, f.Item2, posIn, tilePos);
                    break;
                case "stalagmite":
                    AddStalagmites(allFeatures, posIn, "hematite_stalagmite", RandomGen.GetCountFiftyPercent(f.Item2), "hematite_pebble", 3);
                    break;
                case "auralite":
                    AddStalagmites(allFeatures, posIn, "auralite", RandomGen.GetCountFromAbundance(f.Item2, 2), "gold_piece", 0);
                    break;
                case "mushroom":
                    AddClusteredFeature(allFeatures, posIn, f.Item1, f.Item2);
                    break;
                case "gold_piece":
                case "diamond":
                    AddSmallItems(allFeatures, posIn, f.Item1, RandomGen.GetCountFromAbundance(f.Item2, 3));
                    break;
                default:
                    AddSmallFeatures(allFeatures, posIn, f.Item1, RandomGen.Mercury(f.Item2));
                    break;
            }
        }
    }

    public static void AddTyraniteChunk(List<WorldObject> allFeatures, float count, Vector3 posIn, Vector3Int tilePos)
    {
        if (!RandomGen.ShouldChunkFeatureGenerate(tilePos, 17, 14, 23))
            return;
        AddStalagmites(allFeatures, posIn, "tyranite", RandomGen.Range(2, (int)count), "tyranite_pebble", 0);
        AddSmallItems(allFeatures, posIn, "tyranite_pebble", RandomGen.Mercury(8)); 
        
    }
    
    public static void AddSmallFeatures(List<WorldObject> allFeatures, Vector3 posIn, string obj, float count)
    {
        for (int k = 0; k < count; k++)
        {
            try
            {
                Vector3 pos = RandomGen.GetPosInChunk(GenType.NaiveRandom, posIn.x, posIn.z);
                PlaceFeature(allFeatures, pos, obj);
            }
            catch (CancelledChunkPosException)
            {
                continue;
            }
        }
    }

    public static void AddClusteredFeature(List<WorldObject> allFeatures, Vector3 posIn, string obj, float abundance)
    {
        if (RandomGen.Range(0f, 1f) > abundance)
        {
            Vector3 centre;
            try
            {
                centre = RandomGen.GetPosInChunk(GenType.NaiveRandom, posIn.x, posIn.z);
            }
            catch (CancelledChunkPosException)
            {
                return;
            }
            
            int c = RandomGen.Mercury(3);
            for (int k = 0; k < c; k++)
            {
                try
                {
                    Vector3 pos = RandomGen.GetPosInChunk(GenType.Dense, centre.x, centre.z);
                    PlaceFeature(allFeatures, pos, obj);
                }
                catch (CancelledChunkPosException)
                {
                    continue;
                }
            }
        }
    }

    public static void AddStalagmites(List<WorldObject> allFeatures, Vector3 posIn, string obj, int c, string pebbleId, int pebbleCount)
    {
        for (int k = 0; k < c; k++)
        {
            try
            {
                Vector3 pos = RandomGen.GetPosInChunk(GenType.NaiveRandom, posIn.x, posIn.z);
                PlaceFeature(allFeatures, pos, obj);
                AddItemCluster(allFeatures, pos, pebbleId, pebbleCount);
            }
            catch (CancelledChunkPosException)
            {
                continue;
            }
        }
    }

    public static void AddItemCluster(List<WorldObject> allFeatures, Vector3 posIn, string item, int count)
    {
        int c = RandomGen.MaybeMinusOne(count);
        for (int k = 0; k < c; k++)
        {
            try
            {
                Vector3 pos = RandomGen.GetPosInChunk(GenType.Dense, posIn.x, posIn.z);
                PlaceItem(allFeatures, pos, Inventory.ALL_ITEMS[item]);
            }
            catch (CancelledChunkPosException)
            {
                continue;
            }
        }
    }

    public static void AddSmallItems(List<WorldObject> allFeatures, Vector3 posIn, string item, int c)
    {
        for (int k = 0; k < c; k++)
        {
            try
            {
                Vector3 pos = RandomGen.GetPosInChunk(GenType.NaiveRandom, posIn.x, posIn.z);
                PlaceItem(allFeatures, pos, Inventory.ALL_ITEMS[item]);
            }
            catch (CancelledChunkPosException)
            {   
                continue;
            }
        }
    }

    public static void PlaceFeature(List<WorldObject> allFeatures, Vector3 posIn, string obj)
    {
        WorldObject feature = Instantiate(WORLD_OBJECTS[obj], posIn, Quaternion.identity, INSTANCE.transform).GetComponent<WorldObject>();
        feature.InitSprite();
        feature.Place(allFeatures);
    }
    public static void PlaceItem(List<WorldObject> allFeatures, Vector3 posIn, Item item)
    {
        ItemObject feature = Instantiate(WORLD_OBJECTS["item"], posIn, Quaternion.identity, INSTANCE.transform).GetComponent<ItemObject>();
        feature.InitItem(item, ItemResources.GetItemSize(item.id));
        feature.Place(allFeatures);
    }

    public static void DropItem(string item, Vector3 posIn)
    {
        DropItem(Inventory.ALL_ITEMS[item], posIn);
    }

    public static void DropItem(Item item, Vector3 posIn)
    {
        ItemObject feature = Instantiate(WORLD_OBJECTS["item"], posIn, Quaternion.identity, INSTANCE.transform).GetComponent<ItemObject>();
        feature.InitItem(item, ItemResources.GetItemSize(item.id));
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
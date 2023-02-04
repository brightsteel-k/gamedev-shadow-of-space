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
                    AddRareFeature(allFeatures, posIn, f.Item1, f.Item2, 3);
                    break;
                case "stalagmite":
                    AddStalagmite(allFeatures, posIn, f.Item1, f.Item2);
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
            allFeatures.Add(Instantiate(WORLD_OBJECTS[obj], pos, spriteTilt, INSTANCE.transform).GetComponent<WorldObject>());
        }
    }

    public static void AddRareFeature(List<WorldObject> allFeatures, Vector3 posIn, string obj, float rarity, int degree)
    {
        int c = RandomGen.GetCountFromRarity(rarity, degree);
        AddSmallFeatures(allFeatures, posIn, obj, c);
    }

    public static void AddStalagmite(List<WorldObject> allFeatures, Vector3 posIn, string obj, float count)
    {

    }
}

public enum GenType
{
    NaiveRandom
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour
{
    static Environment INSTANCE;
    static Quaternion spriteTilt = Quaternion.Euler(45, 0, 0);

    public static Dictionary<string, GameObject> WORLD_OBJECTS = new Dictionary<string, GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        INSTANCE = GetComponent<Environment>();
        LoadObjectPrefabs();
    }

    void LoadObjectPrefabs()
    {
        foreach (GameObject g in Resources.LoadAll<GameObject>("Prefabs/WorldObjects"))
        {
            WORLD_OBJECTS.Add(g.name, g);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static WorldObject PopulateChunk(Vector3 posIn, string obj)
    {
        Vector3 pos = RandomGen.GetPos(GenType.NaiveRandom, posIn.x, posIn.z, 25f);
        return Instantiate(WORLD_OBJECTS[obj], pos, spriteTilt, INSTANCE.transform).GetComponent<WorldObject>();
    }

    public static WorldObject[] PopulateSmallFeatures(Vector3 posIn, string obj)
    {
        WorldObject[] returnObjs = new WorldObject[8];

        for (int k = 0; k < 8; k++)
        {
            Vector3 pos = RandomGen.GetPos(GenType.NaiveRandom, posIn.x, posIn.z, 25f);
            returnObjs[k] = Instantiate(WORLD_OBJECTS[obj], pos, spriteTilt, INSTANCE.transform).GetComponent<WorldObject>();
        }

        return returnObjs;
    }
}

public enum GenType
{
    NaiveRandom
}
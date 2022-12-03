using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour
{
    static Environment Instance;
    static Quaternion spriteTilt = Quaternion.Euler(45, 0, 0);

    public static Dictionary<string, GameObject> WorldObjects = new Dictionary<string, GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        Instance = GetComponent<Environment>();
        LoadObjectPrefabs();
    }

    void LoadObjectPrefabs()
    {
        foreach (GameObject g in Resources.LoadAll<GameObject>("Prefabs/WorldObjects"))
        {
            WorldObjects.Add(g.name, g);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static WorldObject PopulateChunk(Vector3 posIn, string obj)
    {
        Vector3 centre = new Vector3(posIn.x, 0.5f, posIn.z);
        return Instantiate(WorldObjects[obj], centre, spriteTilt, Instance.transform).GetComponent<WorldObject>();
    }
}
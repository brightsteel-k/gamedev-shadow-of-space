using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Chunk
{
    public static float WIDTH = 20f;
    public bool active = false;
    public bool initialized = false;
    public Biome biome;
    public Vector3 worldPos;
    List<WorldObject> features = new List<WorldObject>();
    Vector3Int pos;

    public Chunk(int x, int z, Biome b)
    {
        pos = new Vector3Int(x, z);
        worldPos = new Vector3(x * WIDTH, 0f, z * WIDTH);
        biome = b;
    }

    void InitChunk()
    {
        Environment.PopulateChunk(worldPos, biome.Features, features);

        initialized = true;
        active = true;
    }

    public void LoadChunk()
    {
        if (active)
            return;
        if (initialized)
            SetChunkActive(true);
        else
            InitChunk();
    }

    public void SetChunkActive(bool activeIn)
    {
        foreach (WorldObject w in features)
        {
            w.SetActive(activeIn);
        }
        active = activeIn;
    }

    public void AddFeature(WorldObject feature)
    {
        features.Add(feature);
        feature.SetChunkRegistry(features);
    }

    // @TODO: Debug method to check chunks
    public void PrintFeatures()
    {
        string s = "Features:";
        foreach (WorldObject f in features)
        {
            s += "\n" + f.name;
        }
        s += "\nCount: " + features.Count;
        Debug.Log(s);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Chunk
{
    public bool active = false;
    public bool initialized = false;
    public Biome biome;
    public Vector3 worldPos;
    List<WorldObject> features = new List<WorldObject>();
    Vector3Int pos;


    public Chunk(int x, int z, Tile tile)
    {
        pos = new Vector3Int(x, z);
    }

    void InitChunk()
    {
        Environment.PopulateChunk(worldPos, "grass");

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
        if (active == activeIn)
            return;

        foreach (WorldObject w in features)
        {
            w.SetActive(active);
        }
        active = activeIn;
    }
}

public enum Biome
{
    VioletWastes,
    BlackSea,
    MercuryPools
}
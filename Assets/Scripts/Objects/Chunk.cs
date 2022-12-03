using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Chunk
{
    public bool Active = false;
    public bool Initialized = false;
    public Biome Biome;
    public Vector3 WorldPos;
    List<WorldObject> Features = new List<WorldObject>();
    Vector3Int Pos;


    public Chunk(int x, int z, Tile tile)
    {
        Pos = new Vector3Int(x, z);
    }

    void InitChunk()
    {
        Environment.PopulateChunk(WorldPos, "grass");

        Initialized = true;
        Active = true;
    }

    public void LoadChunk()
    {
        if (Active)
            return;
        if (Initialized)
            SetChunkActive(true);
        else
            InitChunk();
    }

    public void SetChunkActive(bool active)
    {
        if (active == Active)
            return;

        foreach (WorldObject w in Features)
        {
            w.SetActive(active);
        }
        Active = active;
    }
}

public enum Biome
{
    VioletWastes,
    BlackSea,
    MercuryPools
}
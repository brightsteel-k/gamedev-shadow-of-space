using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Chunk : Tile
{
    public bool Active = true;
    public bool Initialized = false;
    public Biome Biome;
    List<WorldObject> Features = new List<WorldObject>();
    Vector3Int pos;

    public Chunk(int x, int z)
    {
        pos = new Vector3Int(x, z, 0);
        InitAppearance();
    }

    void InitAppearance()
    {
        color = new Color(5, 66, 163, 255);
        sprite = Resources.Load<Sprite>("Textures/temp_ground");
    }

    void InitObjects(int radius)
    {
        if (Initialized)
            return;

        InitNeighbors(radius);
    }

    void InitNeighbors(int radius)
    {
        ChunkHandler.World.GetTile<Chunk>(pos + Vector3Int.right);
        ChunkHandler.World.GetTile<Chunk>(pos + Vector3Int.left);
        ChunkHandler.World.GetTile<Chunk>(pos + Vector3Int.up);
        ChunkHandler.World.GetTile<Chunk>(pos + Vector3Int.down);
    }

    public void LoadObjects()
    {
        
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

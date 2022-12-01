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

    public Chunk(int x, int z, Tile tile)
    {
        pos = new Vector3Int(x, z);
        
    }

    void SetIdentity()
    {

    }

    void InitObjects(int radius)
    {
        if (Initialized)
            return;

        InitNeighbors(radius);
    }

    void InitNeighbors(int radius)
    {
        if (radius == 0)
            return;

        ChunkHandler.World.GetChunk(pos + Vector3Int.right).InitObjects(radius - 1);
        ChunkHandler.World.GetChunk(pos + Vector3Int.left).InitObjects(radius - 1);
        ChunkHandler.World.GetChunk(pos + Vector3Int.up).InitObjects(radius - 1);
        ChunkHandler.World.GetChunk(pos + Vector3Int.down).InitObjects(radius - 1);
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

public enum Direction
{
    North,
    South,
    East,
    West
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Chunk
{
    public bool Active = true;
    public bool Initialized = false;
    public Biome Biome;
    List<WorldObject> Features = new List<WorldObject>();
    Vector3Int pos;

    static Direction[] Directions = { Direction.North, Direction.East, Direction.South, Direction.West };

    public Chunk(int x, int z, Tile tile)
    {
        pos = new Vector3Int(x, z);

    }

    void SetIdentity()
    {

    }

    void InitObjects()
    {
        if (Initialized)
            return;

        ChunkHandler.Tiles.GetTile<Tile>(pos).color = Color.green;


        Initialized = true;
        Active = true;
    }

    public void LoadChunk(int r, Direction d)
    {
        if (r == 0)
            return;

        if (!Active)
        {
            if (Initialized)
                LoadObjects();
            else
                InitObjects();
        }

        foreach (Direction dir in Directions)
        {
            if (dir != d)
                ChunkHandler.World.LoadChunk(pos, dir, r - 1, d);
        }
    }

    public void LoadObjects()
    {
        if (Active)
            return;

        Active = true;
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
    East,
    South,
    West
}
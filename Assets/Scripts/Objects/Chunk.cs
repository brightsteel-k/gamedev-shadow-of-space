using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk
{
    public bool Active;
    public Biome Biome;
    List<WorldObject> Features = new List<WorldObject>();

    public Chunk(Vector3 from, Vector3 to, Biome biomeIn)
    {
        
    }

    public void GenerateObjects()
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

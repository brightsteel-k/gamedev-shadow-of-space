using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Chunk : Tile
{
    public bool Active = true;
    public Biome Biome;
    List<WorldObject> Features = new List<WorldObject>();

    public Chunk()
    {
        InitAppearance();
    }

    void InitAppearance()
    {
        color = new Color(5, 66, 163, 255);
        sprite = Resources.Load<Sprite>("Textures/temp_ground");
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

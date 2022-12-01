using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ChunkMap
{
    Chunk[,] chunks = new Chunk[,] { };

    public ChunkMap(Tilemap terrain)
    {
        int xLen = terrain.size.x;
        int yLen = terrain.size.y;
        chunks = new Chunk[xLen, yLen];
        for (int x = 0; x < xLen; x++)
        {
            for (int y = 0; y < yLen; y++)
            {
                chunks[x,y] = new Chunk(x, y, terrain.GetTile<Tile>(new Vector3Int(x, y)));
            }
        }
    }

    public Chunk GetChunk(Vector3Int pos)
    {
        return chunks[pos.x, pos.y];
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ChunkHandler : MonoBehaviour
{
    Tilemap tilemap;  

    // Start is called before the first frame update
    void Start()
    {
        tilemap = GetComponent<Tilemap>();
        InitalizeChunks();
    }

    void InitalizeChunks()
    {
        tilemap.RefreshAllTiles();
        for (int x = -5; x < 5; x++)
        {
            for (int z = -5; z < 5; z++)
            {
                tilemap.SetTile(new Vector3Int(x, z), new Chunk());
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

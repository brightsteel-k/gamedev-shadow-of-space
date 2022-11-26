using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ChunkHandler : MonoBehaviour
{
    public static Tilemap World;  

    // Start is called before the first frame update
    void Start()
    {
        World = GetComponent<Tilemap>();
        InitalizeChunks();
    }

    void InitalizeChunks()
    {
        World.RefreshAllTiles();
        for (int x = -5; x < 5; x++)
        {
            for (int z = -5; z < 5; z++)
            {
                Chunk c = Instantiate(new Chunk(x, z));
                World.SetTile(new Vector3Int(x, z), c);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

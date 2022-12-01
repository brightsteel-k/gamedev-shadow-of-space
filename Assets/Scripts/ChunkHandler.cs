using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ChunkHandler : MonoBehaviour
{
    public static Tilemap Tiles;
    public static ChunkMap World;
    

    // Start is called before the first frame update
    void Start()
    {
        Tiles = GetComponent<Tilemap>();
        World = new ChunkMap(Tiles);
    }

    // Update is called once per frame
    void Update()
    {
        

    }
}

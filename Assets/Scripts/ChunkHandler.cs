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
        Tile t = ScriptableObject.CreateInstance<Tile>();
        t.color = new Color(5, 66, 163, 255);
        tilemap.SetTile(new Vector3Int(0, 0), t);        
        Debug.Log(tilemap.CellToWorld(new Vector3Int(0, 0)));
        tilemap.RefreshAllTiles();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

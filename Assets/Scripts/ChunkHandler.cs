using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ChunkHandler : MonoBehaviour
{
    int chunkUpdates = 0;
    public static Tilemap Tiles;
    public static Chunk[,] World;
    static int LoadRadius = 2;

    // Start is called before the first frame update
    void Start()
    {
        Tiles = GetComponent<Tilemap>();
        BuildWorld();
        EventManager.OnTilePosChanged += OnPlayerMove;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void BuildWorld()
    {
        int xLen = Tiles.size.x;
        int yLen = Tiles.size.y;
        World = new Chunk[xLen, yLen];
        for (int x = 0; x < xLen; x++)
        {
            for (int y = 0; y < yLen; y++)
            {
                Vector3Int p1 = new Vector3Int(x, y);
                Chunk c = new Chunk(x, y, Tiles.GetTile<Tile>(p1));
                c.WorldPos = Tiles.GetCellCenterWorld(p1);
                World[x, y] = c;
            }
        }
    }

    public Chunk GetChunk(Vector3Int pos)
    {
        return World[pos.x, pos.y];
    }

    public void Load(Vector3Int pos)
    {
        int x0 = Mathf.Max(pos.x - LoadRadius, 0);
        int x1 = Mathf.Min(pos.x + LoadRadius, World.GetUpperBound(0));
        int y0 = Mathf.Max(pos.y - LoadRadius, 0);
        int y1 = Mathf.Min(pos.y + LoadRadius, World.GetUpperBound(1));

        for (int y = y0; y < y1; y++)
        {
            for (int x = x0; x < x1; x++)
            {
                World[x, y].LoadChunk();
            }
        }
    }

    public void OnPlayerMove()
    {
        chunkUpdates++;
        Debug.Log("Chunk Update: " + chunkUpdates);
        Load(Player.TilePosition);
    }
}

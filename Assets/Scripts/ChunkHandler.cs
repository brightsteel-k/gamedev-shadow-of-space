using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ChunkHandler : MonoBehaviour
{
    int chunkUpdates = 0;
    public static Tilemap TILES;
    public static Chunk[,] WORLD;
    static int LOAD_RADIUS = 2;

    // Start is called before the first frame update
    void Start()
    {
        TILES = GetComponent<Tilemap>();
        BuildWorld();
        EventManager.OnTilePosChanged += OnPlayerMove;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void BuildWorld()
    {
        int xLen = TILES.size.x;
        int yLen = TILES.size.y;
        Debug.Log(xLen + " " + yLen);
        WORLD = new Chunk[xLen, yLen];
        for (int x = 0; x < xLen; x++)
        {
            for (int y = 0; y < yLen; y++)
            {
                Vector3Int p1 = new Vector3Int(x, y);
                Chunk c = new Chunk(x, y, TILES.GetTile<Tile>(p1));
                c.worldPos = TILES.GetCellCenterWorld(p1);
                c.biome = Biome.VioletWastes;
                WORLD[x, y] = c;
            }
        }
    }

    public Chunk GetChunk(Vector3Int pos)
    {
        return WORLD[pos.x, pos.y];
    }

    public void Load(Vector3Int pos)
    {
        int x0 = Mathf.Max(pos.x - LOAD_RADIUS, 0);
        int x1 = Mathf.Min(pos.x + LOAD_RADIUS, WORLD.GetUpperBound(0));
        int y0 = Mathf.Max(pos.y - LOAD_RADIUS, 0);
        int y1 = Mathf.Min(pos.y + LOAD_RADIUS, WORLD.GetUpperBound(1));

        for (int y = y0; y < y1; y++)
        {
            for (int x = x0; x < x1; x++)
            {
                WORLD[x, y].LoadChunk();
            }
        }
    }

    public void OnPlayerMove()
    {
        chunkUpdates++;
        Debug.Log("Chunk Update: " + chunkUpdates);
        Load(Player.TILE_POSITION);
    }
}

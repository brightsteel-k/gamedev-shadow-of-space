using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ChunkHandler : MonoBehaviour
{
    int chunkUpdates = 0;
    public static Tilemap TILES;
    public static Chunk[,] WORLD;
    static int LOAD_RADIUS = 4;

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
        int[] bounds = new int[] { 25, 25 };
        WORLD = new Chunk[bounds[0], bounds[1]];
        for (int x = 0; x < bounds[0]; x++)
        {
            for (int y = 0; y < bounds[1]; y++)
            {
                Vector3Int p1 = new Vector3Int(x, y);
                Chunk c = new Chunk(x, y);
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

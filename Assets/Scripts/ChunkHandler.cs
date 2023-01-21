using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ChunkHandler : MonoBehaviour
{
    int chunkUpdates = 0;
    public static Tilemap TILES;
    public static Chunk[,] WORLD;
    static int LOAD_RADIUS = 5;

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
        int[] bounds = new int[] { 15, 15 };
        WORLD = new Chunk[bounds[0], bounds[1]];
        for (int x = 0; x < bounds[0]; x++)
        {
            for (int z = 0; z < bounds[1]; z++)
            {
                Chunk c = new Chunk(x, z);
                c.biome = Biome.VioletWastes;
                WORLD[x, z] = c;
            }
        }
    }

    public Chunk GetChunk(Vector3Int pos)
    {
        return WORLD[pos.x, pos.z];
    }

    public void Load(Vector3Int pos)
    {
        int x0 = Mathf.Max(pos.x - LOAD_RADIUS, 0);
        int x1 = Mathf.Min(pos.x + LOAD_RADIUS + 1, WORLD.GetUpperBound(0));
        int z0 = Mathf.Max(pos.z - LOAD_RADIUS, 0);
        int z1 = Mathf.Min(pos.z + LOAD_RADIUS + 1, WORLD.GetUpperBound(1));

        Debug.Log(pos);

        for (int z = z0; z < z1; z++)
        {
            for (int x = x0; x < x1; x++)
            {
                if (x == x0 || x == x1 || z == z0 || z == z1)
                    WORLD[x, z].SetChunkActive(false);
                else
                    WORLD[x, z].LoadChunk();
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

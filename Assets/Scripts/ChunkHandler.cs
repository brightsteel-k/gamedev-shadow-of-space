using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ChunkHandler : MonoBehaviour
{
    int chunkUpdates = 0;
    public static Tilemap TILES;
    public static Chunk[,] WORLD;
    static int LOAD_RADIUS = 3;
    static int X_BOUND = 15;
    static int Z_BOUND = 15;
    static bool CHUNK_HANDLING = true;

    // Start is called before the first frame update
    void Start()
    {
        TILES = GetComponent<Tilemap>();
        BuildWorld();
        EventManager.OnTilePosChanged += OnPlayerMove;
        StartCoroutine("UnloadChunks");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void BuildWorld()
    {
        WORLD = new Chunk[X_BOUND, Z_BOUND];
        for (int x = 0; x < X_BOUND; x++)
        {
            for (int z = 0; z < Z_BOUND; z++)
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
        int x1 = Mathf.Min(pos.x + LOAD_RADIUS, WORLD.GetUpperBound(0));
        int z0 = Mathf.Max(pos.z - LOAD_RADIUS, 0);
        int z1 = Mathf.Min(pos.z + LOAD_RADIUS, WORLD.GetUpperBound(1));

        for (int z = z0; z < z1; z++)
        {
            for (int x = x0; x < x1; x++)
            {
                WORLD[x, z].LoadChunk();
            }
        }
    }

    IEnumerator UnloadChunks()
    {
        while (CHUNK_HANDLING)
        {
            for (int x = 0; x < X_BOUND; x++)
            {
                for (int z = 0; z < Z_BOUND; z++)
                {
                    Chunk c = WORLD[x, z];
                    Vector3Int a = Player.TILE_POSITION;
                    Vector3Int b = new Vector3Int(x, 0, z);

                    if (c.active && Vector3Int.Distance(a, b) > 5)
                        c.SetChunkActive(false);
                }
            }
            yield return new WaitForSeconds(10f);
        }
    }

    public void OnPlayerMove()
    {
        chunkUpdates++;
        Load(Player.TILE_POSITION);
    }
}

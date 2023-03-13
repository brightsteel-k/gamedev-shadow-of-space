using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ChunkHandler : MonoBehaviour
{
    public static Tilemap TILES;
    public static Chunk[,] WORLD;
    static int LOAD_RADIUS = 3;
    static int X_BOUND = 30;
    static int Z_BOUND = 30;
    static bool CHUNK_HANDLING = true;

    // Start is called before the first frame update
    void Start()
    {
        TILES = GetComponent<Tilemap>();

        Environment.InitBiomes();
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
                Chunk c = new Chunk(x, z, Environment.Biomes["violet_wastes"]);
                WORLD[x, z] = c;
            }
        }
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
        Load(Player.TILE_POSITION);
    }

    public static float GetMaxXCoord()
    {
        return X_BOUND * Chunk.WIDTH;
    }

    public static float GetMaxZCoord()
    {
        return Z_BOUND * Chunk.WIDTH;
    }

    public static Vector3 BoundCoordinate(Vector3 pos)
    {
        float x = Mathf.Min(pos.x, GetMaxXCoord() - 5);
        float z = Mathf.Min(pos.z, GetMaxZCoord() - 5);
        
        return new Vector3(Mathf.Max(x, 5f), pos.y, Mathf.Max(z, 5f));
    }

    public static float BoundZCoordinate(float z)
    {
        z = Mathf.Min(z, GetMaxZCoord() - 5);
        z = Mathf.Max(z, 5);
        return z;
    }

    public static Vector3Int WorldPosToTile(Vector3 worldPos)
    {
        return new Vector3Int((int)(worldPos.x / Chunk.WIDTH), 0, (int)(worldPos.z / Chunk.WIDTH));
    }

    public static Chunk GetChunk(Vector3 worldPos)
    {
        return GetChunk(WorldPosToTile(worldPos));
    }

    public static Chunk GetChunk(Vector3Int pos)
    {
        return WORLD[pos.x, pos.z];
    }
}

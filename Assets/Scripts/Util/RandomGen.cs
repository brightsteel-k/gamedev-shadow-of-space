using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomGen
{
    static HashSet<Vector3> currentChunkObjects = new HashSet<Vector3>();

    public static void NewChunk()
    {
        currentChunkObjects.Clear();
    }

    public static Vector3 GetPosInChunk(GenType gen, float x, float z)
    {
        Vector3 relative;

        switch (gen)
        {
            case GenType.NaiveRandom:
                relative = ChunkPos(Random.value * Chunk.WIDTH, Random.value * Chunk.WIDTH, true);
                break;
            case GenType.Dense:
                float radians = Random.Range(0, 2 * Mathf.PI);
                relative = ChunkPos(1.6f * Mathf.Cos(radians), 1.6f * Mathf.Sin(radians), false);
                break;
            default:
                relative = ChunkPos(x, z, true);
                break;
        }

        if (currentChunkObjects.Add(relative))
            return ChunkPos(x, z, false) + relative;
        else
            throw new CancelledChunkPosException();
    }

    public static bool ShouldChunkFeatureGenerate(Vector3Int chunk, int multiplier, int increment, int modulus)
    {
        return (multiplier * chunk.x * chunk.z + increment) % modulus == 0;
    }

    public static Vector3 ChunkPos(float x, float z, bool round)
    {
        if (round)
            return new Vector3(x, 0.0f, z);
        else
            return new Vector3(Mathf.Round(x), 0.0f, Mathf.Round(z));
    }

    public static int GetCountFromAbundance(float abundance, int degree)
    {
        return Mathf.FloorToInt(abundance * Mathf.Pow(Random.Range(1, 101), degree));
    }

    public static int GetCountFiftyPercent(float count)
    {
        return Mathf.Max(0, Random.Range((int)count * -1, (int)count + 1));
    }

    public static int Mercury(float a)
    {
        return (int)(a + (Random.value - 0.5f) * 2);
    }

    public static int MaybeMinusOne(int a)
    {
        return Mathf.Max(0, a - Random.Range(0, 2));
    }

    public static bool ShouldContinueCircling(int circles)
    {
        return Random.Range(0, Mathf.Max(0, 7 - circles)) > 1;
    }

    public static float MercuryDirection(float theta)
    {
        return theta + Random.Range(-0.75f, 0.75f);
    }

    public static float RecuperationTime(int numTimesFled)
    {
        float lowerBound = Mathf.Max(7f, 12f - numTimesFled);
        float upperBound = Mathf.Max(lowerBound, 24f - numTimesFled * 2f);
        return Random.Range(lowerBound, upperBound);
    }

    public static float Range(float a, float b)
    {
        return Random.Range(a, b);
    }

    public static int Range(int a, int b)
    {
        return Random.Range(a, b + 1);
    }

    public static Vector3 DropItemMomentum()    
    {
        float theta = Random.Range(0, 2 * Mathf.PI);
        float magnitude = 1.5f;
        return new Vector3(Mathf.Cos(theta) * magnitude, Random.Range(2.5f, 3.5f), Mathf.Sin(theta) * magnitude);
    }

    public static Vector3 RandomRotation()
    {
        return new Vector3(Random.Range(-16f, 16f), 0, Random.Range(-16f, 16f));
    }
}

public class CancelledChunkPosException : System.SystemException
{

}
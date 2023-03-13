using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomGen
{
    public static Vector3 GetPos(GenType gen, float x, float y)
    {
        Vector3 relative;

        switch (gen)
        {
            case GenType.NaiveRandom:
                relative = new Vector3(Random.value * Chunk.WIDTH, 0.0f, Random.value * Chunk.WIDTH);
                break;
            case GenType.Dense:
                float radians = Random.Range(0, 2 * Mathf.PI);
                relative = new Vector3(1.6f * Mathf.Cos(radians), 0.0f, 1.6f * Mathf.Sin(radians));
                break;
            default:
                relative = new Vector3(x, 0.0f, y);
                break;
        }

        return new Vector3(x, 0.0f, y) + relative;
    }

    public static int GetCountFromRarity(float rarity, int degree)
    {
        return Mathf.FloorToInt(rarity * (Mathf.Pow(Random.Range(1, 101), degree)));
    }

    public static int GetCountFiftyPercent(float count)
    {
        return Mathf.Max(0, Random.Range((int)count * -1, (int)count + 1));
    }

    public static int Mercury(float a)
    {
        return (int)(a + (Random.value - 0.5f) * 2);
    }

    public static bool ShouldContinueCircling(int circles)
    {
        return Random.Range(0, Mathf.Max(0, 6 - circles)) > 1;
    }

    public static float MercuryDirection(float theta)
    {
        return theta + Random.Range(-1f, 1f);
    }

    public static float GetFleeDistance()
    {
        return Random.Range(3, 15) * Chunk.WIDTH;
    }
}

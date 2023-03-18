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

    public static int MaybeMinusOne(int a)
    {
        return Mathf.Max(0, a - Random.Range(0, 2));
    }

    public static bool ShouldContinueCircling(int circles)
    {
        return Random.Range(0, Mathf.Max(0, 6 - circles)) > 1;
    }

    public static float MercuryDirection(float theta)
    {
        return theta + Random.Range(-1f, 1f);
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

    public static Vector3 DropItemMomentum()    
    {
        float theta = Random.Range(0, 2 * Mathf.PI);
        float magnitude = 1.5f;
        return new Vector3(Mathf.Cos(theta) * magnitude, Random.Range(2.5f, 3.5f), Mathf.Sin(theta) * magnitude);
    }
}

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


    public static int Mercury(float a)
    {
        return (int)(a + (Random.value - 0.5f) * 2);
    }
}

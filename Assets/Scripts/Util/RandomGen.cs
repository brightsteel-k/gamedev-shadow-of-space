using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomGen
{
    public static Vector3 GetPos(GenType gen, float x, float y, float scale)
    {
        Vector3 relative;

        switch (gen)
        {
            case GenType.NaiveRandom:
                relative = new Vector3(Random.value * scale, 0.0f, Random.value * scale);
                break;
            default:
                relative = new Vector3(x, 0.0f, y);
                break;
        }

        return new Vector3(x, 0.0f, y) + relative;
    }
}

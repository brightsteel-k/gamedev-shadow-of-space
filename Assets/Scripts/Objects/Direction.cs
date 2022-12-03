using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    North,
    East,
    South,
    West
}

static class DirectionMethods
{
    public static Direction Opposite(this Direction d1)
    {
        switch (d1)
        {
            case Direction.North:
                return Direction.South;
            case Direction.East:
                return Direction.West;
            case Direction.South:
                return Direction.North;
            case Direction.West:
                return Direction.East;
            default:
                return Direction.North;
        }
    }

    public static Vector3Int Vector(this Direction d1)
    {
        switch (d1)
        {
            case Direction.North:
                return new Vector3Int(0, 1);
            case Direction.East:
                return new Vector3Int(1, 0);
            case Direction.South:
                return new Vector3Int(0, -1);
            case Direction.West:
                return new Vector3Int(-1, 0);
            default:
                return new Vector3Int(1, 0);
        }
    }

    public static Vector3Int OppositeVector(this Direction d1)
    {
        return -d1.Vector();
    }
}


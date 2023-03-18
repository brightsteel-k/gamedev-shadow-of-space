using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WorldObject : Rotatable
{
    [SerializeField] protected string id;

    public virtual WorldObject Place()
    {
        return this;
    }

    public virtual void InitSprite()
    {
        textureObject = gameObject;
        InitRotation();
    }

    protected override void Pivot(bool clockwise)
    {
        if (gameObject.activeSelf)
            Player.Pivot(textureObject, clockwise);
    }

    public virtual void Remove()
    {
        RemoveRotation();
        Destroy(gameObject);
    }

    public virtual void SetActive(bool active) //Properly deactivates world object, in case information about it needs to be saved first
    {
        gameObject.SetActive(active);
        if (active)
            Player.PivotInit(textureObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WorldObject : Rotatable
{
    protected List<WorldObject> chunkRegistry;
    [SerializeField] protected string id;

    public virtual WorldObject Place(List<WorldObject> registry)
    {
        registry.Add(this);
        SetChunkRegistry(registry);
        return this;
    }

    public virtual void SetChunkRegistry(List<WorldObject> registry)
    {
        chunkRegistry = registry;
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
        if (!chunkRegistry.Remove(this))
            Debug.LogWarning("Unregistered WorldObject trying to be removed from its ChunkRegistry: " + name);
        Destroy(gameObject);
    }
    
    public virtual string getId()
    {
        return id;
    }

    public virtual void SetActive(bool active) //Properly deactivates world object, in case information about it needs to be saved first
    {
        gameObject.SetActive(active);
        if (active)
            Player.PivotInit(textureObject);
    }
}

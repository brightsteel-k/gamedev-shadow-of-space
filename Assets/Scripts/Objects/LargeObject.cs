using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargeObject : WorldObject
{
    [SerializeField] protected float minSize;
    [SerializeField] protected float maxSize;
    [Tooltip("The constant by which the scale is multiplied to get the appropriate y position of the object.")]
    [SerializeField] protected float scaleYPosBy;

    [Header("Breakability")]
    [SerializeField] private string rockDrop = "";
    [SerializeField] private int rockDropNumMin;
    [SerializeField] private int rockDropNumMax;
    [SerializeField] private string rareDrop = "";
    [SerializeField] private GameObject breakParticles;
    public float breakTime;

    protected float size;

    // Start is called before the first frame update
    protected override void Start()
    {
        
    }

    public override void InitSprite()
    {
        textureObject = gameObject;
        InitRotation();
        size = Random.Range(minSize, maxSize);
        sprite.transform.localScale = new Vector3(size, size, size);
        sprite.transform.localPosition = new Vector3(sprite.transform.localPosition.x, size * scaleYPosBy, size * scaleYPosBy);
    }

    public virtual void BreakObject()
    {
        Vector3 dropPos;
        int count = RandomGen.Range(rockDropNumMin, rockDropNumMax);
        for (int k = 0; k < count; k++) {
            dropPos = transform.position + new Vector3(0, RandomGen.Range(0.2f, 0.7f), 0);
            Environment.DropItem(rockDrop, dropPos);
        }

        if (rareDrop != "" && RandomGen.GetCountFromAbundance(0.000002f, 3) > 0)
            Environment.DropItem(rareDrop, transform.position + Vector3.up);
        Remove();
    }

    public GameObject GetBreakParticles()
    {
        return breakParticles;
    }
}

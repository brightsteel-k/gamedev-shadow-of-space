using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargeObject : WorldObject
{
    [SerializeField] protected float minSize;
    [SerializeField] protected float maxSize;
    [Tooltip("The constant by which the scale is multiplied to get the appropriate y position of the object.")]
    [SerializeField] protected float scaleYPosBy;


    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        float size = Random.Range(minSize, maxSize);
        transform.localScale = new Vector3(size, size, size);
        transform.position = new Vector3(transform.position.x, size * scaleYPosBy, transform.position.z);
    }
}

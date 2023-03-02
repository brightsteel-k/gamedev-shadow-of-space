using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargeObject : WorldObject
{
    [SerializeField] protected GameObject sprite;

    [SerializeField] protected float minSize;
    [SerializeField] protected float maxSize;
    [Tooltip("The constant by which the scale is multiplied to get the appropriate y position of the object.")]
    [SerializeField] protected float scaleYPosBy;

    protected float size;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        size = Random.Range(minSize, maxSize);
        sprite.transform.localScale = new Vector3(size, size, size);
        sprite.transform.localPosition = new Vector3(0, size * scaleYPosBy, 0);
    }

    protected override void InitSprite(int index)
    {
        sprite.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Textures/Features/" + id + "_" + index);
    }
}

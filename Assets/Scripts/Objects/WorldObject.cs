using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldObject : Rotatable
{
    [SerializeField] protected string id;
    [Tooltip("Length equal to the number of textures for this asset.\n" +
             "Found in Resources/Textures/Features")]
    [SerializeField] protected float[] baseHeights;
    protected int spriteId;

    protected override void Start()
    {
        
    }

    public virtual WorldObject Place()
    {
        transform.position = new Vector3(transform.position.x, baseHeights[spriteId], transform.position.z);
        return this;
    }

    public virtual void InitSprite()
    {
        textureObject = gameObject;
        InitRotation();
        spriteId = baseHeights.Length - 1;
        if (spriteId > 0)
        {
            spriteId = Random.Range(0, spriteId + 1);
            SetSprite();
        }
    }

    protected virtual void SetSprite()
    {
        GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Textures/Features/" + id + "_" + spriteId);
    }

    protected override void Pivot(bool clockwise)
    {
        if (gameObject.activeSelf)
            Player.Pivot(textureObject, clockwise);
    }

    public void SetActive(bool active) //Properly deactivates world object, in case information about it needs to be saved first
    {
        gameObject.SetActive(active);
        if (active)
            Player.PivotInit(textureObject);
    }
}

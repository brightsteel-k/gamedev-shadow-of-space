using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizableObject : WorldObject
{

    [Tooltip("Length equal to the number of textures for this asset.\n" +
             "Found in Resources/Textures/Features")]
    [SerializeField] protected float[] baseHeights;
    protected int spriteId;

    public override WorldObject Place(List<WorldObject> registry)
    {
        transform.position = new Vector3(transform.position.x, baseHeights[spriteId], transform.position.z);
        return base.Place(registry);
    }

    public override void InitSprite()
    {
        base.InitSprite();
        spriteId = baseHeights.Length - 1;
        if (spriteId > 0)
        {
            spriteId = Random.Range(0, spriteId + 1);
            GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Textures/Features/" + id + "_" + spriteId);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

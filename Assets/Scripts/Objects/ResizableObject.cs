using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizableObject : WorldObject
{
    [Tooltip("Length equal to the number of textures for this asset.\n" +
             "Found in Resources/Textures/Features")]
    [SerializeField] protected Vector3[] texturePositions;
    protected int spriteId;

    public override WorldObject Place(List<WorldObject> registry)
    {
        sprite.transform.localPosition = texturePositions[spriteId];
        return base.Place(registry);
    }

    public override void InitSprite()
    {
        base.InitSprite();
        spriteId = texturePositions.Length - 1;
        if (spriteId > 0)
        {
            spriteId = Random.Range(0, spriteId + 1);
            sprite.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Textures/Features/" + id + "_" + spriteId);
        }
    }

    public override void Harvest()
    {
        tag = "Untagged";
        // @TODO: Particles
        Remove();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, "Rigidbody Gizmo.tiff", false);
    }
}

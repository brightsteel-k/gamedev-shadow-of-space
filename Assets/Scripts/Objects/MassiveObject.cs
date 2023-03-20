using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MassiveObject : LargeObject
{
    protected int initDirection = 1;
    protected int directionOffset = 0;
    protected Sprite[] textures;
    protected SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    protected override void Start()
    {
        
    }

    public override void InitSprite()
    {
        base.InitSprite();
        textures = ItemTextures.GetDirectionalTextures(id, out initDirection);
        spriteRenderer = sprite.GetComponent<SpriteRenderer>();
        RotateTexture(0);
    }

    public override void BreakObject()
    {
        base.BreakObject();
    }

    protected override void Pivot(bool clockwise)
    {
        base.Pivot(clockwise);
        RotateTexture(clockwise ? -1 : 1);
    }

    protected virtual void RotateTexture(int change)
    {
        int camRotation = Player.CAMERA_ROTATION + change;

        if ((initDirection + camRotation) % 8 == 0)
        {
            LeanTween.delayedCall(0.05f, e => spriteRenderer.sprite = textures[0]);
        }
        else if ((initDirection + camRotation) % 8 < 4 && (initDirection + camRotation) % 8 > 0)
        {
            LeanTween.delayedCall(0.05f, e => spriteRenderer.sprite = textures[1]);
        }
        else if ((initDirection + camRotation) == 4)
        {
            LeanTween.delayedCall(0.05f, e => spriteRenderer.sprite = textures[2]);
        }
        else
        {
            LeanTween.delayedCall(0.05f, e => spriteRenderer.sprite = textures[3]);
        }
        // Debug.Log("CAMERA_ROTATION: " + camRotation + ", Direction Offset: " + directionOffset + ", New Direction: " + newDirection);
    }
}

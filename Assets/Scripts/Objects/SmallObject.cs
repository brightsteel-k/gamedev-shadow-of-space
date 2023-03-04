using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallObject : WorldObject
{
    [SerializeField] protected GameObject sprite;
    private bool isGrounded = false;
    private Rigidbody rb;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (!isGrounded)
        {
            if (rb.isKinematic)
                rb.isKinematic = false;
            if (transform.position.y <= baseHeights[spriteId])
            {
                rb.isKinematic = true;
                isGrounded = true;
                transform.position = new Vector3(transform.position.x, baseHeights[spriteId], transform.position.z);
            }
        }
    }

    public override void InitSprite()
    {
        textureObject = sprite;
        InitRotation();
        spriteId = baseHeights.Length - 1;
        if (spriteId > 0)
        {
            spriteId = Random.Range(0, spriteId + 1);
            SetSprite();
        }
    }

    protected override void SetSprite()
    {
        sprite.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Textures/Features/" + id + "_" + spriteId);
    }
}

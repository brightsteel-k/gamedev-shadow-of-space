using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallObject : WorldObject
{
    [SerializeField] protected GameObject sprite;
    private bool isGrounded = false;
    private Rigidbody rb;
    float physHeight;

    // Start is called before the first frame update
    protected override void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (!isGrounded)
        {
            if (rb.isKinematic)
                rb.isKinematic = false;
            if (transform.position.y <= physHeight)
            {
                rb.isKinematic = true;
                isGrounded = true;
                transform.position = new Vector3(transform.position.x, physHeight, transform.position.z);
            }
        }
    }
    public void InitItem(string itemId)
    {
        id = itemId;
        InitSprite();
    }

    public override void InitSprite()
    {
        textureObject = sprite;
        InitRotation();

        Sprite img = ItemTextures.GetItemTexture(id);
        physHeight = img.texture.height / img.pixelsPerUnit / 10 / Mathf.Sqrt(2);
        sprite.GetComponent<SpriteRenderer>().sprite = img;
    }

    public override WorldObject Place()
    {
        transform.position = new Vector3(transform.position.x, physHeight, transform.position.z);
        return this;
    }
}

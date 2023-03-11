using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : WorldObject
{
    [SerializeField] protected GameObject sprite;
    protected float size = 0.25f;
    private bool isGrounded = false;
    private Rigidbody rb;
    float physHeight;

    // Start is called before the first frame update
    protected override void Start()
    {
        
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

    public void InitItem(string itemId, float itemSize)
    {
        rb = GetComponent<Rigidbody>();
        id = itemId;
        size = itemSize;
        InitSprite();
        sprite.transform.localScale = new Vector3(size, size, size);

        Sprite img = ItemTextures.GetItemTexture(id);
        physHeight = img.texture.height / img.pixelsPerUnit / 12 / Mathf.Sqrt(2);
        sprite.GetComponent<SpriteRenderer>().sprite = img;
    }

    public override WorldObject Place()
    {
        transform.position = new Vector3(transform.position.x, physHeight, transform.position.z);
        isGrounded = true;
        rb.isKinematic = true;
        return this;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : WorldObject
{
    protected float size = 0.25f;
    private bool isGrounded = false;
    private Rigidbody rb;
    private SphereCollider sphereCollider;
    float physHeight;
    Item item;

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

    public void InitItem(Item itemIn, float itemSize)
    {
        textureObject = gameObject;
        rb = GetComponent<Rigidbody>();
        sphereCollider = GetComponent<SphereCollider>();
        id = itemIn.id;
        item = itemIn;
        size = itemSize;
        InitSprite();
        sprite.transform.localScale = new Vector3(size, size, size);

        Sprite img = ItemTextures.GetItemTexture(id);
        physHeight = img.texture.height / img.pixelsPerUnit / 12 / Mathf.Sqrt(2);
        sprite.GetComponent<SpriteRenderer>().sprite = img;
    }

    public void Pickup(Vector3 playerPos)
    {
        sphereCollider.enabled = false;
        tag = "Untagged";
        transform.LeanMove(playerPos, 0.1f).setOnComplete(Remove);
    }

    public Item GetItem()
    {
        return item;
    }

    public override WorldObject Place(List<WorldObject> registry)
    {
        transform.position = new Vector3(transform.position.x, physHeight, transform.position.z);
        isGrounded = true;
        rb.isKinematic = true;
        return base.Place(registry);
    }
}

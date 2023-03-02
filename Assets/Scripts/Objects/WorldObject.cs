using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldObject : Rotatable
{
    [SerializeField] protected string id;
    [Tooltip("Length equal to the number of textures for this asset.\n" +
             "Found in Resources/Textures/Features")]
    [SerializeField] float[] baseHeights;

    protected override void Start()
    {
        base.Start();
        int i = baseHeights.Length - 1;
        if (i > 0)
        {
            i = Random.Range(0, i + 1);
            InitSprite(i);
        }

        transform.position = new Vector3(transform.position.x, baseHeights[i], transform.position.z);
    }

    protected virtual void InitSprite(int index)
    {
        GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Textures/Features/" + id + "_" + index);
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

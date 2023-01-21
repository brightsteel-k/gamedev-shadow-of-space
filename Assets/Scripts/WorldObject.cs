using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldObject : MonoBehaviour
{
    [SerializeField] string id;
    [Tooltip("The number of textures for this asset.\n" +
             "Found in Resources/Textures/Features")]
    [SerializeField] int textures;
    [SerializeField] float[] baseHeights;

    private void Start()
    {
        EventManager.OnWorldPivot += Pivot;
        int i = textures - 1;
        if (i > 0)
        {
            i = Random.Range(0, textures);
            GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Textures/Features/" + id + "_" + i);
        }

        transform.position = new Vector3(transform.position.x, baseHeights[i], transform.position.z);
        Player.PivotInit(gameObject);
    }

    private void OnDestroy()
    {
        EventManager.OnWorldPivot -= Pivot;
    }

    private void Pivot() => Player.Pivot(gameObject);

    public void SetActive(bool active) //Properly deactivates world object, in case information about it needs to be saved first
    {
        Debug.Log(name + " should be disappearing");
        gameObject.SetActive(active);
    }
}

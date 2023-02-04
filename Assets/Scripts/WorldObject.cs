using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldObject : MonoBehaviour
{
    [SerializeField] string id;
    [Tooltip("Length equal to the number of textures for this asset.\n" +
             "Found in Resources/Textures/Features")]
    [SerializeField] float[] baseHeights;

    public virtual void Start()
    {
        EventManager.OnWorldPivot += Pivot;
        int i = baseHeights.Length - 1;
        if (i > 0)
        {
            i = Random.Range(0, i + 1);
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
        gameObject.SetActive(active);
    }
}

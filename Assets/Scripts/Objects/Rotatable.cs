using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotatable : MonoBehaviour
{
    protected GameObject textureObject;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        textureObject = gameObject;
    }

    protected virtual void InitRotation()
    {
        EventManager.OnWorldPivot += Pivot;
        VerifyRotation();
    }

    protected virtual void VerifyRotation()
    {
        Vector3 rotation = textureObject.transform.eulerAngles;
        textureObject.transform.eulerAngles = new Vector3(rotation.x, Player.CurrentYRotation(), rotation.z);
    }

    protected virtual void RemoveRotation()
    {
        EventManager.OnWorldPivot -= Pivot;
    }

    private void OnDestroy()
    {
        RemoveRotation();
    }

    protected virtual void Pivot(bool clockwise) {
        if (gameObject.activeSelf)
        {
            VerifyRotation();
            Player.Pivot(textureObject, clockwise);
        }
    }
}

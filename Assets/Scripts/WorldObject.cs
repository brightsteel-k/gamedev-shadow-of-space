using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldObject : MonoBehaviour
{
    private void Start()
    {
        EventManager.OnWorldPivot += Pivot;
    }

    private void OnDestroy()
    {
        EventManager.OnWorldPivot -= Pivot;
    }

    private void Pivot() //Rotates this object a certain amount so it continues to face the camera
    {
        Quaternion currentRotation = transform.rotation;
        float rotationChange = Mathf.Rad2Deg* 2 * Mathf.PI * Player.WorldPlayer.rotationChangeQuotient;
        Quaternion modifiedRotation = Quaternion.Euler(currentRotation.eulerAngles.x, currentRotation.eulerAngles.y - rotationChange, currentRotation.eulerAngles.z);
        LeanTween.rotate(this.gameObject, modifiedRotation.eulerAngles, Player.WorldPlayer.rotationTime).setEase(LeanTweenType.easeOutQuint);
    }

    public void SetActive(bool active) //Properly deactivates world object, in case information about it needs to be saved first
    {
        gameObject.SetActive(active);
    }
}

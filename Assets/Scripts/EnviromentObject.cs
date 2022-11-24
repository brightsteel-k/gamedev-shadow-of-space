using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviromentObject : MonoBehaviour
{
    private Player player;

    private void Start()
    {
        EventManager.OnWorldPivot += Pivot;
        player = FindObjectOfType<Player>();
    }

    private void OnDestroy()
    {
        EventManager.OnWorldPivot -= Pivot;
    }

    private void Update()
    {
        //RotateTowardsCamera();
        //DetermineRenderOrder();
    }

    private void DetermineRenderOrder()
    {
        if (transform.position.x > player.transform.position.x)
        {
            GetComponent<SpriteRenderer>().rendererPriority = 1;
        } else
        {
            GetComponent<SpriteRenderer>().rendererPriority = 3;
        }
    }

    private void RotateTowardsCamera() //Rotates this object towards the camera so that it appears the same irrespective of distance/orientation
    {
        Quaternion currentRotation = transform.rotation;
        Quaternion newRotation = Quaternion.FromToRotation(transform.position, player.mainCamera.transform.position);
        Quaternion modifiedRotation = Quaternion.Euler(newRotation.eulerAngles.x, currentRotation.eulerAngles.y, currentRotation.eulerAngles.z);
        transform.rotation = modifiedRotation;
    }

    private void Pivot() //Rotates this object a certain amount so it continues to face the camera
    {
        Quaternion currentRotation = transform.rotation;
        float rotationChange = Mathf.Rad2Deg* 2 * Mathf.PI * player.rotationChangeQuotient;
        Quaternion modifiedRotation = Quaternion.Euler(currentRotation.eulerAngles.x, currentRotation.eulerAngles.y - rotationChange, currentRotation.eulerAngles.z);
        LeanTween.rotate(this.gameObject, modifiedRotation.eulerAngles, player.rotationTime).setEase(LeanTweenType.easeOutQuint);
    }
}

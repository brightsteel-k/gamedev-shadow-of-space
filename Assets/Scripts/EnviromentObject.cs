using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviromentObject : MonoBehaviour
{
    private Player player;

    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    void Update()
    {
        RotateTowardsCamera();
    }

    private void RotateTowardsCamera() //Rotates this object towards the camera so that it appears the same irrespective of distance/orientation
    {

        Vector3 positionDifference = player.camera.transform.position - transform.position; // Gets the vector of the object to the camera
        float rotationFloat = positionDifference.normalized.z; //Gets the z component of the unit vector 

        float rotationAngle = Mathf.Rad2Deg *  ((float)Math.Acos(rotationFloat)); //Converts the z component to the angle from the z-axis

        Vector3 currentRotation = transform.eulerAngles;
        transform.eulerAngles = new Vector3(-rotationAngle, currentRotation.y, currentRotation.z); //Changes the x rotation component to the evaluated angle

    }
}

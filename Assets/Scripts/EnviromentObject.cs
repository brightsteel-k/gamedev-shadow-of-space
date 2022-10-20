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

    private void RotateTowardsCamera() //IN PROGRESS :(
    {
        Vector3 positionDifference = player.camera.transform.position - transform.position;
        Vector3 forward = transform.forward;
        Vector3 newDirection = Vector3.Cross(positionDifference, forward); //The vector math is not even close to being right btw

        transform.rotation = Quaternion.Euler(newDirection); //Supposed to make the object look at the camera
    }
}

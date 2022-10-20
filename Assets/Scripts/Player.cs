using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] private float speed;
    public GameObject camera;

    //Runs shortly after the beginning of runtime
    private void Start()
    {
        camera = transform.GetComponentInChildren<Camera>().gameObject; //Gets the camera object
    }

    //Runs every frame
    private void Update()
    {
        Move();
    }

    private void Move()
    {
        Vector3 positionDifference = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        transform.position = transform.position += positionDifference * (speed * Time.deltaTime); //Moves the player
    }
}

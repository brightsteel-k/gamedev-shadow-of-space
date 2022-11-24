using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] private float speed;
    public GameObject mainCamera;
    [SerializeField] private float rotation = 3 * Mathf.PI / 2;
    [SerializeField] private float cameraOffsetRadius;
    public float rotationChangeQuotient = 1 / 8;
    [SerializeField] private KeyCode RotateKey;
    [SerializeField] private EventManager eventManager;


    private void Start()
    {

        EventManager.OnWorldPivot += PivotCamera;
    }

    //Runs every frame
    private void Update()
    {
        Move();
        if (Input.GetKeyDown(RotateKey))
        {
            Debug.Log("Should do a thing!");
            eventManager.WorldPivot();
        }
    }

    private void Move()
    {
        Vector3 positionDifference = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        transform.position = transform.position += positionDifference * (speed * Time.deltaTime); //Moves the player
    }

    private void PivotCamera()
    {
        float rotationChange = 2 * Mathf.PI * rotationChangeQuotient;
        rotation += rotationChange;

        if (rotation > 2 * Mathf.PI)
        {
            rotation -= 2 * Mathf.PI;
        }

        Vector2 position = new Vector2(Mathf.Cos(rotation), Mathf.Sin(rotation)) * cameraOffsetRadius;

        mainCamera.transform.localPosition = new Vector3(position.x, 6, position.y);
        Vector3 currentRotation = mainCamera.transform.localEulerAngles;
        mainCamera.transform.rotation = Quaternion.Euler(currentRotation.x, currentRotation.y - (rotationChange * Mathf.Rad2Deg), currentRotation.z);
    }

}

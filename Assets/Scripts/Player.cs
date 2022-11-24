using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player WorldPlayer;
    [SerializeField] private float speed;
    public GameObject mainCamera;
    [SerializeField] private float rotation = 3 * Mathf.PI / 2;
    [SerializeField] private float cameraOffsetRadius;
    public float rotationChangeQuotient = 1 / 8;
    [SerializeField] private KeyCode RotateKey;
    public float rotationTime = 0.33f;
    [SerializeField] private EventManager eventManager;
    [SerializeField] private bool canMove = true;


    private void Start()
    {
        WorldPlayer = GetComponent<Player>();
        EventManager.OnWorldPivot += PivotCamera;
    }

    //Runs every frame
    private void Update()
    {
        Move();

        if (Input.GetKeyDown(RotateKey) && canMove)
        {
            eventManager.WorldPivot();
        }
    }

    private void ToggleMove() => canMove = !canMove;

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
        Vector3 newLocalPosition = new Vector3(position.x, 6, position.y);
        Vector3 currentRotation = mainCamera.transform.localEulerAngles;

        LeanTween.moveLocal(mainCamera, newLocalPosition, rotationTime).setEase(LeanTweenType.easeOutQuint);
        LeanTween.rotate(mainCamera, Quaternion.Euler(currentRotation.x, currentRotation.y - (rotationChange * Mathf.Rad2Deg), currentRotation.z).eulerAngles, rotationTime)
            .setEase(LeanTweenType.easeOutQuint)
            .setOnStart(ToggleMove)
            .setOnComplete(ToggleMove);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{
    public static Player WorldPlayer;
    [SerializeField] private EventManager eventManager;
    [SerializeField] private CharacterController characterController;
    public GameObject mainCamera;

    [SerializeField] private float speed;
    [SerializeField] private float rotation = 3 * Mathf.PI / 2;
    [SerializeField] private float cameraOffsetRadius;
    public float rotationChangeQuotient = 1 / 8;
    [SerializeField] private KeyCode RotateKey;
    public float rotationTime = 0.33f;
    [SerializeField] private bool canRotate = true;


    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        WorldPlayer = GetComponent<Player>();
        EventManager.OnWorldPivot += PivotCamera;
    }

    private void Update()
    {
        if (Input.GetKeyDown(RotateKey) && canRotate)
        {
            eventManager.WorldPivot();
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void ToggleMove() => canRotate = !canRotate;

    private void Move()
    {
        Vector3 movementInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        characterController.Move(speed * Time.fixedDeltaTime * movementInput);
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

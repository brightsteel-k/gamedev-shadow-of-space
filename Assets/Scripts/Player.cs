using System;
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
    [SerializeField] private float rotation = Mathf.PI / 2;
    private const float initialRotation = 3 * Mathf.PI / 2;
    [SerializeField] private float cameraOffsetRadius;
    public float rotationChangeQuotient = 1 / 8;
    [SerializeField] private KeyCode RotateKey;
    public float rotationTime = 0.33f;
    public static bool canRotate = true;
    public static LeanTweenType easeType = LeanTweenType.easeOutQuint;


    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        WorldPlayer = GetComponent<Player>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(RotateKey) && canRotate)
        {
            PivotWorld();
        }
    }

    private void PivotWorld()
    {
        eventManager.WorldPivot();

        Pivot(this.gameObject);

        float rotationChange = 2 * Mathf.PI * rotationChangeQuotient;
        rotation += rotationChange;

        if (rotation > 2 * Mathf.PI)
        {
            rotation -= 2 * Mathf.PI;
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    public static void ToggleMove() => Player.canRotate = !Player.canRotate;

    private void Move()
    {
        Vector3 movementInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Quaternion forwardDirection = Quaternion.FromToRotation(Vector3.forward, transform.forward);
        Vector3 modifiedMovement = forwardDirection * movementInput;
        characterController.Move(speed * Time.fixedDeltaTime * modifiedMovement);
    }

    public static void Pivot(GameObject obj)
    {
        Quaternion currentRotation = obj.transform.rotation;
        float rotationChange = Mathf.Rad2Deg * 2 * Mathf.PI * Player.WorldPlayer.rotationChangeQuotient;
        Quaternion modifiedRotation = Quaternion.Euler(currentRotation.eulerAngles.x, currentRotation.eulerAngles.y - rotationChange, currentRotation.eulerAngles.z);
        LeanTween.rotate(obj, modifiedRotation.eulerAngles, Player.WorldPlayer.rotationTime).setEase(Player.easeType)
            .setEase(easeType)
            .setOnStart(ToggleMove)
            .setOnComplete(ToggleMove);
    }

}

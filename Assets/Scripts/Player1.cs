using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{
    public static Player WORLD_PLAYER;
    public static Vector3Int TILE_POSITION = Vector3Int.zero;
    [SerializeField] private CharacterController characterController;
    public GameObject mainCamera;

    [SerializeField] private float speed;
    [SerializeField] private float rotation = Mathf.PI / 2;
    public float rotationChangeQuotient = 1 / 8;
    [SerializeField] private KeyCode rotateKey;
    public float rotationTime = 0.33f;
    public static bool canRotate = true;
    public static LeanTweenType easeType = LeanTweenType.easeOutQuint;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        WORLD_PLAYER = GetComponent<Player>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(rotateKey) && canRotate)
        {
            PivotWorld();
        }

        CheckCurrentTile();
    }

    private void PivotWorld()
    {
        EventManager.WorldPivot();

        Pivot(gameObject);

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

    public static void ToggleMove() => canRotate = !canRotate;

    private void Move()
    {
        Vector3 forwardDirection = transform.forward;
        Vector3 sideDirection = Vector3.Cross(forwardDirection, Vector3.up);

        Vector3 movementInput = new Vector3(-Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        float movementIntoX = Vector3.Dot(movementInput, sideDirection);
        float movementIntoZ = Vector3.Dot(movementInput, forwardDirection);

        Vector3 modifiedMovement = new Vector3(movementIntoX, 0, movementIntoZ);
        
        characterController.Move(speed * Time.fixedDeltaTime * modifiedMovement);
    }

    public static void PivotInit(GameObject obj)
    {
        Vector3 currentRotation = obj.transform.eulerAngles;
        obj.transform.eulerAngles = new Vector3(currentRotation.x, WORLD_PLAYER.transform.eulerAngles.y, currentRotation.z);
    }

    public static void Pivot(GameObject obj)
    {
        Quaternion currentRotation = obj.transform.rotation;
        float rotationChange = Mathf.Rad2Deg * 2 * Mathf.PI * WORLD_PLAYER.rotationChangeQuotient;
        Quaternion modifiedRotation = Quaternion.Euler(currentRotation.eulerAngles.x, currentRotation.eulerAngles.y - rotationChange, currentRotation.eulerAngles.z);
        LeanTween.rotate(obj, modifiedRotation.eulerAngles, WORLD_PLAYER.rotationTime).setEase(easeType)
            .setOnStart(ToggleMove)
            .setOnComplete(ToggleMove);
    }

    public void CheckCurrentTile()
    {
        Vector3Int currentPos = WorldPosToTile(transform.position);
        if (!currentPos.Equals(TILE_POSITION))
        {
            TILE_POSITION = currentPos;
            EventManager.TilePosChanged();
        }
    }

    Vector3Int WorldPosToTile(Vector3 v)
    {
        return new Vector3Int((int)(v.x / 20f), (int)(v.y / 20f), (int)(v.z / 20f));
    }
}

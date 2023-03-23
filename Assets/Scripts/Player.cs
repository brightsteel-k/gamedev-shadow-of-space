using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{
    public static Player WORLD_PLAYER;
    public static Inventory INVENTORY;
    public static Energy ENERGY;
    public static Vector3Int TILE_POSITION = Vector3Int.zero;
    public static CharacterController CONTROLLER;
    public static int CAMERA_ROTATION = 2;
    public static GameObject MAIN_CAMERA;
    public static bool IN_MENU = false;
    private PlayerAnimation animations;
    private ItemOperator itemOperator;

    [SerializeField] private float speed;
    private static float pivotCooldown = 1f;
    [SerializeField] private float cameraOffsetRadius;
    private float rotationChangeQuotient = 1f / 8f;
    public float rotationTime = 0.33f;
    public static bool canRotate = true;
    public static LeanTweenType easeType = LeanTweenType.easeOutQuint;

    [SerializeField] private bool walkType;

    private int mouseThreshold;
    private int mouseCentrePos;
    private int mouseInput = 0;

    // Inventory
    
    private void Awake()
    {
        WORLD_PLAYER = GetComponent<Player>();
    }

    private void Start()
    {
        CONTROLLER = GetComponent<CharacterController>();
        INVENTORY = GetComponent<Inventory>();
        ENERGY = GetComponent<Energy>();
        animations = GetComponent<PlayerAnimation>();
        MAIN_CAMERA = transform.Find("Main Camera").gameObject;
        itemOperator = GetComponent<ItemOperator>();
        mouseThreshold = Screen.width / 3;
        mouseCentrePos = Screen.width / 2;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (pivotCooldown != 0)
            TickPivotCooldown();

        if (!IN_MENU)
        {
            PlayerRotateInput();
            if (Input.GetMouseButtonDown(1))
                itemOperator.UseSelectedItem();
            if (Input.GetKeyDown(KeyCode.Space))
                TryPickupItem();
            if (Input.GetKeyDown(KeyCode.Q))
                itemOperator.TryDropSelectedItem();
        }

        CheckCurrentTile();


        // @TODO: Debug method to check chunks
        if (Input.GetKeyDown(KeyCode.I))
            PrintChunk();

    }

    // @TODO: Debug method to check chunks
    private void PrintChunk()
    {
        ChunkHandler.GetChunk(transform.position).PrintFeatures();
    }

    private void TryPickupItem()
    {
        Collider[] results = new Collider[5];
        Collider feature = null;
        int num = Physics.OverlapSphereNonAlloc(transform.position, 1.5f, results);
        for (int i = 0; i < num; i++)
        {
            if (results[i].tag == "Item" || results[i].tag == "Harvestable")
            {
                feature = results[i];
                break;
            }
        }
        if (feature == null)
            return;


        if (feature.tag == "Item")
        {
            ItemObject item = feature.GetComponent<ItemObject>();
            if (INVENTORY.addItem(item.GetItem()))
                item.Pickup(transform.position + new Vector3(0, 1f, 0));
        }
        else
        {
            WorldObject worldObject = feature.GetComponent<WorldObject>();
            if (INVENTORY.addItem(worldObject.GetID()))
                worldObject.Harvest();
        }
    }

    public void UpdateSelectedItem()
    {
        itemOperator.selectedItem = INVENTORY.getSelectedItem();
    }

    private void PlayerRotateInput()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = false;
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            Cursor.lockState = CursorLockMode.Locked;
            mouseInput = 0;
        }
        else if (mouseCentrePos - Input.mousePosition.x > mouseThreshold)
        {
            if (mouseInput != 1 && canRotate && pivotCooldown == 0)
            {
                PivotWorld(false);
                mouseInput = 1;
            }
        }
        else if (mouseCentrePos - Input.mousePosition.x < -mouseThreshold)
        {
            if (mouseInput != -1 && canRotate && pivotCooldown == 0)
            {
                PivotWorld(true);
                mouseInput = -1;
            }
        }
        else if (mouseInput != 0)
        {
            mouseInput = 0;
        }
    }

    private void TickPivotCooldown()
    {
        if (pivotCooldown < 0)
        {
            pivotCooldown = 0;
            return;
        }

        pivotCooldown -= Time.deltaTime;
    }

    private void PivotWorld(bool clockwise)
    {
        pivotCooldown = 0.5f;
        
        EventManager.WorldPivot(clockwise);
        Pivot(gameObject, clockwise);
        animations.RotateCamera(clockwise);

        if (clockwise)
            CAMERA_ROTATION = CAMERA_ROTATION == 0 ? 7 : CAMERA_ROTATION - 1;
        else
            CAMERA_ROTATION = CAMERA_ROTATION == 7 ? 0 : CAMERA_ROTATION + 1;
    }

    public Vector3 GetDirection()
    {
        int dir = (animations.direction * 2 - CAMERA_ROTATION + 8) % 8;
        Vector3 finalDirection = Vector3.zero;
        if (0 <= dir && dir <= 1 || dir == 7)
            finalDirection = finalDirection + Vector3.right;
        if (1 <= dir && dir <= 3)
            finalDirection = finalDirection + Vector3.back;
        if (3 <= dir && dir <= 5)
            finalDirection = finalDirection + Vector3.left;
        if (5 <= dir && dir <= 7)
            finalDirection = finalDirection + Vector3.forward;
        
        return finalDirection.normalized;
    }

    private void FixedUpdate()
    {
        if (!IN_MENU)
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
        
        CONTROLLER.Move(speed * Time.fixedDeltaTime * modifiedMovement);
    }

    public void SetInMenu(bool inMenu)
    {
        IN_MENU = inMenu;
        animations.SetInMenu(inMenu);
    }

    public static void PivotInit(GameObject obj)
    {
        Vector3 currentRotation = obj.transform.eulerAngles;
        obj.transform.eulerAngles = new Vector3(currentRotation.x, WORLD_PLAYER.transform.eulerAngles.y, currentRotation.z);
    }

    public static void Pivot(GameObject obj, bool clockwise)
    {
        Quaternion currentRotation = obj.transform.rotation;
        float rotationChange = Mathf.Rad2Deg * 2 * Mathf.PI * (clockwise ? -WORLD_PLAYER.rotationChangeQuotient : WORLD_PLAYER.rotationChangeQuotient);
        Quaternion modifiedRotation = Quaternion.Euler(currentRotation.eulerAngles.x, currentRotation.eulerAngles.y - rotationChange, currentRotation.eulerAngles.z);
        LeanTween.rotate(obj, modifiedRotation.eulerAngles, WORLD_PLAYER.rotationTime).setEase(easeType)
            .setEase(easeType)
            .setOnStart(ToggleMove)
            .setOnComplete(ToggleMove);
    }

    public void CheckCurrentTile()
    {
        Vector3Int currentPos = ChunkHandler.WorldPosToTile(transform.position);
        if (!currentPos.Equals(TILE_POSITION))
        {
            TILE_POSITION = currentPos;
            EventManager.TilePosChanged();
        }
    }
}

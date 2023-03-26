using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    public static Player WORLD_PLAYER;
    public static Inventory INVENTORY;
    public static Health HEALTH;
    public static Energy ENERGY;
    public static Vector3Int TILE_POSITION = Vector3Int.zero;
    public static CharacterController CONTROLLER;
    public static int CAMERA_ROTATION = 2;
    public static Camera MAIN_CAMERA;
    private static AudioSource audioSource;
    public static bool IN_MENU = false;
    private PlayerAnimation animations;
    private ItemOperator itemOperator;

    [SerializeField] private GameObject rotateBar;
    private Transform rotateBarPointer;
    private TextMeshProUGUI rotateBarText;
    [SerializeField] private float speed;
    private static float pivotCooldown = 1f;
    [SerializeField] private float cameraOffsetRadius;
    private float rotationChangeQuotient = 1f / 8f;
    public float rotationTime = 0.33f;
    public static bool canRotate = true;
    public static LeanTweenType easeType = LeanTweenType.easeOutQuint;

    private int mouseThreshold;
    private int mouseHalfScreen;

    public bool hasEncounteredMonster = false;

    private void Awake()
    {
        WORLD_PLAYER = GetComponent<Player>();
    }

    private void Start()
    {
        CONTROLLER = GetComponent<CharacterController>();
        INVENTORY = GetComponent<Inventory>();
        ENERGY = GetComponent<Energy>();
        HEALTH = GetComponent<Health>();
        animations = GetComponent<PlayerAnimation>();
        MAIN_CAMERA = transform.Find("Main Camera").GetComponent<Camera>();
        audioSource = GetComponent<AudioSource>();
        itemOperator = GetComponent<ItemOperator>();
        rotateBarPointer = rotateBar.transform.Find("Pointer");
        rotateBarText = rotateBar.transform.Find("Direction").GetComponent<TextMeshProUGUI>();
        InitRotateSystem();
        EventManager.OnPlayerDying += Die;
    }

    private void InitRotateSystem()
    {
        mouseThreshold = Screen.width / 3;
        mouseHalfScreen = Screen.width / 2;
        Cursor.visible = false;

        // Deals with logic for resizing to fit screen
        float scale = (Screen.width / 2) / 700f;
        rotateBar.transform.localScale = new Vector3(scale, scale, scale);
    }

    private void Update()
    {
        if (EventManager.PLAYER_DYING)
            return;
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


        // @TODO: Debug methods
        if (Input.GetKeyDown(KeyCode.I))
            PrintChunk();
        if (Input.GetKeyDown(KeyCode.P))
            Debug.Log("Why can't I rotate? CanRotate: " + canRotate + ", In_Menu: " + IN_MENU + ", cooldown: " + pivotCooldown);

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
        float deltaMousePos = Input.mousePosition.x - mouseHalfScreen;
        UpdateRotateBar(deltaMousePos);
        if (deltaMousePos > mouseThreshold)
        {
            if (canRotate && pivotCooldown == 0)
            {
                PivotWorld(true);
            }
        }
        else if (deltaMousePos < -mouseThreshold)
        {
            if (canRotate && pivotCooldown == 0)
            {
                PivotWorld(false);
            }
        }
    }

    private void UpdateRotateBar(float deltaMousePos)
    {
        float x = deltaMousePos / mouseThreshold * 295f;
        rotateBarPointer.localPosition = new Vector2(Mathf.Clamp(x, -320, 320), 0);
    }

    private string GetDirectionBarText(int newRotation)
    {
        string display = "";
        if (newRotation > 0 && newRotation < 4)
            display = "N";
        else if (newRotation > 4)
            display = "S";

        if (newRotation > 2 && newRotation < 6)
            display += "W";
        else if (newRotation > 6 || newRotation < 2)
            display += "E";
        return display;
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
        pivotCooldown = 0.66f;
        int newCameraRotation = (CAMERA_ROTATION + (clockwise ? 7 : 1)) % 8;
        rotateBarText.SetText(GetDirectionBarText(newCameraRotation));
        
        EventManager.WorldPivot(clockwise);
        Pivot(gameObject, clockwise);
        animations.RotateCamera(clockwise);

        CAMERA_ROTATION = newCameraRotation;
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
        if (!IN_MENU && !EventManager.PLAYER_DYING)
            Move();
    }

    public static void ToggleMove() => canRotate = !canRotate;

    private void Move()
    {
        Vector3 forwardDirection = transform.forward;
        Vector3 sideDirection = Vector3.Cross(forwardDirection, Vector3.up);

        Vector3 movementInput = Vector3.zero;
        if (!EventManager.PLAYER_DYING)
            movementInput = new Vector3(-Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        float movementIntoX = Vector3.Dot(movementInput, sideDirection);
        float movementIntoZ = Vector3.Dot(movementInput, forwardDirection);

        Vector3 modifiedMovement = new Vector3(movementIntoX, -1f, movementIntoZ);
        
        CONTROLLER.Move(speed * Time.fixedDeltaTime * modifiedMovement);
    }

    public void SetInMenu(bool inMenu)
    {
        IN_MENU = inMenu;
        rotateBar.SetActive(!inMenu);
        animations.SetInMenu(inMenu);
    }

    public void Hurt(float amount)
    {
        HEALTH.removeHealth(amount);
        
    }

    private void Die()
    {
        SetInMenu(false);
        rotateBar.SetActive(false);
    }

    public static float CurrentYRotation()
    {
        return WORLD_PLAYER.transform.eulerAngles.y;
    }

    public static void Pivot(GameObject obj, bool clockwise)
    {
        Quaternion currentRotation = obj.transform.rotation;
        float rotationChange = Mathf.Rad2Deg * 2 * Mathf.PI * (clockwise ? -WORLD_PLAYER.rotationChangeQuotient : WORLD_PLAYER.rotationChangeQuotient);
        Quaternion modifiedRotation = Quaternion.Euler(currentRotation.eulerAngles.x, currentRotation.eulerAngles.y - rotationChange, currentRotation.eulerAngles.z);
        LeanTween.rotate(obj, modifiedRotation.eulerAngles, WORLD_PLAYER.rotationTime).setEase(easeType)
            .setEase(easeType)
            .setOnStart(() => canRotate = false)
            .setOnComplete(e => canRotate = true);
    }

    public static void PlaySound(AudioClip clip) => PlaySound(clip, 0.3f);

    public static void PlaySound(AudioClip clip, float volume)
    {
        audioSource.pitch = 1 + RandomGen.Range(-0.3f, 0.3f);
        audioSource.PlayOneShot(clip, volume);
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

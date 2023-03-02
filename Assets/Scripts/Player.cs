using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{
    public static Player WORLD_PLAYER;
    public static Vector3Int TILE_POSITION = Vector3Int.zero;
    public static CharacterController CONTROLLER;
    public GameObject mainCamera;

    [SerializeField] private float speed;
    [SerializeField] private float rotation = Mathf.PI / 2;
    private static float pivotCooldown = 0;
    [SerializeField] private float cameraOffsetRadius;
    public float rotationChangeQuotient = 1f / 8f;
    public float rotationTime = 0.33f;
    public static bool canRotate = true;
    public static LeanTweenType easeType = LeanTweenType.easeOutQuint;

    private Animator anim;
    private SpriteRenderer sprite;
    private bool moving = false;
    private int[] direction = new int[] { 0, 0 };
    [SerializeField] private bool walkType;

    private int mouseThreshold;
    private int mouseCentrePos;
    private int mouseInput = 0;

    private void Awake()
    {
        WORLD_PLAYER = GetComponent<Player>();
    }

    private void Start()
    {
        CONTROLLER = GetComponent<CharacterController>();
        anim = transform.Find("Texture").GetComponent<Animator>();
        sprite = transform.Find("Texture").GetComponent<SpriteRenderer>();
        mouseThreshold = Screen.width / 3;
        mouseCentrePos = Screen.width / 2;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (pivotCooldown != 0)
        {
            TickPivotCooldown();
        }

        PlayerRotateInput();
        AnimatePlayerRotation();
        PlayerAnimation();

        CheckCurrentTile();
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
                PivotWorld(true);
                mouseInput = 1;
            }
        }
        else if (mouseCentrePos - Input.mousePosition.x < -mouseThreshold)
        {
            if (mouseInput != -1 && canRotate && pivotCooldown == 0)
            {
                PivotWorld(false);
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

        float rotationChange = 2 * Mathf.PI * (clockwise ? -rotationChangeQuotient : rotationChangeQuotient);
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
        Vector3 movementInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Quaternion forwardDirection = Quaternion.FromToRotation(Vector3.forward, transform.forward);
        Vector3 modifiedMovement = forwardDirection * movementInput;
        CONTROLLER.Move(speed * Time.fixedDeltaTime * modifiedMovement);
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

    private void AnimatePlayerRotation()
    {
        // Horizontal
        if (direction[0] == 0)
        {
            if (Input.GetKeyDown(KeyCode.A))
                SetAnimRotation(3);
            else if (Input.GetKeyDown(KeyCode.D))
                SetAnimRotation(1);
        }
        else
        {
            if (Input.GetKeyUp(KeyCode.A) && direction[0] == -1)
                CheckRotationChange(3);
            if (Input.GetKeyUp(KeyCode.D) && direction[0] == 1)
                CheckRotationChange(1);
        }

        // Vertical
        if (direction[1] == 0)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                if (direction[0] == 0)
                    SetAnimRotation(0);
                else
                    direction[1] = 1;
            }
            else if(Input.GetKeyDown(KeyCode.S))
            {
                if (direction[0] == 0)
                    SetAnimRotation(2);
                else
                    direction[1] = -1;
            }
        }
        else
        {
            if (Input.GetKeyUp(KeyCode.W) && direction[1] == 1)
                CheckRotationChange(0);
            if (Input.GetKeyUp(KeyCode.S) && direction[1] == -1)
                CheckRotationChange(2);
        }
    }

    void CheckRotationChange(int currentDir)
    {
        bool horizontalLifted = currentDir % 2 == 1;
        if (horizontalLifted)
        {
            direction[0] = 0;
        }
        else
        {
            direction[1] = 0;
            if (direction[0] != 0)  // Set direction to corresponding X direction if dir is already decided
            {
                SetAnimRotation(2 - direction[0]);
                return;
            }
        }
        
        if (direction[0] == 0) // Set direction to corresponding X direction if key is already pressed
        {
            if (Input.GetKey(KeyCode.A))
            {
                SetAnimRotation(3);
                return;
            }
            
            if (Input.GetKey(KeyCode.D))
            {
                SetAnimRotation(1);
                return;
            }
        }

        if (horizontalLifted && direction[1] != 0) // Set direction to corresponding Y direction if dir is already decided
        {
            SetAnimRotation(1 - direction[1]);
            return;
        }

        if (direction[1] == 0) // Set direction to corresponding Y direction if key is already pressed
        {
            if (Input.GetKey(KeyCode.W))
            {
                if (direction[0] == 0)
                    SetAnimRotation(0);
                else
                    direction[1] = 1;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                if (direction[0] == 0)
                    SetAnimRotation(2);
                else
                    direction[1] = -1;
            }
        }
    }

    void SetAnimRotation(int dir)
    {
        anim.SetInteger("Direction", dir);
        switch (dir)
        {
            case 0:
                direction[1] = 1;
                break;
            case 1:
                direction[0] = 1;
                sprite.flipX = false;
                break;
            case 2:
                direction[1] = -1;
                break;
            case 3:
                direction[0] = -1;
                sprite.flipX = true;
                break;
        }

        Debug.Log("Facing: " + dir);
    }

    void PlayerAnimation()
    {
        if (moving && CONTROLLER.velocity.magnitude <= 0.01f)
        {
            moving = false;
            anim.SetBool("Running", false);
        }
        if (!moving && CONTROLLER.velocity.magnitude > 0.01f)
        {
            moving = true;
            anim.SetBool("Running", true);
        }
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

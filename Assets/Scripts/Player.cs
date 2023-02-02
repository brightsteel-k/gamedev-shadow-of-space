using System;
using System.Collections;
using System.Collections.Generic;
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
    private const float initialRotation = 3 * Mathf.PI / 2;
    [SerializeField] private float cameraOffsetRadius;
    public float rotationChangeQuotient = 1 / 8;
    [SerializeField] private KeyCode rotateKey;
    public float rotationTime = 0.33f;
    public static bool canRotate = true;
    public static LeanTweenType easeType = LeanTweenType.easeOutQuint;

    private Animator anim;
    private SpriteRenderer sprite;
    private bool moving = false;
    private int[] direction = new int[] { 0, 0 };
    [SerializeField] private bool walkType;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        WORLD_PLAYER = GetComponent<Player>();
        anim = transform.Find("Texture").GetComponent<Animator>();
        sprite = transform.Find("Texture").GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(rotateKey) && canRotate)
        {
            PivotWorld();
        }

        PlayerRotation();
        PlayerAnimation();

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
        Vector3 movementInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Quaternion forwardDirection = Quaternion.FromToRotation(Vector3.forward, transform.forward);
        Vector3 modifiedMovement = forwardDirection * movementInput;
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
            .setEase(easeType)
            .setOnStart(ToggleMove)
            .setOnComplete(ToggleMove);
    }

    private void PlayerRotation()
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
        if (moving && characterController.velocity.magnitude <= 0.01f)
        {
            moving = false;
            anim.SetBool("Running", false);
        }
        if (!moving && characterController.velocity.magnitude > 0.01f)
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

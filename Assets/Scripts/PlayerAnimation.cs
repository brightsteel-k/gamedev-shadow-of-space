using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] GameObject textureObject;
    private Animator anim;
    private SpriteRenderer spriteRenderer;
    Sprite[] idleSprites = new Sprite[3];
    private bool moving = false;
    private int[] inputs = new int[] { 0, 0 };
    private int direction = 2;
    private int camDirectionOffset = 0;
    private string currentState = "Idle";


    // Start is called before the first frame update
    void Start()
    {
        idleSprites[0] = Resources.Load<Sprite>("Textures/Player/Standing_UP");
        idleSprites[1] = Resources.Load<Sprite>("Textures/Player/Standing_SIDE");
        idleSprites[2] = Resources.Load<Sprite>("Textures/Player/Standing_DOWN");
        anim = textureObject.GetComponent<Animator>();
        spriteRenderer = textureObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        ManageCameraOffset();
        ManageDirection();
        ManageVelocity();
    }

    private void ManageDirection()
    {
        // Horizontal
        if (inputs[0] == 0)
        {
            if (Input.GetKeyDown(KeyCode.A))
                SetDirection(3);
            else if (Input.GetKeyDown(KeyCode.D))
                SetDirection(1);
        }
        else
        {
            if (Input.GetKeyUp(KeyCode.A) && inputs[0] == -1)
                CheckRotationChange(3);
            if (Input.GetKeyUp(KeyCode.D) && inputs[0] == 1)
                CheckRotationChange(1);
        }

        // Vertical
        if (inputs[1] == 0)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                if (inputs[0] == 0)
                    SetDirection(0);
                else
                    inputs[1] = 1;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                if (inputs[0] == 0)
                    SetDirection(2);
                else
                    inputs[1] = -1;
            }
        }
        else
        {
            if (Input.GetKeyUp(KeyCode.W) && inputs[1] == 1)
                CheckRotationChange(0);
            if (Input.GetKeyUp(KeyCode.S) && inputs[1] == -1)
                CheckRotationChange(2);
        }
    }

    void CheckRotationChange(int currentDir)
    {
        bool horizontalLifted = currentDir % 2 == 1;
        if (horizontalLifted)
        {
            inputs[0] = 0;
        }
        else
        {
            inputs[1] = 0;
            if (inputs[0] != 0)  // Set direction to corresponding X direction if dir is already decided
            {
                SetDirection(2 - inputs[0]);
                return;
            }
        }

        if (inputs[0] == 0) // Set direction to corresponding X direction if key is already pressed
        {
            if (Input.GetKey(KeyCode.A))
            {
                SetDirection(3);
                return;
            }

            if (Input.GetKey(KeyCode.D))
            {
                SetDirection(1);
                return;
            }
        }

        if (horizontalLifted && inputs[1] != 0) // Set direction to corresponding Y direction if dir is already decided
        {
            SetDirection(1 - inputs[1]);
            return;
        }

        if (inputs[1] == 0) // Set direction to corresponding Y direction if key is already pressed
        {
            if (Input.GetKey(KeyCode.W))
            {
                if (inputs[0] == 0)
                    SetDirection(0);
                else
                    inputs[1] = 1;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                if (inputs[0] == 0)
                    SetDirection(2);
                else
                    inputs[1] = -1;
            }
        }
    }

    void ManageVelocity()
    {
        if (moving && Player.CONTROLLER.velocity.magnitude <= 0.01f)
        {
            moving = false;
            UpdateAnimationState();
        }
        if (!moving && Player.CONTROLLER.velocity.magnitude > 0.01f)
        {
            moving = true;
            UpdateAnimationState();
        }
    }

    void SetDirection(int dir)
    {
        direction = dir;
        switch (dir)
        {
            case 0:
                inputs[1] = 1;
                break;
            case 1:
                inputs[0] = 1;
                break;
            case 2:
                inputs[1] = -1;
                break;
            case 3:
                inputs[0] = -1;
                break;
        }
        UpdateAnimationState();
    }

    public void RotateCamera(bool clockwise)
    {
        if (!IsInUnmovingAnimationState())
            return;

        camDirectionOffset += clockwise ? 1 : -1;
        if (camDirectionOffset >= 2)
        {
            direction = (direction + 3) % 4;
            camDirectionOffset = 0;
            SetIdleState();
        }
        else if (camDirectionOffset <= -2)
        {
            direction = (direction + 1) % 4;
            camDirectionOffset = 0;
            SetIdleState();
        }
    }

    void ManageCameraOffset()
    {
        if (camDirectionOffset == 0)
            return;

        if (!IsInUnmovingAnimationState())
            camDirectionOffset = 0;
    }

    bool IsInUnmovingAnimationState()
    {
        return currentState == "Idle";
    }

    void UpdateAnimationState()
    {
        if (!anim.enabled) { anim.enabled = true; }
        if (moving)
        {
            SetRotatableState(AnimationState.Running.ToString());
        }
        else
        {
            SetIdleState();
        }
    }

    void SetRotatableState(string state)
    {
        string newState = "";
        switch (direction)
        {
            case 0:
                newState = state + " (UP)";
                break;
            case 1:
                spriteRenderer.flipX = false;
                newState = state + " (SIDE)";
                break;
            case 2:
                newState = state + " (DOWN)";
                break;
            case 3:
                spriteRenderer.flipX = true;
                newState = state + " (SIDE)";
                break;
        }
        currentState = newState;
        anim.Play(newState);
    }

    void SetIdleState()
    {
        anim.Play("Idle");
        anim.enabled = false;

        currentState = "Idle";
        switch (direction)
        {
            case 0:
                spriteRenderer.sprite = idleSprites[0];
                break;
            case 1:
                spriteRenderer.flipX = false;
                spriteRenderer.sprite = idleSprites[1];
                break;
            case 2:
                spriteRenderer.sprite = idleSprites[2];
                break;
            case 3:
                spriteRenderer.flipX = true;
                spriteRenderer.sprite = idleSprites[1];
                break;
        }
    }
}

enum AnimationState
{
    Idle = 0,
    Running = 1
}
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
    public int direction = 2;
    private int camDirectionOffset = 0;
    private AnimationState currentState = AnimationState.Idle;


    // Start is called before the first frame update
    void Start()
    {
        idleSprites[0] = Resources.Load<Sprite>("Textures/Player/Standing_UP");
        idleSprites[1] = Resources.Load<Sprite>("Textures/Player/Standing_SIDE");
        idleSprites[2] = Resources.Load<Sprite>("Textures/Player/Standing_DOWN");
        anim = textureObject.GetComponent<Animator>();
        spriteRenderer = textureObject.GetComponent<SpriteRenderer>();
        EventManager.OnPlayerDying += Die;
    }

    // Update is called once per frame
    void Update()
    {
        if (EventManager.PLAYER_DYING)
            return;

        ManageDirection();
        if (!Player.IN_MENU)
        {
            ManageCameraOffset();
            ManageVelocity();
        }
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
            SetRotatableState(currentState);
        }
        else if (camDirectionOffset <= -2)
        {
            direction = (direction + 1) % 4;
            camDirectionOffset = 0;
            SetRotatableState(currentState);
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
        return currentState == AnimationState.Idle;
    }

    public void SetInMenu(bool inMenu)
    {
        if (inMenu)
            SetRotatableState(AnimationState.Idle);
        else
            UpdateAnimationState();
    }

    private void Die()
    {
        anim.Play("Dying", 0);
        LeanTween.value(0.6f, 0f, 5f)
            .setEaseOutQuint()
            .setOnUpdate(e => anim.speed = e);
    }

    void UpdateAnimationState()
    {
        if (Player.IN_MENU)
            return;

        if (moving)
        {
            SetRotatableState(AnimationState.Running);
        }
        else
        {
            SetRotatableState(AnimationState.Idle);
        }
    }

    void SetRotatableState(AnimationState state)
    {
        string newState = state.ToString();
        switch (direction)
        {
            case 0:
                newState += " (UP)";
                break;
            case 1:
                spriteRenderer.flipX = false;
                newState += " (SIDE)";
                break;
            case 2:
                newState += " (DOWN)";
                break;
            case 3:
                spriteRenderer.flipX = true;
                newState += " (SIDE)";
                break;
        }
        currentState = state;
        anim.Play(newState, 0);
    }
}

enum AnimationState
{
    Idle = 0,
    Running = 1
}

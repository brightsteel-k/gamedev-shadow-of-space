using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StalkerAnimation : MonoBehaviour
{
    [SerializeField] private SpriteRenderer texture;
    private Animator anim;
    private NavMeshAgent navMeshAgent;
    private SpriteRenderer spriteRenderer;
    private int previousDirection = 2;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        anim = texture.transform.GetComponent<Animator>();
        spriteRenderer = texture.GetComponent<SpriteRenderer>();
    }

    public void UpdateAnimations()
    {
        ManageDirection();
    }

    private void ManageDirection()
    {
        int direction = 0;
        float theta = Player.CAMERA_ROTATION * Mathf.PI / 4f - Mathf.Atan2(navMeshAgent.velocity.z, navMeshAgent.velocity.x);
        float horizontal = Mathf.Sin(theta);
        float vertical = Mathf.Cos(theta);
        bool sideDominant = Mathf.Abs(horizontal) >= Mathf.Abs(vertical);

        if (sideDominant)
            direction = horizontal >= 0 ? 1 : 3;
        else
            direction = vertical > 0 ? 0 : 2;

        if (direction != previousDirection)
        {
            SetRunningDirection(direction);
            previousDirection = direction;
        }
    }

    void SetRunningDirection(int direction)
    {
        string animState = "Running ";
        switch (direction)
        {
            case 0:
                animState += "(UP)";
                break;
            case 1:
                spriteRenderer.flipX = false;
                animState += "(SIDE)";
                break;
            case 2:
                animState += "(DOWN)";
                break;
            case 3:
                spriteRenderer.flipX = true;
                animState += "(SIDE)";
                break;
        }
        anim.Play(animState, 0);
    }
}

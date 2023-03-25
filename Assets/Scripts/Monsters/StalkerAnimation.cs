using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StalkerAnimation : MonoBehaviour
{
    [SerializeField] private SpriteRenderer texture;
    private Animator anim;
    private NavMeshAgent navMeshAgent;
    public int internalDirection = 2;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        anim = texture.transform.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        ManageDirection();
    }

    private void ManageDirection()
    {
        float theta = Mathf.PI / 2 - Player.CAMERA_ROTATION * Mathf.PI / 4f;
        /*float dx = navMeshAgent.velocity.x;
        float dz = navMeshAgent.velocity.z;
        bool sideDominant = Mathf.Abs(dx) >= Mathf.Abs(dz);

        if (sideDominant)
        {
            if (dx > 0 && internalDirection != 2)
            {

            }
        }*/
    }

    private int calculateRelativeDirection(int playerCameraRotation)
    {
        return (internalDirection * 2 - playerCameraRotation + 8) % 8;
    }
}

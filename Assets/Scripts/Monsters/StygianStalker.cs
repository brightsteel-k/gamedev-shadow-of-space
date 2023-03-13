using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class StygianStalker : Rotatable
{
    private static Transform TARGET;
    [SerializeField] private Vector3 spawnPos;
    [SerializeField] float circleSpeed;
    [SerializeField] float circleRadius;
    [SerializeField] float circleWaypointThreshold;
    private StygianMode currentMode;
    private bool corporeal = false;
    private Vector3 incorporealPos;
    private Vector3 targetPos;
    private float playerPosUpdateTime = 12f;
    private NavMeshAgent navMeshAgent;

    private float stalkingTheta;
    private int stalkingCircles;

    // Start is called before the first frame update
    protected override void Start()
    {
        textureObject = transform.Find("Texture").gameObject;

        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.autoBraking = false;
        TARGET = Player.WORLD_PLAYER.transform;
        if (!corporeal)
            StartIncorporeal();
    }

    // Update is called once per frame
    void Update()
    {
        if (!corporeal)
        {
            IncorporealPosition();
            return;
        }

        if (currentMode == StygianMode.Stalking)
            CirclePlayer();
    }

    void StartIncorporeal()
    {
        currentMode = StygianMode.Tracking;
        incorporealPos = spawnPos;
        targetPos = TARGET.position;
        StartCoroutine("UpdatePlayerLocation");
    }

    void IncorporealPosition()
    {
        Vector3 deltaVec = Vector3.Normalize(targetPos - incorporealPos);
        incorporealPos += deltaVec * Time.deltaTime * 10f;

        if (Vector3.Distance(incorporealPos, TARGET.position) <= 30f)
            SpawnStalker();
    }

    void SpawnStalker()
    {
        transform.position = incorporealPos;
        navMeshAgent.enabled = true;
        InitRotation();
        textureObject.SetActive(true);
        corporeal = true;

        BeginCircling();
    }

    void BeginCircling()
    {
        Debug.Log("Stalking!");
        currentMode = StygianMode.Stalking;
        navMeshAgent.speed = circleSpeed;

        stalkingTheta = Mathf.Atan2(transform.position.z - TARGET.position.z, transform.position.x - TARGET.position.x);
        NextCirclePos();
    }


    private void CirclePlayer()
    {
        if (Vector3.Distance(transform.position, targetPos) < circleWaypointThreshold)
        {
            NextCirclePos();
            Debug.Log("Distance to: " + targetPos);
        }
    }

    private void NextCirclePos()
    {
        stalkingTheta += 1f;
        if (stalkingTheta > 2 * Mathf.PI)
        {
            if (!RandomGen.ShouldContinueCircling(stalkingCircles))
            {
                BeginAttacking();
            }
        }
        targetPos = TARGET.position + new Vector3(circleRadius * Mathf.Cos(stalkingTheta), 0f, circleRadius * Mathf.Sin(stalkingTheta));
        navMeshAgent.SetDestination(targetPos);
        Debug.Log("Next Circle Waypoint - Traveling to: " + targetPos);
    }

    void BeginAttacking()
    {
        Debug.Log("Attacking!");
    }

    private IEnumerator UpdatePlayerLocation()
    {
        while (!corporeal)
        {
            yield return new WaitForSeconds(playerPosUpdateTime);
            if (!corporeal)
            {
                targetPos = TARGET.position;
                if (playerPosUpdateTime > 1f)
                    playerPosUpdateTime--;
                else if (playerPosUpdateTime == 1f)
                    playerPosUpdateTime = 0.5f;
            }
        }
    }

    protected override void Pivot(bool clockwise)
    {
        if (corporeal)
            Player.Pivot(textureObject, clockwise);
    }
}

public enum StygianMode
{
    Stalking,
    Attacking,
    Fleeing,
    Tracking
}
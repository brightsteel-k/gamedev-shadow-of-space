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
    private StygianMode currentMode = StygianMode.Tracking;
    private bool corporeal = false;
    private Vector3 incorporealPos;
    private Vector3 targetPos;
    private float playerPosUpdateTime = 12f;
    private NavMeshAgent navMeshAgent;

    private float stalkingTheta;
    private int stalkingCircles;

    private int numTimesFled = 0;

    // Start is called before the first frame update
    protected override void Start()
    {
        textureObject = transform.Find("Texture").gameObject;

        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.autoBraking = false;
        TARGET = Player.WORLD_PLAYER.transform;
        incorporealPos = spawnPos;
        StartTracking();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.P)) // TODO: REMOVE DEBUGGING TOOL
        {
            BeginFleeing(TARGET.position);
        }


        if (currentMode == StygianMode.Tracking)
            TrackingPosition();
        else if (currentMode == StygianMode.Stalking)
            CirclePlayer();
        else if (currentMode == StygianMode.Attacking)
            ChargePlayer();
        else if (currentMode == StygianMode.Fleeing)
            DoFleeing();
    }

    void StartTracking()
    {
        Debug.Log("Tracking");
        currentMode = StygianMode.Tracking;
        corporeal = false;
        targetPos = TARGET.position;
        StartCoroutine("UpdatePlayerLocation");
    }

    void TrackingPosition()
    {
        Vector3 deltaVec = Vector3.Normalize(targetPos - incorporealPos);
        incorporealPos += deltaVec * Time.deltaTime * 10f;

        if (Vector3.Distance(incorporealPos, TARGET.position) <= 30f)
            SpawnStalker();
    }

    void SpawnStalker()
    {
        corporeal = true;
        transform.position = incorporealPos;
        navMeshAgent.enabled = true;
        InitRotation();
        textureObject.SetActive(true);

        BeginCircling();
    }

    // STALKING = = = = = = = = = = = = = = = = = = = = = = = = [BEGINNING OF STALKING PHASE] = = = = = = = = = = = = = = = = = = = = = = = = STALKING

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
    }
    // STALKING = = = = = = = = = = = = = = = = = = = = = = = = [END OF STALKING PHASE] = = = = = = = = = = = = = = = = = = = = = = = = STALKING
    // ATTACKING = = = = = = = = = = = = = = = = = = = = = = [BEGINNING OF ATTACKING PHASE] = = = = = = = = = = = = = = = = = = = = = = ATTACKING

    void BeginAttacking()
    {
        Debug.Log("Attacking!");
        currentMode = StygianMode.Attacking;
    }

    void ChargePlayer()
    {
        navMeshAgent.SetDestination(TARGET.position + Player.CONTROLLER.velocity);
    }

    // ATTACKING = = = = = = = = = = = = = = = = = = = = = = = [END OF ATTACKING PHASE] = = = = = = = = = = = = = = = = = = = = = = = ATTACKING
    // FLEEING = = = = = = = = = = = = = = = = = = = = = = = [BEGINNING OF FLEEING PHASE] = = = = = = = = = = = = = = = = = = = = = = = FLEEING

    void BeginFleeing(Vector3 source)
    {
        Debug.Log("Fleeing!");
        currentMode = StygianMode.Fleeing;
        numTimesFled++;
        float theta = Mathf.Atan2(transform.position.z - source.z, transform.position.x - source.x);
        theta = RandomGen.MercuryDirection(theta);

        Vector3 direction = new Vector3(Mathf.Cos(theta), 0, Mathf.Sin(theta));
        targetPos = ChunkHandler.BoundCoordinate(direction.normalized * 80f);
        navMeshAgent.SetDestination(targetPos);
    }

    void DoFleeing()
    {
        if (Vector3.Distance(transform.position, targetPos) < 4f)
        {
            if (Vector3.Distance(transform.position, TARGET.position) > 40f)
                BeginRecuperating();
            else
                BeginAttacking();
        }
    }

    // FLEEING = = = = = = = = = = = = = = = = = = = = = = = = [END OF FLEEING PHASE] = = = = = = = = = = = = = = = = = = = = = = = = FLEEING
    // RECUPERATING = = = = = = = = = = = = = = = = = = = [BEGINNING OF RECUPERATING PHASE] = = = = = = = = = = = = = = = = = = = RECUPERATING

    void BeginRecuperating()
    {
        Debug.Log("Recuperating!");
        currentMode = StygianMode.Recuperating;
        incorporealPos = transform.position;
        navMeshAgent.enabled = false;
        corporeal = false;
        textureObject.SetActive(false);

        StartCoroutine("Recuperate");
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "MobileFlare")
        {
            BeginFleeing(other.transform.position);
        }
    }

    private IEnumerator UpdatePlayerLocation()
    {
        while (currentMode == StygianMode.Tracking)
        {
            yield return new WaitForSeconds(playerPosUpdateTime);
            if (currentMode == StygianMode.Tracking)
            {
                targetPos = TARGET.position;
                if (playerPosUpdateTime > 1f)
                    playerPosUpdateTime--;
                else if (playerPosUpdateTime == 1f)
                    playerPosUpdateTime = 0.5f;
            }
        }
    }

    private IEnumerator Recuperate()
    {
        yield return new WaitForSeconds(RandomGen.RecuperationTime(numTimesFled));
        StartTracking();
    }

    protected override void Pivot(bool clockwise)
    {
        if (corporeal)
            Player.Pivot(textureObject, clockwise);
    }
}

public enum StygianMode
{
    Tracking,
    Stalking,
    Attacking,
    Fleeing,
    Recuperating
}
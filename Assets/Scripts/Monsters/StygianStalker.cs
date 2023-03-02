using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class StygianStalker : Rotatable
{
    private static Transform TARGET;
    [SerializeField] private Vector3 spawnPos;
    [SerializeField] private Transform marker;
    private StygianMode currentMode;
    private bool corporeal = false;
    private Vector3 incorporealPos;
    private Vector3 targetPos;
    private float playerPosUpdateTime = 12f;
    private NavMeshAgent navMeshAgent;

    private int stalkingCircles;
    private int stalkingMaxCircles;

    // Start is called before the first frame update
    protected override void Start()
    {
        textureObject = transform.Find("Texture").gameObject;
        InitRotation();

        navMeshAgent = GetComponent<NavMeshAgent>();
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
        marker.position = incorporealPos;

        if (Vector3.Distance(incorporealPos, TARGET.position) <= 30f)
            SpawnStalker();
    }

    void SpawnStalker()
    {
        navMeshAgent.enabled = true;
        textureObject.SetActive(true);
        corporeal = true;

        currentMode = StygianMode.Stalking;
        StalkPlayer();
    }

    void StalkPlayer()
    {
        Debug.Log("Stalking!");
        stalkingMaxCircles = Random.Range(2, 6);
        stalkingCircles = 0;
        StartCoroutine("CirclePlayer");
    }

    protected override void Pivot(bool clockwise)
    {
        if (corporeal)
            Player.Pivot(textureObject, clockwise);
    }

    private IEnumerator UpdatePlayerLocation()
    {
        while (!corporeal)
        {
            yield return new WaitForSeconds(playerPosUpdateTime);
            targetPos = TARGET.position;
            if (playerPosUpdateTime > 1f)
                playerPosUpdateTime--;
            else if (playerPosUpdateTime == 1f)
                playerPosUpdateTime = 0.5f;
        }
    }

    private IEnumerator CirclePlayer()
    {
        float theta = 0;
        while (theta < 6.3f)
        {
            Vector3 newPos = TARGET.position + new Vector3(8f * Mathf.Cos(theta), 0f, 8f * Mathf.Sin(theta));
            float newSpeed = Player.CONTROLLER.velocity.magnitude * 4f + 3f;
            LeanTween.value(navMeshAgent.speed, newSpeed, 0.2f).setEaseOutQuad();
            navMeshAgent.SetDestination(newPos);
            theta += 0.3f;
            yield return new WaitForSeconds(0.6f);
        }

        if (++stalkingCircles < stalkingMaxCircles)
        {
            StartCoroutine("CirclePlayer");
        }
    }
}

public enum StygianMode
{
    Stalking,
    Attacking,
    Fleeing,
    Tracking
}
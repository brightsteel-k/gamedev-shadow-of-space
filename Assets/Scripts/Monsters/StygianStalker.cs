using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class StygianStalker : Rotatable
{
    public static StygianStalker WORLD_STALKER;
    private static Transform TARGET;
    [SerializeField] private Vector3 spawnPos;
    [SerializeField] float circleSpeed;
    [SerializeField] float circleRadius;
    [SerializeField] float circleWaypointThreshold;
    private StygianMode currentMode = StygianMode.Recuperating;
    private bool corporeal;
    private Vector3 incorporealPos;
    private Vector3 targetPos;
    private float playerPosUpdateTime = 12f;
    private NavMeshAgent navMeshAgent;
    private StalkerAnimation animations;

    private float stalkingTheta;
    private int stalkingCircles = 0;

    private int numTimesFled = 0;
    private float attackCooldown = 0;
    private bool assaultEnded;

    [Header("Sounds")]
    private AudioSource audioSource;
    private bool charged = false;
    [SerializeField] AudioClip roarClip;
    [SerializeField] AudioClip biteClip;

    // Start is called before the first frame update
    protected override void Start()
    {
        textureObject = transform.Find("Texture").gameObject;
        WORLD_STALKER = this;
        corporeal = false;

        navMeshAgent = GetComponent<NavMeshAgent>();
        animations = GetComponent<StalkerAnimation>();
        navMeshAgent.autoBraking = false;
        audioSource = GetComponent<AudioSource>();
        TARGET = Player.WORLD_PLAYER.transform;
        incorporealPos = spawnPos;
    }

    private void Update()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (corporeal)
            animations.UpdateAnimations();

        if (currentMode == StygianMode.Tracking)
            TrackingPosition();
        else if (currentMode == StygianMode.Stalking)
            CirclePlayer();
        else if (currentMode == StygianMode.Attacking)
            ChargePlayer();
        else if (currentMode == StygianMode.Fleeing)
            DoFleeing();

        if (attackCooldown > 0)
            attackCooldown -= Time.deltaTime;
        else if (attackCooldown < 0)
            attackCooldown = 0;

        RoarWhenCharging();
    }

    public void StartTracking()
    {
        gameObject.SetActive(true);
        currentMode = StygianMode.Tracking;
        corporeal = false;
        targetPos = TARGET.position;
        StartCoroutine("UpdatePlayerLocation");
    }

    void TrackingPosition()
    {
        Vector3 deltaVec = Vector3.Normalize(targetPos - incorporealPos);
        incorporealPos += deltaVec * Time.deltaTime * 15f;

        if (Vector3.Distance(incorporealPos, TARGET.position) <= 30f)
            SpawnStalker();
    }

    void SpawnStalker()
    {
        corporeal = true;
        assaultEnded = true;
        transform.position = incorporealPos;
        navMeshAgent.enabled = true;
        InitRotation();
        textureObject.SetActive(true);

        BeginCircling();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!corporeal)
            return;

        switch (other.tag)
        {
            case "Breaking":
            case "Breakable":
                BreakThroughObject(other.GetComponent<LargeObject>());
                break;
            case "Player":
                AttackPlayer(other.GetComponent<Player>());
                break;
            case "NewFlare":
                BeginFleeing(other.transform.position);
                other.GetComponent<Flare>().Detonate();
                break;
        }
    }

    void BreakThroughObject(LargeObject largeObject)
    {
        largeObject.BreakObject();
    }

    void AttackPlayer(Player player)
    {
        if (attackCooldown > 0 || currentMode == StygianMode.Fleeing)
            return;

        player.Hurt(85);
        audioSource.PlayOneShot(biteClip);
        attackCooldown = 5f;

        BeginCircling();
    }

    void RoarWhenCharging()
    {
        if (!corporeal)
        {
            if (charged)
                charged = false;
            return;
        }

        
        float distance = Vector3.Distance(transform.position, TARGET.position);
        if (charged)
        {
            if (distance > 40)
                charged = false;
        }
        else
        {
            if (distance < 30)
            {
                audioSource.PlayOneShot(roarClip, 2f);
                charged = true;
            }
        }
    }

    // STALKING = = = = = = = = = = = = = = = = = = = = = = = = [BEGINNING OF STALKING PHASE] = = = = = = = = = = = = = = = = = = = = = = = = STALKING

    void BeginCircling()
    {
        currentMode = StygianMode.Stalking;
        navMeshAgent.speed = circleSpeed;
        stalkingCircles = 0;

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
            if (!RandomGen.ShouldContinueCircling(stalkingCircles++))
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
        currentMode = StygianMode.Attacking;
        if (assaultEnded)
        {
            MusicManager.SetAttackingTrack(true);
            assaultEnded = false;
        }
        if (!Player.WORLD_PLAYER.hasEncounteredMonster)
            Player.WORLD_PLAYER.hasEncounteredMonster = true;
    }

    void ChargePlayer()
    {
        // Vector3 monsterToPlayer = Player.CONTROLLER.transform.position - transform.position;
        // Vector3 cross = Vector3.Cross(monsterToPlayer, Vector3.up).normalized;
        // Vector3 projection = Vector3.Dot(cross, Player.CONTROLLER.velocity) * Player.CONTROLLER.velocity / 4;
        // navMeshAgent.SetDestination(TARGET.position + projection);
        // sphere.position = TARGET.position + projection;
        if (Vector3.Distance(transform.position, TARGET.position) > 3f)
            navMeshAgent.SetDestination(TARGET.position + Player.CONTROLLER.velocity);
        else
            navMeshAgent.SetDestination(TARGET.position);
    }

    // ATTACKING = = = = = = = = = = = = = = = = = = = = = = = [END OF ATTACKING PHASE] = = = = = = = = = = = = = = = = = = = = = = = ATTACKING
    // FLEEING = = = = = = = = = = = = = = = = = = = = = = = [BEGINNING OF FLEEING PHASE] = = = = = = = = = = = = = = = = = = = = = = = FLEEING

    public void BeginFleeing(Vector3 source)
    {
        currentMode = StygianMode.Fleeing;
        assaultEnded = true;
        numTimesFled++;
        float theta = Mathf.Atan2(transform.position.z - source.z, transform.position.x - source.x);
        theta = RandomGen.MercuryDirection(theta);

        audioSource.pitch = 2.2f;
        audioSource.PlayOneShot(roarClip);
        MusicManager.SetAttackingTrack(false);
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
        currentMode = StygianMode.Recuperating;
        incorporealPos = transform.position;
        navMeshAgent.enabled = false;
        corporeal = false;
        textureObject.SetActive(false);

        StartCoroutine("Recuperate");
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
        {
            VerifyRotation();
            Player.Pivot(textureObject, clockwise);
        }
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
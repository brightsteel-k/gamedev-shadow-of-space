using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOperator : MonoBehaviour
{
    public Item selectedItem;
    private Energy energy;
    private Inventory inventory;

    private bool isDrilling = false;
    private LargeObject currentlyDrilling;
    private float drillingTimer = 0;
    Transform currentDrillParticles;

    private int groundLayer;
    private bool isThrowing = false;
    [SerializeField] GameObject flarePrefab;
    

    // Start is called before the first frame update
    void Start()
    {
        energy = GetComponent<Energy>();
        inventory = GetComponent<Inventory>();
        groundLayer = LayerMask.GetMask("Ground");
    }

    // Update is called once per frame
    void Update()
    {
        if (isDrilling && !Player.IN_MENU)
            UpdateDrilling();

        if (isThrowing && !Player.IN_MENU)
            UpdateThrowing();
    }

    public void TryDropSelectedItem()
    {
        if (selectedItem != null)
        {
            Environment.DropItem(selectedItem, CentrePos());
            inventory.removeSelectedItem();
        }
    }


    public void UseSelectedItem()
    {
        if (selectedItem == null)
            return;
        switch (selectedItem.id)
        {
            case "battery":
                SwitchBatteries(selectedItem);
                break;
            case "drill":
                StartDrilling();
                break;
            case "flare":
                StartThrowing();
                break;
        }
    }

    private void SwitchBatteries(Item battery)
    {
        Item newBattery = energy.SwitchBatteries(battery);
        inventory.removeSelectedItem();
        Environment.DropItem(newBattery, CentrePos());
    }

    private void StartDrilling()
    {
        isDrilling = true;
        energy.SetDrilling(true);
    }

    private void UpdateDrilling()
    {
        RaycastHit hit;
        if (Physics.Raycast(CentrePos(), Player.WORLD_PLAYER.GetDirection(), out hit, 2))
        {
            if (hit.transform.tag == "Breakable")
            {
                currentlyDrilling = hit.transform.GetComponent<LargeObject>();
                hit.transform.tag = "Breaking";
                drillingTimer = 0;
                currentDrillParticles = Instantiate(currentlyDrilling.GetMineParticles(), hit.point + Player.WORLD_PLAYER.GetDirection() * 0.1f, Quaternion.identity).transform;
                currentDrillParticles.LookAt(transform.position + new Vector3(0, 3, 0));
            }

            if (hit.transform.tag != "Breaking")
                LoseDrillingSubject();

            if (currentlyDrilling != null)
            {
                drillingTimer += Time.deltaTime;
                if (drillingTimer >= currentlyDrilling.breakTime)
                    BreakDrillingSubject();
            }
        }
        else if (currentlyDrilling != null)
        {
            LoseDrillingSubject();
        }

        
        if (Input.GetMouseButtonUp(1) || selectedItem.id != "drill")
            StopDrilling();
    }

    private void LoseDrillingSubject()
    {
        if (currentlyDrilling != null)
            currentlyDrilling.tag = "Breakable";
        if (currentDrillParticles != null)
            currentDrillParticles.GetComponent<CollidingParticles>().active = false;
        currentlyDrilling = null;
        drillingTimer = 0;
    }

    private void BreakDrillingSubject()
    {
        currentlyDrilling.BreakObject();
        currentlyDrilling = null;
        StopDrilling();
    }

    private void StopDrilling()
    {
        LoseDrillingSubject();
        isDrilling = false;
        energy.SetDrilling(false);
    }

    private void StartThrowing()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isThrowing = true;
    }

    private void UpdateThrowing()
    {
        if (Input.GetMouseButtonUp(1))
            ThrowFlare();

        if (selectedItem == null || selectedItem.id != "flare")
            StopThrowing();
    }

    private void ThrowFlare()
    {
        RaycastHit hit;
        Ray ray = Player.MAIN_CAMERA.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray.origin, ray.direction, out hit, 100f, groundLayer))
        {
            Rigidbody flare = Instantiate(flarePrefab, CentrePos(), Quaternion.identity).GetComponent<Rigidbody>();
            flare.velocity = CalculateVelocity(new Vector3(transform.position.x, 0, transform.position.z), hit.point);
            flare.angularVelocity = RandomGen.RandomRotation();
        }
        else
        {
            Instantiate(flarePrefab, CentrePos(), Quaternion.identity);
        }
        inventory.removeSelectedItem();
        StopThrowing();
    }

    private Vector3 CalculateVelocity(Vector3 sourcePos, Vector3 targetPos)
    {
        float distance = Vector3.Distance(transform.position, targetPos);
        float vy = 3f;
        float vx = -0.5f * vy * distance + (distance / 2) * Mathf.Sqrt(vy + 19.62f);
        float theta = Mathf.Atan2(targetPos.z - sourcePos.z, targetPos.x - sourcePos.x);
        return new Vector3(vx * Mathf.Cos(theta), vy, vx * Mathf.Sin(theta));
    }

    private void StopThrowing()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isThrowing = false;
    }

    public Vector3 CentrePos()
    {
        return transform.position + Vector3.up;
    }

    public void OnDrawGizmos()
    {
        /*Vector3 centre = CentrePos();
        Gizmos.DrawLine(centre, centre + Player.WORLD_PLAYER.GetDirection() * 2);*/
    }
}

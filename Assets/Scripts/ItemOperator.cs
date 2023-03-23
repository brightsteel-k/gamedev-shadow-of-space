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

    // Start is called before the first frame update
    void Start()
    {
        energy = GetComponent<Energy>();
        inventory = GetComponent<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDrilling && !Player.IN_MENU)
            UpdateDrilling();
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

        }
    }

    private void SwitchBatteries(Item battery)
    {
        Item newBattery = energy.SwitchBatteries(battery);
        inventory.removeSelectedItem();
        Environment.DropItem(newBattery, transform.position + CentrePos());
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
            }

            if (hit.transform.tag != "Breaking")
                LoseDrillingSubject();

            if (currentlyDrilling != null)
            {
                drillingTimer += Time.deltaTime;
                currentDrillParticles = Instantiate(currentlyDrilling.GetBreakParticles(), hit.transform.position, Quaternion.identity).transform;
                currentDrillParticles.LookAt(transform.position + new Vector3(0, 3, 0));
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
        currentlyDrilling = null;
        drillingTimer = 0;
    }

    private void BreakDrillingSubject()
    {
        currentlyDrilling.BreakObject();
        currentlyDrilling = null;
        drillingTimer = 0;
    }

    private void StopDrilling()
    {
        isDrilling = false;
        energy.SetDrilling(false);
        drillingTimer = 0;
    }

    public Vector3 CentrePos()
    {
        return transform.position + Vector3.up;
    }
}

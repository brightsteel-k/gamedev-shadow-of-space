using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryHandler : MonoBehaviour
{
    public Inventory inventory;
    public Grid inventoryGrid;
    public Sprite DEFAULT_SPRITE;
    private void Start()
    {
        inventory = new Inventory();
        inventory.items[0, 0] = new Item("Sword", 0, "Attack!", DEFAULT_SPRITE);
    }

    public void MoveIntoView()
    {
        
    }

    public void MoveOutOfView()
    {
        
    }

    private void Update()
    {
        
    }
}

[System.Serializable]
public class Inventory
{
    public Item[,] items = new Item[2,7];

}

public class Item 
{
    public string itemName;
    public int id;
    public int quantity;
    public Sprite sprite;
    public string description;

    public Item(string itemName, int id, string description, Sprite sprite, int quantity = 1)
    {
        this.itemName = itemName;
        this.id = id;
        this.sprite = sprite;
        this.quantity = quantity;
        this.description = description;
    }

    //public void IncreaseQuantity(int addedQuantity = 1) => this.quantity += addedQuantity;
}
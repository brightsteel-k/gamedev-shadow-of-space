using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Item", menuName = "Crafting/Recipe")]
public class Recipe : ScriptableObject
{
    [Serializable]
    public struct Pair
    {
        public int amount;
        public Item item;
    }
    
    [SerializeField]
    public List<Pair> needed;
    //Could be replaced with the serialized pair.
    //In case more then 1 of the resulting item is made.
    public Item created;
}

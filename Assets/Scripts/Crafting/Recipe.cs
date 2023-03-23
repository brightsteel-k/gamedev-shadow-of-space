using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Item", menuName = "Crafting/Recipe")]
public class Recipe : ScriptableObject
{
    [Serializable]
    public struct Pair1
    {
        public int amount;
        public Item item;
    }
    
    [Serializable]
    public struct Pair2
    {
        public int amount;
        public ResourceType res;
    }
    
    [SerializeField]
    public List<Pair1> needed;

    [SerializeField]
    public List<Pair2> liquids;
    //Could be replaced with the serialized pair.
    //In case more then 1 of the resulting item is made.
    public Item created;

    public float amount = 1; //For liquids
}

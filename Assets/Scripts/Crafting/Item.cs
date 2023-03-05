using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
[CreateAssetMenu(fileName = "New Item", menuName = "Crafting/Item")]
public class Item : ScriptableObject
{
    //Maybe better to use enums?
    public string type;

    public Sprite sprite;
    
}

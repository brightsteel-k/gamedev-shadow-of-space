using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

 //The serializable with info about the type
[System.Serializable]
[CreateAssetMenu(fileName = "New ResourceType" , menuName = "Crafting/ResourceType")]
public class ResourceType : ScriptableObject
{
    public string id;

    public Color color;

    public float maximum;

}

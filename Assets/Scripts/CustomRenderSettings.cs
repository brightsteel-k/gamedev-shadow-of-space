using UnityEngine;

[CreateAssetMenu(fileName = "New Render Settings", menuName = "Custom Render Settings")]
public class CustomRenderSettings : ScriptableObject
{
    public Color32 colour;

    public bool fogEnabled;
    public Color32 fogColour;
    public float fogIntensity;
}

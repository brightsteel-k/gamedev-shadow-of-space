using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
[CreateAssetMenu(fileName = "New Item", menuName = "Crafting/Item")]
public class Item : ScriptableObject
{
    public string displayName;

    public string id;

    public Sprite sprite;

    public bool isLiquid;
    public ResourceType madeLiq;
    public Dictionary<string, float> tags = new Dictionary<string, float>();

    public Item Initialize(Item template, params (string, float)[] tags)
    {
        displayName = template.displayName;
        id = template.id;
        sprite = template.sprite;
        SetTags(tags);
        return this;
    }

    public void SetTags(params (string, float)[] tags)
    {
        foreach ((string, float) pair in tags)
        {
            this.tags.Add(pair.Item1, pair.Item2);
        }
    }

    public float GetTag(string key)
    {
        float result = 0;
        if (tags.TryGetValue(key, out result))
        {
            return result;
        }
        
        return 0;
    }

    public void SetTag(string key, float value)
    {
        if (tags.ContainsKey(key))
            tags[key] = value;
        else
            tags.Add(key, value);
    }
}

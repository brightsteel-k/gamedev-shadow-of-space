using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ComponentUI : MonoBehaviour
{
    public Image icon;
    public TMP_Text text;
    public TMP_Text amount;
    public TMP_Text held;
    public void setValues(Sprite _sprite, string _name, string _amount, string _held)
    {
        icon.sprite = _sprite;
        text.text = _name;
        amount.text = _amount;
        held.text = _held;
    }
}

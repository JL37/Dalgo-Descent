using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemScripableObject", menuName = "ScriptableItems", order = 1)]
public class ItemScripableObject : ScriptableObject
{
    public string ItemName;
    public string Description;
    public Sprite SpriteImage;

}

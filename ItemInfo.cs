using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Items/Create Item")]
public class ItemInfo : ScriptableObject
{
    public new string name;
    public Sprite image_2d;
    public GameObject the_item;
}

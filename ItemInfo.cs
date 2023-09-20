using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

[CreateAssetMenu(fileName = "New Item", menuName = "Items/Create Item")]
public class ItemInfo : ScriptableObject
{
    public new string name;
    public bool isAnimationRigging;
    public Sprite image_2d;
    public GameObject the_item;
    public Vector3 item_Pos;
    public Vector3 parent_Pos;
    public Quaternion item_Rot;
    public Quaternion parent_Rot;
    public Vector3 leftHint;
    public Vector3 rightHint;
    public AudioClip _reloadClip;
    public AudioClip _shotClip;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ItemInfo_Component : MonoBehaviour
{
    [Header("Item Info")]
    public ItemInfo info;

    public void Awake()
    {
        info.the_item = gameObject;
    }
}

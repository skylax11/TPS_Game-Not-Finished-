using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [Header("Slot Items")]
    [SerializeField] GameObject[] slot_Images;
    [SerializeField] SlotItemInfos[] slot_Items;
    public static Inventory instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            return;
        }
    }
    private void Start()
    {
        slot_Items = new SlotItemInfos[9];

        for (int i = 0; i < slot_Items.Length; i++)
        {
            slot_Items[i] = new SlotItemInfos(i);
            print(slot_Items[i].name);
        }
    }
    public void SwitchItems(float index)
    {
       
    }
}
public class SlotItemInfos
{
    public string name;
    public Sprite image_2d;
    public GameObject the_item;
    public SlotItemInfos(int index)
    {
        name = "Slot" + index+1;
    }
}
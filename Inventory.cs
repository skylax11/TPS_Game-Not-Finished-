using Assets.Scripts;
using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [Header("Slot Items")]
    [SerializeField] public GameObject[] slot_Images;
    [SerializeField] public SlotItemInfos[] All_Items;
    public SlotItemInfos current_item;
    public int previous_index;
    public static Inventory instance;
    [SerializeField] public GameObject empty_GameObject;
    [Header("Weapon Parent")]
    [SerializeField] GameObject wepParent;
    [Header("Raycast")]
    RaycastHit hit;
    [SerializeField] GameObject cam;
    WeaponComponent item;
    [Header("The Panel")]
    [SerializeField] GameObject inventory_Panel;
    public bool isOpen;
    [Header("TPS Controller")]
    [TypeFilter(typeof(ThirdPersonController))]
    public ThirdPersonController tps;

    private void Awake()
    {
        isOpen = false;

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

        previous_index = 0;
        All_Items = new SlotItemInfos[37];

        for (int i = 0; i < 37; i++)
        {
            All_Items[i] = new SlotItemInfos(i);
            All_Items[i].the_item = empty_GameObject;
            All_Items[i].id = i;
        }
        current_item = All_Items[0];
    }
    public void SwapIcon(int first, int second)
    {
        GameObject temp = slot_Images[first];
        slot_Images[first] = slot_Images[second];
        slot_Images[second] = temp;
    }
    public void SwitchItems(float index)
    {
        
        previous_index = current_item.id;
        current_item = All_Items[(int)index];
        All_Items[previous_index].the_item.gameObject.SetActive(false);
        All_Items[(int)index].the_item.gameObject.SetActive(true);


        if (All_Items[(int)index].isAnimationRigging)
        {
           

           TPSController.instance.canShoot = true;
           TPSController.instance.PoseRig.weight = 1f;
           TPSController.instance.HandRig.weight = 1f;
        }
        else
        {
           TPSController.instance.canShoot = false;
           TPSController.instance.aimRig.weight = 0f;
           TPSController.instance.PoseRig.weight = 0f;
           TPSController.instance.ShootingRig.weight = 0f;
           TPSController.instance.HandRig.weight = 0f;
        }
        if(current_item.isAnimationRigging)
        {
            SetRigDetails(current_item.weapon_rotation, current_item.pos_offset,current_item.left,current_item.right,current_item.leftHint,current_item.rightHint);

            wepParent.transform.position = current_item.parent_Pos;
            wepParent.transform.rotation = current_item.parent_Rot;
        }
    }
    public void SetRigDetails(Quaternion[] parent, Vector3[] position,GameObject left,GameObject right, Vector3 leftHint, Vector3 rightHint)
    {
        for (int i = 0; i < item.pos_offset.Length; i++)
        {
            print(parent[i]);
            TPSController.instance.position_aiming_shooting_Parent[i].gameObject.transform.localRotation = parent[i]; // BURAYI AYARLA
            print(TPSController.instance.position_aiming_shooting_Parent[i].gameObject.name);
            TPSController.instance.position_aiming_shooting_Pos[i].data.offset = position[i];      // Assignments for Rigs
        }
        if (item.right != null)
        {
            TPSController.instance.HandRig.GetComponentsInChildren<TwoBoneIKConstraint>()[0].data.target = right.transform;
            TPSController.instance._rightHint.transform.localPosition = rightHint;
        }
        if (item.left != null)
        {
            TPSController.instance.HandRig.GetComponentsInChildren<TwoBoneIKConstraint>()[1].data.target = left.transform;
            TPSController.instance._leftHint.transform.localPosition = item.leftHint;
        }
        TPSController.instance._rigBuilder.Build();

    }
    public void SetRigDetails(Quaternion[] parent, Vector3[] position)
    {
        for (int i = 0; i < item.pos_offset.Length; i++)
        {
            print(parent[i]);
            TPSController.instance.position_aiming_shooting_Parent[i].gameObject.transform.localRotation = parent[i]; // BURAYI AYARLA
            print(TPSController.instance.position_aiming_shooting_Parent[i].gameObject.name);
            TPSController.instance.position_aiming_shooting_Pos[i].data.offset = position[i];      // Assignments for Rigs
        }
        TPSController.instance._rigBuilder.Build();

    }
    public void AddItems() // RIG TRANSFORM EKLE
    {
        if (Physics.Raycast(TPSController.instance.debugTransform.position, TPSController.instance.debugTransform.forward, out hit, 20f))
        {
            if (hit.transform.CompareTag("Weapon"))
            {
                item = hit.transform.GetComponent<WeaponComponent>();
                if (item == null) { print("null"); }

                if (item.right != null)
                {
                    TPSController.instance.HandRig.GetComponentsInChildren<TwoBoneIKConstraint>()[0].data.target = item.right.transform;
                    TPSController.instance._rightHint.transform.localPosition = item.rightHint; 
                }
                if (item.left != null)
                {
                    TPSController.instance.HandRig.GetComponentsInChildren<TwoBoneIKConstraint>()[1].data.target = item.left.transform;
                    TPSController.instance._leftHint.transform.localPosition = item.leftHint;
                }

                TPSController.instance.spawnBulletTransform = item.bulletTransform;

                All_Items[current_item.id].image_2d = item.info.image_2d;
                All_Items[current_item.id].name = item.info.name;
                All_Items[current_item.id].the_item = item.gameObject;
                All_Items[current_item.id].isAnimationRigging = item.info.isAnimationRigging;
                All_Items[current_item.id].parent_Pos = item.info.parent_Pos;
                All_Items[current_item.id].parent_Rot = item.info.parent_Rot;
                All_Items[current_item.id].pos_offset = item.pos_offset;
                All_Items[current_item.id].weapon_position = item.weapon_position;
                All_Items[current_item.id].weapon_rotation   = item.weapon_rotation;
                All_Items[current_item.id].left = item.left;
                All_Items[current_item.id].right = item.right;
                All_Items[current_item.id].leftHint = item.leftHint;
                All_Items[current_item.id].rightHint = item.rightHint;


                slot_Images[current_item.id].GetComponent<Image>().sprite = All_Items[current_item.id].image_2d;
                current_item = All_Items[current_item.id];

                item.transform.parent = wepParent.transform;


                item.transform.localPosition = item.info.item_Pos;  // Setting Transforms BURAYI AYARLA
                item.transform.localRotation = item.info.item_Rot;
                wepParent.transform.position = item.info.parent_Pos;
                wepParent.transform.rotation = item.info.parent_Rot;
                item.transform.parent = wepParent.transform;



                item.transform.AddComponent<RigTransform>();     // Adding RigTransform for Animation Rigging
                if (item.info.isAnimationRigging)
                {
                    item.transform.GetComponent<Rigidbody>().useGravity = false;
                    item.transform.GetComponent<Collider>().enabled = false;

                    SetRigDetails(item.weapon_rotation,item.pos_offset);

                    TPSController.instance.aiming = item.aim;
                    TPSController.instance.PoseRig.weight = 1f;
                    TPSController.instance.HandRig.weight = 1f;
                    TPSController.instance.canShoot = true;
                }
            }

        }
    }
    public void Show()
    {
        if (!isOpen)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            inventory_Panel.SetActive(true);
            isOpen = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            inventory_Panel.SetActive(false);
            isOpen = false;
        }
    }
}
public class SlotItemInfos
{
    public string name;
    public int id;
    public bool isAnimationRigging;
    public Sprite image_2d;
    public GameObject the_item;
    public Vector3 parent_Pos;
    public Quaternion parent_Rot;
    public Vector3[] pos_offset;
    public Quaternion[] weapon_rotation;
    public Vector3[] weapon_position;
    public GameObject right;
    public GameObject left;
    public Vector3 leftHint;
    public Vector3 rightHint;
    public Vector3 item_Pos;
    public Vector3 item_Rot;

    public SlotItemInfos(int index)
    {
        name = "Slot" + (index + 1);
        id = index;
    }
}
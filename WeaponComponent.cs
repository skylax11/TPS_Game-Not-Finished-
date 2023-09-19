using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UI;
using static UnityEditor.Progress;

namespace Assets.Scripts
{
    public class WeaponComponent : MonoBehaviour, IWeapon , IInteractable
    {
        [Header("Item Info")]
        [SerializeField] public ItemInfo info;   // SCRIPTABLE OBJECT

        [SerializeField]
        public Vector3[] weapon_position;
        [SerializeField]
        public Quaternion[] weapon_rotation;
        [SerializeField]
        public Vector3[] pos_offset;
        [SerializeField]
        public MultiAimConstraint aim;
        [SerializeField] 
        public GameObject left;
        [SerializeField] 
        public GameObject right;
        [SerializeField]
        public Transform bulletTransform;

        [SerializeField] int currentAmmo;
        [SerializeField] int ammo;
        [SerializeField] int magazineAmmo;
        [SerializeField] int damage;
        [SerializeField] float fireFreq;
        [Header("Animation")]
        [SerializeField]
        public Vector3 weapon_reloadPosition;
        [SerializeField]
        public Quaternion weapon_reloadRotation;
        public int _currentAmmo
        {
            get { return currentAmmo; }
            set { currentAmmo = value; } // allows to set the parameter ( for instance in TPSController script the Inventory.instance.current_item.weapon_data._currentAmmo--; command)
        }
        public int _ammo
        {
            get { return ammo; }
            set { ammo = value; }
        }
        public int _magazineAmmo
        {
            get { return magazineAmmo; }
            set { magazineAmmo = value; }
        }
        public int _damage
        {
            get { return  damage; }
            set { damage = value; }
        }
        public float _fireFreq
        {
            get { return  fireFreq; }
            set { }
        }
        #region    ADDING & SETTING ITEMS 
        public void AddItem(SlotItemInfos[] All_Items,WeaponComponent item, int id)
        {
            if (item == null) { print("null"); }

            Inventory.instance.current_item.weapon_data = item.GetComponent<IWeapon>();

            if (item.right != null)
            {
                TPSController.instance.HandRig.GetComponentsInChildren<TwoBoneIKConstraint>()[0].data.target = item.right.transform;
                TPSController.instance._rightHint.transform.localPosition = item.info.rightHint;
            }
            if (item.left != null)
            {
                TPSController.instance.HandRig.GetComponentsInChildren<TwoBoneIKConstraint>()[1].data.target = item.left.transform;
                TPSController.instance._leftHint.transform.localPosition = item.info.leftHint;
            }

            TPSController.instance.spawnBulletTransform = item.bulletTransform;

            All_Items[id].image_2d = item.info.image_2d;
            All_Items[id].name = item.info.name;
            All_Items[id].the_item = item.gameObject;
            All_Items[id].isAnimationRigging = item.info.isAnimationRigging;
            All_Items[id].parent_Pos = item.info.parent_Pos;
            All_Items[id].parent_Rot = item.info.parent_Rot;
            All_Items[id].pos_offset = item.pos_offset;
            All_Items[id].weapon_position = item.weapon_position;
            All_Items[id].weapon_rotation = item.weapon_rotation;
            All_Items[id].left = item.left;
            All_Items[id].right = item.right;
            All_Items[id].leftHint = item.info.leftHint;
            All_Items[id].rightHint = item.info.rightHint;


            Inventory.instance.slot_Images[id].GetComponent<Image>().sprite = All_Items[id].image_2d;
            Inventory.instance.current_item = All_Items[id];

            item.transform.parent = Inventory.instance.wepParent.transform;


            item.transform.localPosition = item.info.item_Pos;  // Setting Transforms BURAYI AYARLA
            item.transform.localRotation = item.info.item_Rot;
            Inventory.instance.wepParent.transform.position = item.info.parent_Pos;
            Inventory.instance.wepParent.transform.rotation = item.info.parent_Rot;
            item.transform.parent = Inventory.instance.wepParent.transform;



            item.transform.AddComponent<RigTransform>();     // Adding RigTransform for Animation Rigging
            if (item.info.isAnimationRigging)
            {
                item.transform.GetComponent<Rigidbody>().useGravity = false;
                item.transform.GetComponent<Collider>().enabled = false;

                Inventory.instance.SetRigDetails(item.weapon_rotation, item.pos_offset);

                TPSController.instance.aiming = item.aim;
                TPSController.instance.PoseRig.weight = 1f;
                TPSController.instance.HandRig.weight = 1f;
                TPSController.instance.canShoot = true;
            }
        }
        #endregion
        public void swapWeapons()
        {
            TPSController.instance.fireFreq = Inventory.instance.current_item.the_item.GetComponent<IWeapon>()._fireFreq;
            TPSController.instance._maganize = Inventory.instance.current_item.the_item.GetComponent<IWeapon>()._magazineAmmo;
            TPSController.instance._ammo = Inventory.instance.current_item.the_item.GetComponent<IWeapon>()._ammo;
            TPSController.instance.spawnBulletTransform = Inventory.instance.current_item.the_item.GetComponent<WeaponComponent>().bulletTransform;
            
            UI_Manager.instance.reflectAmmo();

        }
        public void Reload(IWeapon data)
        {
            Inventory.instance.current_item.the_item.GetComponent<Animator>().SetBool("reload", true);

            int diff = data._ammo - data._currentAmmo;

            print(data._magazineAmmo);
            print(diff);

            data._currentAmmo = data._magazineAmmo > diff ? data._ammo : data._currentAmmo+data._magazineAmmo;
            data._magazineAmmo = data._magazineAmmo > diff ? data._magazineAmmo-diff : 0 ;
            UI_Manager.instance.reflectAmmo();

        }
        public void SetReloadAnimation_False()
        {
            Inventory.instance.current_item.the_item.GetComponent<Animator>().SetBool("reload", false);
        }
    }
}

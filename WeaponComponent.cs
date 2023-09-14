using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Assets.Scripts
{
    public class WeaponComponent : MonoBehaviour, IWeapon
    {
        [Header("Item Info (Scriptable Object)")]
        [SerializeField] public ItemInfo info;   // SCRIPTABLE OBJECT
        [Header("Item")]
        [SerializeField] public SlotItemInfos itemInfo;
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
        public Vector3 leftHint;
        [SerializeField]
        public Vector3 rightHint;
        [SerializeField]
        public Transform bulletTransform;

        public void Awake()
        {
            info.the_item = gameObject;
        }

    }
}

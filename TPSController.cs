using Assets.Scripts;
using Cinemachine;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class TPSController : MonoBehaviour
{
    [SerializeField] public CinemachineVirtualCamera aimVirtualCam;
    [SerializeField] public CinemachineVirtualCamera ShootVirtualCam;
    [SerializeField] private float aimSens;
    [SerializeField] private float normalSens;
    private ThirdPersonController tps;
    private StarterAssetsInputs starterAssetsInputs;
    [SerializeField] LayerMask aimColliderMask = new LayerMask();
    [SerializeField] public Transform debugTransform;
    [SerializeField] public Transform pjBulletTransform;
    [SerializeField] public Transform spawnBulletTransform;
    [Header("Rigging")]
    [SerializeField] public RigBuilder _rigBuilder;
    [SerializeField] public Rig aimRig;
    [SerializeField] public Rig ShootingRig;
    [SerializeField] public Rig PoseRig;
    [SerializeField] public Rig HandRig;
    [SerializeField] public TwoBoneIKConstraint[] position_aiming_shooting_Bone;
    [SerializeField] public MultiParentConstraint[] position_aiming_shooting_Parent;
    [SerializeField] public MultiAimConstraint aiming;
    [SerializeField] public MultiPositionConstraint[] position_aiming_shooting_Pos;
    [SerializeField] public GameObject _leftHint;
    [SerializeField] public GameObject _rightHint;


    [Header("Weapon")]
    [SerializeField] public float fireFreq = 0.15f;
    [SerializeField] public int _ammo;
    [SerializeField] public int _maganize;

    public float fireCounter;
    public bool canShoot;
    [Header("Singleton")]
    public static TPSController instance;
    [Header("Object Pool")]
    public Queue bullets;
    public bool didReachMax = false;

    private void Awake()
    {
        bullets = new Queue();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        canShoot = false;

        if (instance == null)
        {
            instance = this;
        }
        else { return; }
        tps = GetComponent<ThirdPersonController>();
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
    }
    private void Update()
    {
        SlotItemInfos _currentItem = Inventory.instance.current_item; 

        Vector3 mouseWorldPos = Vector3.zero;

        Vector2 centerPoint = new Vector2(Screen.width / 2f , Screen.height / 2f);

        Ray ray = Camera.main.ScreenPointToRay(centerPoint);
        
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 500f, aimColliderMask))
        {
            debugTransform.position = raycastHit.point;
            mouseWorldPos = raycastHit.point;
        }
        if (starterAssetsInputs.aim && canShoot)
        {
            aimVirtualCam.gameObject.SetActive(true); tps.SetSens(aimSens);
            tps.setRotateMove(false);

            PoseRig.weight += Time.deltaTime*20f;
            aimRig.weight += Time.deltaTime * 10f;


            Vector3 worldAimTarget = mouseWorldPos;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;
            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
        }
        else
        {
            aimVirtualCam.gameObject.SetActive(false); tps.SetSens(normalSens);
            tps.setRotateMove(true);
            if (canShoot)
            {
                aimRig.weight -= Time.deltaTime * 10f;
                PoseRig.weight += Time.deltaTime * 20f;
            }

        }

        if ((starterAssetsInputs.shoot && canShoot) && _currentItem.weapon_data._currentAmmo > 0)
        {
            ShootVirtualCam.gameObject.SetActive(true);
            Vector3 worldAimTarget = mouseWorldPos;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;
            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);

            while (ShootingRig.weight <= 1 && !starterAssetsInputs.aim)
            {
                PoseRig.weight -= Time.deltaTime;
                ShootingRig.weight += Time.deltaTime * 10f;
                if(ShootingRig.weight ==1) { break; }
            }

            _currentItem.the_item.GetComponent<IWeapon>().Shot(_currentItem.the_item.GetComponent<IWeapon>(),mouseWorldPos,spawnBulletTransform);
        }
        else
        {
            ShootVirtualCam.gameObject.SetActive(false);
            if (canShoot)
            {
                ShootingRig.weight -= Time.deltaTime * 3f;
                PoseRig.weight += Time.deltaTime * 5f;
            }
        }

    }
}

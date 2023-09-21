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
    public CinemachineVirtualCamera AimVirtualCam;
    public CinemachineVirtualCamera ShootVirtualCam;
    [SerializeField] private float aimSens;
    [SerializeField] private float normalSens;
    private ThirdPersonController tps;
    private StarterAssetsInputs starterAssetsInputs;
    [SerializeField] LayerMask aimColliderMask = new LayerMask();
    public Transform DebugTransform;
    public Transform PjBulletTransform;
    public Transform SpawnBulletTransform;
    [Header("Rigging")]
    public RigBuilder _rigBuilder;
    public Rig AimRig;
    public Rig ShootingRig;
    public Rig PoseRig;
    public Rig HandRig;
    public TwoBoneIKConstraint[] Position_aiming_shooting_Bone;
    public MultiParentConstraint[] Position_aiming_shooting_Parent;
    public MultiAimConstraint Aiming;
    public MultiPositionConstraint[] Position_aiming_shooting_Pos;
    public GameObject LeftHint;
    public GameObject RightHint;


    [Header("Weapon")]
    public float FireFreq = 0.15f;
    public int Ammo;
    public int Maganize;

    public float FireCounter;
    public bool CanShoot;
    [Header("Singleton")]
    public static TPSController instance;
    [Header("Object Pool")]
    public Queue Bullets;
    public bool DidReachMax = false;

    private void Awake()
    {
        Bullets = new Queue();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        CanShoot = false;

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
            DebugTransform.position = raycastHit.point;
            mouseWorldPos = raycastHit.point;
        }
        if (starterAssetsInputs.aim && CanShoot)
        {
            AimVirtualCam.gameObject.SetActive(true); tps.SetSens(aimSens);
            tps.setRotateMove(false);

            PoseRig.weight += Time.deltaTime*20f;
            AimRig.weight += Time.deltaTime * 10f;


            Vector3 worldAimTarget = mouseWorldPos;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;
            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
        }
        else
        {
            AimVirtualCam.gameObject.SetActive(false); tps.SetSens(normalSens);
            tps.setRotateMove(true);
            if (CanShoot)
            {
                AimRig.weight -= Time.deltaTime * 10f;
                PoseRig.weight += Time.deltaTime * 20f;
            }

        }

        if ((starterAssetsInputs.shoot && CanShoot) && _currentItem.weapon_data._currentAmmo > 0)
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

            _currentItem.the_item.GetComponent<IWeapon>().Shot(_currentItem.the_item.GetComponent<IWeapon>(),mouseWorldPos,SpawnBulletTransform);
        }
        else
        {
            ShootVirtualCam.gameObject.SetActive(false);
            if (CanShoot)
            {
                ShootingRig.weight -= Time.deltaTime * 3f;
                PoseRig.weight += Time.deltaTime * 5f;
            }
            if (ShootingRig.weight == 0)
            {
                starterAssetsInputs.reload = true;
            }
        }

    }
}

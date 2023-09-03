using Cinemachine;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class TPSController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera aimVirtualCam;
    [SerializeField] private CinemachineVirtualCamera ShootVirtualCam;
    [SerializeField] private float aimSens;
    [SerializeField] private float normalSens;
    private ThirdPersonController tps;
    private StarterAssetsInputs starterAssetsInputs;
    [SerializeField] LayerMask aimColliderMask = new LayerMask();
    [SerializeField] private Transform debugTransform;
    [SerializeField] private Transform pjBulletTransform;
    [SerializeField] private Transform spawnBulletTransform;
    [Header("Rigging")]
    [SerializeField] Rig aimRig;
    [SerializeField] Rig ShootingRig;
    [SerializeField] Rig PoseRig;
    [Header("Weapon")]
    [SerializeField] float fireFreq = 0.15f;
    float fireCounter;
    [Header("Singleton")]
    public static TPSController instance;

    private void Awake()
    {
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
        Vector3 mouseWorldPos = Vector3.zero;

        Vector2 centerPoint = new Vector2(Screen.width / 2f , Screen.height / 2f);

        Ray ray = Camera.main.ScreenPointToRay(centerPoint);

        if (Physics.Raycast(ray, out RaycastHit raycastHit, 500f, aimColliderMask))
        {
            debugTransform.position = raycastHit.point;
            mouseWorldPos = raycastHit.point;
        }
        if (starterAssetsInputs.aim)
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
            aimRig.weight -= Time.deltaTime*10f;
            PoseRig.weight += Time.deltaTime*20f;

        }
        if (starterAssetsInputs.shoot)
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
            Vector3 aimDir = (mouseWorldPos - spawnBulletTransform.position).normalized;
            if (Time.time > fireCounter)
            {
                Instantiate(pjBulletTransform, spawnBulletTransform.position, Quaternion.LookRotation(aimDir, Vector3.up));
                fireCounter = fireFreq + Time.time;
            }
        }
        else
        {
            ShootVirtualCam.gameObject.SetActive(false);
            ShootingRig.weight -= Time.deltaTime * 3f;
            PoseRig.weight += Time.deltaTime*5f;
        }

    }
}
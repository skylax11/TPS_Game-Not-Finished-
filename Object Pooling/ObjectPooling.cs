using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.ObjectPooling
{
    public class ObjectPooling : MonoBehaviour
    {
        public static ObjectPooling instance;

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
        public void do_ObjectPooling(Queue bullets, Vector3 mouseWorldPos, Transform spawnBulletTransform)
        {
            TPSController _tps = TPSController.instance;
            Vector3 aimDir = (mouseWorldPos - spawnBulletTransform.position).normalized;

            if (bullets.Count < 30 && !_tps.DidReachMax)
            {
                Transform bullet = Instantiate(_tps.PjBulletTransform, _tps.SpawnBulletTransform.position, Quaternion.LookRotation(aimDir, Vector3.up));
                bullets.Enqueue(bullet);
            }
            else
            {
                _tps.DidReachMax = true;
                Transform _bullet = bullets.Peek() as Transform;
                if (_bullet.gameObject.active == false)
                {
                    _bullet.transform.rotation = Quaternion.LookRotation(aimDir, Vector3.up);
                    _bullet.transform.position = _tps.SpawnBulletTransform.position;
                    _bullet.transform.GetComponent<BulletProjectile>().resetVelo();
                    _bullet.gameObject.SetActive(true);
                }
                else
                {
                    do
                    {
                        Transform _oldBullet = bullets.Dequeue() as Transform;
                        _bullet = bullets.Peek() as Transform;

                        _oldBullet.gameObject.SetActive(false);
                        bullets.Enqueue(_oldBullet);

                        if (_bullet.gameObject.active == false)
                        {
                            _bullet.transform.rotation = Quaternion.LookRotation(aimDir, Vector3.up);
                            _bullet.transform.position = _tps.SpawnBulletTransform.position;
                            _bullet.transform.GetComponent<BulletProjectile>().resetVelo();
                            _bullet.gameObject.SetActive(true);
                            break;
                        }
                    }
                    while (_bullet.gameObject.active == true);
                }
            }
        }
    }
}

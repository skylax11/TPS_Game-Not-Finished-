using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Assets.Scripts
{
    public interface IWeapon
    {
        public int _currentAmmo { get; set;}
        public int _ammo { get; set; }
        public int _magazineAmmo { get; set; }
        public int _damage { get; set; }
        public float _fireFreq { get; set; }
        public void swapWeapons();
        public void Reload(IWeapon data);
        public void Shot(IWeapon data, Vector3 mouseWorldPos, Transform spawnBulletTransform);

    }
}

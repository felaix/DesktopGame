using UnityEngine;
using System.Threading.Tasks;
using EditorAttributes;

namespace TDS
{
    public class Gun : MonoBehaviour
    {
        [SerializeField] private GameObject bulletPrefab;

        [ReadOnly][SerializeField] private Bullet _currentBullet;
        [ReadOnly][SerializeField] private GunType _currentBulletType = GunType.Normal;

        [ReadOnly][SerializeField] private bool _canShoot;

        [ReadOnly] public float GunSpeed = .2f;
        [ReadOnly] public float _delay;

        private void OnDisable()
        {
            Destroy(gameObject);
        }

        public void UpgradeGun()
        {
            _currentBulletType = GunType.Shotgun;
        }

        private void Update()
        {
            if (!_canShoot)
            {
                _delay += Time.deltaTime;
                if (_delay >= GunSpeed) { _canShoot = true; _delay = 0; }
            }
        }

        public void ShootBullet(Vector2 direction, float spd, int numOfBullets = 1)
        {
            if (_canShoot)
            {

                _canShoot = false;

                for (int i = 0; i < numOfBullets; i++)
                {
                    // Spawn a bullet
                    _currentBullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity).GetComponent<Bullet>();
                    _currentBullet.Initialize(direction, spd, _currentBulletType);

                    // Delay 
                    //await Task.Delay((int)spd * 100);
                }
            }
        }
        
    }
}

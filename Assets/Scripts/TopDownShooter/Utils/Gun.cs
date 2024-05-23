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


        private void OnDisable()
        {
            Destroy(gameObject);
        }

        public void UpgradeGun()
        {
            _currentBulletType = GunType.Shotgun;
        }

        public async void ShootBullet(Vector2 direction, float spd, int numOfBullets = 1)
        {
            for (int i = 0; i < numOfBullets; i++)
            {
                // Spawn a bullet
                _currentBullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity).GetComponent<Bullet>();
                _currentBullet.Initialize(direction, spd, _currentBulletType);

                // Delay 
                await Task.Delay((int)spd * 1000);
            }
        }
        
    }
}

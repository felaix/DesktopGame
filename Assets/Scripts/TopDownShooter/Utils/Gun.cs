using UnityEngine;
using System.Threading.Tasks;
using EditorAttributes;
using UnityEngine.UI;

namespace TDS
{
    public class Gun : MonoBehaviour
    {
        [SerializeField] private GameObject _bulletPrefab;
        [SerializeField] private Slider _delaySlider;

        [ReadOnly][SerializeField] private Bullet _currentBullet;
        [ReadOnly][SerializeField] private GunType _currentBulletType = GunType.Normal;

        [ReadOnly][SerializeField] private bool _canShoot;

        public float GunSpeed = .4f;
        [ReadOnly] public float _delay;

        private void OnDisable()
        {
            Destroy(gameObject);
        }

        public void ModifyGunSpeed(float spd)
        {
            GunSpeed = spd;
            ModifySliderValues(_delay, spd);
        }

        public void ModifySliderValues(float currentValue, float maxValue)
        {
            _delaySlider.maxValue = maxValue;
            _delaySlider.value = currentValue;
        }

        public void UpgradeGun()
        {
            //_currentBulletType = GunType.Shotgun;
            ModifyGunSpeed(GunSpeed / 1.3f);
        }

        private void Update()
        {
            if (!_canShoot)
            {
                ModifySliderValues(_delay, GunSpeed);
                _delay += Time.deltaTime;
                if (_delay >= GunSpeed) { _canShoot = true; _delay = 0; }
            }
        }

        public void ShootBullet(Vector2 direction, float spd, int numOfBullets = 1)
        {
            if (_canShoot)
            {

                for (int i = 0; i < numOfBullets; i++)
                {
                    Debug.Log("Instantiating bullet " + i);

                    // Spawn a bullet
                    _currentBullet = Instantiate(_bulletPrefab, transform.position, Quaternion.identity).GetComponent<Bullet>();
                    _currentBullet.Initialize(direction, spd, _currentBulletType);

                    // Delay 
                }

                _canShoot = false;

            }
            else
            {
                CanvasManager.Instance.CreatePlayerTMP("Gun reloading ...");
            }
        }
        
    }
}

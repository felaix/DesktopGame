using UnityEngine;
using DG.Tweening;

namespace TDS
{
    public class Gun : MonoBehaviour
    {
        [SerializeField] private Bullet bullet;

        public void SetBullet(Bullet blt) { bullet = blt; }

        public void ShootBullet(Vector2 direction, float spd)
        {
            Bullet bulletInstance = Instantiate(bullet, transform.position, Quaternion.identity);
            bulletInstance.Initialize(direction, spd);
        }

    }

}

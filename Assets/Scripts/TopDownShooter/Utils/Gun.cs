using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;
using UnityEngine.UIElements;

namespace TDS
{
    public class Gun : MonoBehaviour
    {
        [SerializeField] private Bullet bullet;

        public void SetBullet(Bullet blt) { bullet = blt; }

        public async void ShootBullet(Vector2 direction, float spd, int numOfBullets = 1)
        {
            for (int i = 0; i < numOfBullets; i++)
            {
                Bullet bulletInstance = Instantiate(bullet, transform.position, Quaternion.identity);
                float randomOffsetY = 0f;
                float randomOffsetX = 0f;

                if (direction.y < direction.x) randomOffsetY = Random.Range(-1, direction.magnitude);
                if (direction.y > direction.x) randomOffsetX = Random.Range(-1, direction.magnitude);

                Vector2 randomOffset = new Vector2(randomOffsetX, randomOffsetY);
                bulletInstance.Initialize(direction + randomOffset, spd);
                await Task.Delay(10);
            }
        }

    }

}

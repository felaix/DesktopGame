using EditorAttributes;
using System;
using UnityEngine;

namespace TDS
{
    [RequireComponent(typeof(Collider2D))]
    [Serializable]
    public class Bullet : MonoBehaviour
    {
        [ReadOnly][SerializeField] public GunType Type;
        [ReadOnly] [SerializeField] private Vector2 _direction = new Vector2(1, 0);
        [ReadOnly][SerializeField] private float _speed;

        public void Initialize(Vector2 dir, float spd, GunType type = GunType.Normal)
        {
            _direction = dir;
            _speed = spd;
            Type = type;
        }

        void Update()
        {
            Move(_direction * _speed);
            Rotate(_direction * _speed);
        }

        public void Move(Vector3 direction)
        {
            if (Type.Equals(GunType.Normal))
            {
                transform.position += direction * Time.deltaTime;
                Debug.Log("Move Direction: " + direction);
            }
            if (Type.Equals(GunType.Shotgun))
            {
                transform.position += GetRandomOffset(direction) * Time.deltaTime;

            }
        }

        public Vector3 GetRandomOffset(Vector3 dir)
        {
            float randomAngle = UnityEngine.Random.Range(-30f, 30f);
            return new Vector2(dir.x, dir.y + randomAngle);
        }

        public void ManipulateDirection(Vector2 dir)
        {
            _direction = dir;
        }

        private void Rotate(Vector2 direction)
        {

            if (Type.Equals(GunType.Normal))
            {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                //Debug.Log("Angle: " + angle);
                transform.rotation = Quaternion.Euler(0f, 0f, angle);
            }

            if (Type.Equals(GunType.Shotgun))
            {
                float randomMultiplier = UnityEngine.Random.Range(0f, 4f);
                float angle = Mathf.Atan2(direction.y * randomMultiplier, direction.x * randomMultiplier) * Mathf.Rad2Deg;

                //Debug.Log("Angle: " + angle);
                transform.rotation = Quaternion.Euler(0f, 0f, angle);

            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Enemy enemy = collision.transform.GetComponent<Enemy>();
            if (enemy != null) Destroy(this.gameObject);

            else if (collision.gameObject.CompareTag("Player")) return;
            
            Destroy(this.gameObject);
        }

    }

    public enum GunType
    {
        Normal,
        Shotgun,
    }

}

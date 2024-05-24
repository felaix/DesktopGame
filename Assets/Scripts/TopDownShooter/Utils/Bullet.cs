using EditorAttributes;
using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace TDS
{
    [RequireComponent(typeof(Collider2D))]
    [Serializable]
    public class Bullet : MonoBehaviour
    {
        [ReadOnly][SerializeField] public GunType Type;
        [ReadOnly] [SerializeField] private Vector2 _direction = new Vector2(1, 0);
        [ReadOnly][SerializeField] private float _speed;

        private bool _manipulated = false;

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

            //if (Type.Equals(GunType.Shotgun))
            //{
            //    ManipulateDirection(GetRandomOffset(direction));
            //}

            transform.position += GetRandomOffset(direction) * Time.deltaTime;
        }

        public Vector3 GetRandomOffset(Vector2 dir)
        {
            //dir.Normalize();
            float randomAngle = UnityEngine.Random.Range(-_speed, _speed);
            return new Vector2(dir.x + randomAngle, dir.y + randomAngle);
        }

        public void ManipulateDirection(Vector2 dir)
        {
            if (_manipulated) return; 
            _direction = dir; 
            _manipulated = true;
        }

        private void Rotate(Vector2 direction)
        {

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);

            //if (Type.Equals(GunType.Normal))
            //{
            //    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            //    //Debug.Log("Angle: " + angle);
            //    transform.rotation = Quaternion.Euler(0f, 0f, angle);
            //}

            //if (Type.Equals(GunType.Shotgun))
            //{
            //    float randomMultiplier = UnityEngine.Random.Range(0f, 4f);
            //    float angle = Mathf.Atan2(direction.y * randomMultiplier, direction.x * randomMultiplier) * Mathf.Rad2Deg;

            //    //Debug.Log("Angle: " + angle);
            //    transform.rotation = Quaternion.Euler(0f, 0f, angle);

            //}
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            Enemy enemy = collision.transform.GetComponent<Enemy>();
            if (enemy != null) Destroy(this.gameObject);
            else if (collision.gameObject.CompareTag("Wall")) Destroy(this.gameObject);
            else if (collision.gameObject.CompareTag("Player")) return;

            //Destroy(this.gameObject);
        }

    }

    public enum GunType
    {
        Normal,
        Shotgun,
    }

}

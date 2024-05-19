using UnityEngine;

namespace TDS
{
    public class Bullet : MonoBehaviour
    {

        private Vector2 direction = new Vector2(1, 0);
        [SerializeField] private float speed = 5f;

        public void Initialize(Vector2 dir, float spd)
        {
            direction = dir;
            speed = spd;
        }

        void Update()
        {
            Move(direction * speed);
            Rotate(direction * speed);
        }

        public void Move(Vector3 targetPos)
        {
            transform.position += targetPos * Time.deltaTime;
        }

        private void Rotate(Vector2 direction)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Destroy(this.gameObject);
        }

    }

}

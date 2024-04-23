using UnityEngine;
using UnityEngine.InputSystem;

namespace TDS
{
    public class Player : MonoBehaviour
    {
        private Stats stats;
        private TDS_Controls input;
        private Rigidbody2D rb;
        private Gun gun;

        private Vector2 direction;

        private void Awake()
        {
            if (stats == null)
            {
                stats = new Stats();
                stats.Initialize(10, 5f, 20);
            }

            if (input == null)
            {
                input = new TDS_Controls();
                input.Enable();
            }

        }

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            gun = GetComponent<Gun>();

            if (gun == null) Debugger.Instance.CreateWarningLog("Gun is missing");
            if (rb == null) Debugger.Instance.CreateWarningLog("Rigidbody2D is missing");
        }

        private void OnEnable()
        {
            SubscribeControls();
        }

        private void OnDisable()
        {
            UnsubscribeControls();
        }

        #region Controls
        private void SubscribeControls()
        {
            input.Player.Move.performed += Move;
            input.Player.Fire.performed += Fire;
            input.Player.Move.canceled += StopMoving;

            input.Player.Fire.canceled -= Fire;
        }
        private void UnsubscribeControls()
        {
            input.Player.Move.performed -= Move;
            input.Player.Fire.performed -= Fire;

            input.Player.Move.canceled -= StopMoving;
            input.Player.Fire.canceled -= Fire;
        }

        #endregion

        #region Movement

        private void StopMoving(InputAction.CallbackContext context)
        {
            Debugger.Instance.CreateLog("Stop Move");
            rb.velocity = Vector2.zero;
        }

        private void Fire(InputAction.CallbackContext context)
        {
            gun.ShootBullet(direction, stats.AttackSpeed);
        }   

        private void Move(InputAction.CallbackContext context)
        {
            direction = context.ReadValue<Vector2>();

            rb.velocity = direction * stats.Speed;

            Rotate(direction);
        }

        #endregion

        #region Rotation

        private void Rotate(Vector2 direction)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        #endregion

    }
}

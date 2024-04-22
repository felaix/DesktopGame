using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TDS
{
    public class Player : MonoBehaviour
    {
        private Stats stats;
        private TDS_Controls input;
        private Rigidbody2D rb;

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
            Debugger.Instance.CreateLog("Fire");
        }   

        private void Move(InputAction.CallbackContext context)
        {
            Debugger.Instance.CreateLog("Move");

            Vector2 direction = context.ReadValue<Vector2>();

            rb.velocity = direction * stats.Speed;
            transform.LookAt(direction);
        }

        #endregion

        #region Rotation


        #endregion

    }
}

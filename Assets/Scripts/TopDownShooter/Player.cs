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
        private Animator anim;

        private Vector2 direction;

        private void Awake()
        {
            if (stats == null)
            {
                stats = new Stats();
                stats.Initialize(3, 5f, 20);
            }

            if (input == null)
            {
                input = new TDS_Controls();
                input.Enable();
            }

        }

        public void IncreaseSpeed(float amount) => stats.Speed += amount;
        public void DecreaseSpeed(float amount) => stats.Speed -= amount;




        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            gun = GetComponent<Gun>();
            anim = GetComponent<Animator>();

            if (anim == null) Debugger.Instance.CreateWarningLog("Animator is missing");
            if (gun == null) Debugger.Instance.CreateWarningLog("Gun is missing");
            if (rb == null) Debugger.Instance.CreateWarningLog("Rigidbody2D is missing");
        }

        private void OnEnable()
        {
            SubscribeControls();
            Heal(3);
        }

        private void OnDisable()
        {
            UnsubscribeControls();
        }

        public void Heal(int amount)
        {
            stats.HP += amount;
            CanvasManager.Instance.UpdatePlayerHP(stats.HP);
        }

        public void DealDamage(int damage)
        {

            Debug.Log(stats.HP + " " + stats);

            stats.HP -= damage;
            CanvasManager.Instance.UpdatePlayerHP(stats.HP);

            if (stats.HP < 0) 
            { 
                stats.HP = 0;
                input.Disable();
            }
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


        private void Fire(InputAction.CallbackContext context)
        {
            gun.ShootBullet(direction, stats.AttackSpeed);
        }

        #region Movement

        private void StopMoving(InputAction.CallbackContext context)
        {
            Debugger.Instance.CreateLog("Stop Move");
            anim.SetBool("Idle", true);
            rb.velocity = Vector2.zero;
        }

        private void Move(InputAction.CallbackContext context)
        {
            Vector2 inputDirection = context.ReadValue<Vector2>();
            //Vector2 horizontalMovement = new Vector2(inputDirection.x, 0f).normalized;
            //Vector2 verticalMovement = new Vector2(0f, inputDirection.y).normalized;

            Vector2 movement = inputDirection.normalized;

            if (movement.magnitude > 0f)
            {
                anim.SetBool("Idle", false);

                rb.velocity = movement * stats.Speed;
                direction = movement;
            }
            //else if (verticalMovement.magnitude > 0f)
            //{
            //    rb.velocity = verticalMovement * stats.Speed;
            //    direction = verticalMovement;
            //}
            else
            {
                Debug.Log("Idle");
                anim.SetBool("Idle", true);
                rb.velocity = Vector2.zero;
                direction = Vector2.zero;
            }

            Rotate(direction);
        }


        #endregion

        #region Rotation

        private void Rotate(Vector2 direction)
        {

            Vector3 scaleDirection = direction;
            scaleDirection.y = 1f;
            scaleDirection.z = 1f;
            if (scaleDirection.x == 0f) scaleDirection.x = 1f; 
            //if (scaleDirection.y == 0f) scaleDirection.y = 1f; 
            //if (scaleDirection.z == 0f) scaleDirection.z = 1f;

            if (direction.x < 0f) anim.Play("Player_Walk_R"); 
            if (direction.x > 0f) anim.Play("Player_Walk_R");
            
            if (direction.y < 0f) anim.Play("Player_Walk_Down");
            if (direction.y > 0f) anim.Play("Player_Walk_Up");

            transform.localScale = scaleDirection;
        }

        #endregion

    }
}

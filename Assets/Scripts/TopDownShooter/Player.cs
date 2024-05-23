using UnityEngine;
using UnityEngine.InputSystem;

namespace TDS
{
    public class Player : MonoBehaviour
    {
        private Stats _stats;
        private TDS_Controls _input;
        private Rigidbody2D _rb;
        private Gun _gun;
        private Animator _anim;

        private Vector3 _initialPosition;
        private Vector2 _direction;

        private void Awake()
        {
            CreateStats();
            CreateInput();
        }

        public void IncreaseSpeed(float amount) => _stats.Speed += amount;
        public void DecreaseSpeed(float amount) => _stats.Speed -= amount;
        public void IncreaseMaxHP(int amount) => _stats.MaxHP += amount;
        public void IncreaseHP(int amount) {_stats.HP += amount; if (_stats.HP >= _stats.MaxHP) _stats.HP = _stats.MaxHP; }
        public void IncreaseBullets(int amount) { _stats.NumOfBullets += amount; _gun.UpgradeGun(); }
        private void CreateStats()
        {
            if (_stats == null)
            {
                _stats = new Stats();
                _stats.Initialize(3, 5, 10f, 3);
            }
        }

        private void CreateInput()
        {
            if (_input == null)
            {
                _input = new TDS_Controls();
                _input.Enable();
            }
        }

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _gun = GetComponent<Gun>();
            _anim = GetComponent<Animator>();

            CreateStats();
            CreateInput();
            SubscribeControls();

            _initialPosition = transform.position;
        }

        private void OnEnable()
        {
            //SubscribeControls();
            Heal(3);
            //transform.position = Vector2.zero;
        }

        public void ResetPosition()
        {
            transform.position = _initialPosition;
        }

        private void OnDisable()
        {
            Destroy(gameObject);
            UnsubscribeControls();
        }

        public void Heal(int amount)
        {
            IncreaseHP(amount);
            CanvasManager.Instance.UpdatePlayerHP(_stats.HP, _stats.MaxHP);
        }

        public void Shield(int amount)
        {
            _stats.HP += amount;
            CanvasManager.Instance.UpdatePlayerHP((int)_stats.HP, _stats.MaxHP);
        }

        public void DealDamage(int damage)
        {

            Debug.Log(_stats.HP + " " + _stats);

            _stats.HP -= damage;
            CanvasManager.Instance.UpdatePlayerHP(_stats.HP, _stats.MaxHP);

            if (_stats.HP < 0) 
            { 
                _stats.HP = 0;
                _input.Disable();
            }
        }

        #region Controls
        private void SubscribeControls()
        {
            _input.Player.Move.performed += Move;
            _input.Player.Fire.performed += Fire;
            _input.Player.Move.canceled += StopMoving;

            _input.Player.Fire.canceled -= Fire;
        }
        private void UnsubscribeControls()
        {
            _input.Player.Move.performed -= Move;
            _input.Player.Fire.performed -= Fire;

            _input.Player.Move.canceled -= StopMoving;
            _input.Player.Fire.canceled -= Fire;
        }

        #endregion


        private void Fire(InputAction.CallbackContext context)
        {
            _gun.ShootBullet(_direction, _stats.AttackSpeed, _stats.NumOfBullets);
        }

        #region Movement

        private void StopMoving(InputAction.CallbackContext context)
        {
            Debugger.Instance.CreateLog("Stop Move");
            _anim.SetBool("Idle", true);
            _rb.velocity = Vector2.zero;
        }

        private void Move(InputAction.CallbackContext context)
        {
            Vector2 inputDirection = context.ReadValue<Vector2>();
            //Vector2 horizontalMovement = new Vector2(inputDirection.x, 0f).normalized;
            //Vector2 verticalMovement = new Vector2(0f, inputDirection.y).normalized;

            Vector2 movement = inputDirection.normalized;

            if (movement.magnitude > 0f)
            {
                _anim.SetBool("Idle", false);

                _rb.velocity = movement * _stats.Speed;
                _direction = movement;
            }
            //else if (verticalMovement.magnitude > 0f)
            //{
            //    rb.velocity = verticalMovement * stats.Speed;
            //    direction = verticalMovement;
            //}
            else
            {
                Debug.Log("Idle");
                _anim.SetBool("Idle", true);
                _rb.velocity = Vector2.zero;
                _direction = Vector2.zero;
            }

            Rotate(_direction);
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

            if (direction.x < 0f) _anim.Play("Player_Walk_R"); 
            if (direction.x > 0f) _anim.Play("Player_Walk_R");
            
            if (direction.y < 0f) _anim.Play("Player_Walk_Down");
            if (direction.y > 0f) _anim.Play("Player_Walk_Up");

            transform.localScale = scaleDirection;
        }

        #endregion

    }
}

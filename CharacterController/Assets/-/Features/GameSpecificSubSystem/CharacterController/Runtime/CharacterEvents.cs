using UnityEngine;
using UnityEngine.InputSystem;

namespace CharacterController.Runtime
{
    public class CharacterEvents : MonoBehaviour
    {


        #region Publics
        
        //
    
        #endregion


        #region Unity API


        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void Update()
        {
            switch (_state)
            {
                case State.IDLE:
                    break;
                case State.WALK:
                    WalkingLoop();
                    break;
                case State.JUMP:
                    CheckJump();
                    break;
            }
        }

        #endregion
    


        #region Main Methods

        public void OnMove(InputAction.CallbackContext context)
        {
            _state = State.WALK;
            _direction = context.ReadValue<Vector2>();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.performed && _isGrounded)
            {
                Jump();
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            _isGrounded = true;
            _rb.linearVelocity = Vector2.zero; 
            _rb.linearVelocity = Vector2.zero;
            WalkingLoop();
        }
    
        #endregion

     
        #region Utils

        private void WalkingLoop()
        {
            _rb.transform.Translate(_direction * _speed * Time.deltaTime);
        }

        private void Jump()
        {
            _state = State.JUMP;
            _isGrounded = false;
            Debug.Log($"Jump numéro {_counterJump}");
            _rb.AddForce(new Vector2(_direction.x, _jumpForce) * _speedJump, ForceMode2D.Impulse); 
            _counterJump++;
        }

        private void CheckJump()
        {
            if (_counterJump >= 2)
            {
                _counterJump = 0;
            }
            
            if (!_isGrounded && _rb.linearVelocity.y < 0 && _apexActivation && _counterJump >= 2)
            {
                _rb.AddForce(-transform.up * _apexForce);
            }
        }
    
        #endregion
    
    
        #region Privates and Protected
    
        [Header("Movement")]
        [SerializeField] private float _speed = 1.0f;
        [SerializeField] private float _acceleration = 1.0f;
        [SerializeField] private float _deceleration = 1.0f;
        
        [Header("Jump")]
        [SerializeField] private bool _doubleJump = true;
        [SerializeField] private float _speedJump = 1.0f;
        [SerializeField] private float _jumpForce = 1.0f;
        
        [Header("Apex")]
        [SerializeField] private bool _apexActivation = true;
        [SerializeField] private float _apexForce = 5.0f;
        
        private Rigidbody2D _rb;
        private Vector2 _direction;
        private bool _isGrounded;
        private State _state;
        private int _counterJump = 0;

        private enum State
        {
            IDLE,
            WALK,
            JUMP,
            FALL,
        }

        #endregion
    }
}

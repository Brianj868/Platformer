using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] int _playerNumber = 1;
    [Header("Movement")]
    [SerializeField] float _speed = 1;
    [SerializeField] float _slipFactor = 1;
    [Header("Jump")]
    [SerializeField] float _jumpVelocity = 10;
    [SerializeField] int _maxJumps = 2;
    [SerializeField] Transform _feet;
    [SerializeField] Transform _leftSensor;
    [SerializeField] Transform _rightSensor;
    [SerializeField] float _downPull = 5;
    [SerializeField] float _maxJumpDuration = 0.1f;
    [SerializeField] string _sceneName;
    [SerializeField] float _wallSlideSpeed = 1;
    [SerializeField] float _acceleration = 1;
    [SerializeField] float _deceleration = 1;
    [SerializeField] float _airAcceleration = 1;
    [SerializeField] float _airDeceleration = 1;

    AudioSource _audioSource;
    Vector3 _startPosition;
    int _jumpsRemaining;
    float _fallTimer;
    float _jumpTimer;
    Rigidbody2D _rigidbody2D;
    Animator _animator;
    SpriteRenderer _spriteRenderer;
    float _horizontal;
    bool _isGrounded;
    bool _isOnSlipperySurface;
    string _jumpButton;
    string _horizontalAxis;
    int _layerMask;

    public int PlayerNumber => _playerNumber;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _startPosition = transform.position;
        _jumpsRemaining = _maxJumps;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _jumpButton = $"P{_playerNumber}Jump";
        _horizontalAxis = $"P{_playerNumber}Horizontal";
        _layerMask = LayerMask.GetMask("Default");
    }

    void Update()
    {
        UpdateIsGrounded();
        ReadHorizontalInput();

        if (_isOnSlipperySurface)
            SlipHorizontal();
        else
            MoveHorizontal();

        UpdateAnimator();
        UpdateSpriteDirection();

        if (ShouldSlide())
        {
            if (ShouldStartJump())
                WallJump();
            else
                Slide();
            return;
        }     

        if (ShouldStartJump())
            Jump();
        else if (ShouldContinueJump())
            ContinueJump();

        _jumpTimer += Time.deltaTime;

        if (_isGrounded && _fallTimer > 0)
        {
            _fallTimer = 0;
            _jumpsRemaining = _maxJumps;
        }
        else
        {
            _fallTimer += Time.deltaTime;
            var downForce = _downPull * _fallTimer * _fallTimer;
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _rigidbody2D.velocity.y - downForce);
        }

    }

    void WallJump()
    {
        _rigidbody2D.velocity = new Vector2(-_horizontal * _jumpVelocity, _jumpVelocity * 1.5f);
    }

    void Slide()
    {
        _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, -_wallSlideSpeed);
    }

    bool ShouldSlide()
    {
        if (_isGrounded)
            return false;

        if (_rigidbody2D.velocity.y > 0)
            return false;

        if (_horizontal < 0)
        {
            var hit = Physics2D.OverlapCircle(_leftSensor.position, 0.1f);
            if (hit != null && hit.CompareTag("Wall"))
                return true;
        }

        if (_horizontal > 0)
        {
            var hit = Physics2D.OverlapCircle(_rightSensor.position, 0.1f);
            if (hit != null && hit.CompareTag("Wall"))
                return true;
        }

        return false;
    }

    void ContinueJump()
    {
        _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _jumpVelocity);
        _fallTimer = 0;
    }

    bool ShouldContinueJump()
    {
        return Input.GetButton(_jumpButton) && _jumpTimer <= _maxJumpDuration;
    }

    void Jump()
    {
        _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _jumpVelocity);
        _jumpsRemaining--;
        Debug.Log($"Jumps Remaining {_jumpsRemaining}");
        _fallTimer = 0;
        _jumpTimer = 0;

        if (_audioSource != null)
            _audioSource.Play();
    }

    bool ShouldStartJump()
    {
        return Input.GetButtonDown(_jumpButton) && _jumpsRemaining > 0;
    }

    void MoveHorizontal()
    {
        float smoothnessMultiplier = _horizontal == 0 ? _deceleration : _acceleration;
        if (_isGrounded == false)
            smoothnessMultiplier = _horizontal == 0 ? _airDeceleration : _airAcceleration;

        float newHorizontal = Mathf.Lerp(_rigidbody2D.velocity.x, _horizontal * _speed, Time.deltaTime * smoothnessMultiplier);
        _rigidbody2D.velocity = new Vector2(newHorizontal, _rigidbody2D.velocity.y);
    }

    void SlipHorizontal()
    {
        var desiredVelocity = new Vector2(_horizontal * _speed, _rigidbody2D.velocity.y);
        var smoothedVelocity = Vector2.Lerp(
            _rigidbody2D.velocity,
            desiredVelocity,
            Time.deltaTime / _slipFactor);

        _rigidbody2D.velocity = smoothedVelocity;
    }

    void ReadHorizontalInput()
    {
        _horizontal = Input.GetAxis(_horizontalAxis) * _speed;
    }

    void UpdateSpriteDirection()
    {
        if (_horizontal != 0)
        {
            _spriteRenderer.flipX = _horizontal < 0;
        }
    }

    void UpdateAnimator()
    {
        bool walking = _horizontal != 0;
        _animator.SetBool("Walk", walking);
        _animator.SetBool("Jump", ShouldContinueJump());
        _animator.SetBool("Slide", ShouldSlide());
    }

    void UpdateIsGrounded()
    {
        var hit = Physics2D.OverlapCircle(_feet.position, 0.1f, _layerMask);
        _isGrounded = hit != null;

        if (hit != null)
            _isOnSlipperySurface = hit.CompareTag("Slippery");
        else
            _isOnSlipperySurface = false;
    }

    internal void ResetToStart()
    {
        //_rigidbody2D.position = _startPosition;
        SceneManager.LoadScene(_sceneName);
    }

    internal void TeleportTo(Vector3 position)
    {
        _rigidbody2D.position = position;
        _rigidbody2D.velocity = Vector2.zero;
    }
}
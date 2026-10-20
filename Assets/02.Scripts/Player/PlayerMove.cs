using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMove : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 7f;

    [SerializeField] private float _sprintMoveSpeed = 12f;

    [Header("Jump")]
    [SerializeField] private float _jumpPower = 10f;

    [SerializeField] private int _maxJumpCount = 2;

    [Header("Stamina")]
    [SerializeField] private float _maxStamina = 100f;

    [SerializeField] private float _staminaDecreaseRate = 20f;
    [SerializeField] private float _staminaRecoveryRate = 15f;

    [Header("Gravity")]
    [SerializeField] private float _gravity = -20f;

    [SerializeField] private float _groundedVelocity = -2f;

    [Header("Components")]
    [SerializeField] private CharacterController _characterController;

    private Vector3 _moveDirection;

    private float _yVelocity;
    private float _curStamina;

    private int _curJumpCount;

    private bool _isSprinting;

    public float CurrentStamina => _curStamina;
    public float MaxStamina => _maxStamina;
    public bool IsSprinting => _isSprinting;

    private void Awake()
    {
        if (_characterController == null)
            _characterController = GetComponent<CharacterController>();

        _curStamina = _maxStamina;
    }

    private void Update()
    {
        HandleMovementInput();
        HandleGround();
        HandleJump();
        HandleSprint();
        HandleGravity();
        ApplyMovement();
    }

    // 이동 입력
    private void HandleMovementInput()
    {
        var h = Input.GetAxisRaw("Horizontal");
        var v = Input.GetAxisRaw("Vertical");

        var inputDirection = new Vector3(h, 0f, v).normalized;

        _moveDirection = transform.TransformDirection(inputDirection);
    }

    // 지면 확인
    private void HandleGround()
    {
        if (!_characterController.isGrounded)
            return;

        _curJumpCount = 0;

        if (_yVelocity < 0f)
            _yVelocity = _groundedVelocity;
    }

    // 점프
    private void HandleJump()
    {
        if (!Input.GetButtonDown("Jump"))
            return;

        if (_curJumpCount >= _maxJumpCount)
            return;

        _yVelocity = _jumpPower;
        _curJumpCount++;
    }

    // 달리기 및 스태미나
    private void HandleSprint()
    {
        var isMoving = _moveDirection.sqrMagnitude > 0f;
        var isHoldingSprint = Input.GetKey(KeyCode.LeftShift);

        _isSprinting =
            isHoldingSprint &&
            isMoving &&
            _characterController.isGrounded &&
            _curStamina > 0f;

        if (_isSprinting)
            _curStamina -= _staminaDecreaseRate * Time.deltaTime;
        else
            _curStamina += _staminaRecoveryRate * Time.deltaTime;

        _curStamina = Mathf.Clamp(_curStamina, 0f, _maxStamina);
    }

    // 중력
    private void HandleGravity()
    {
        if (_characterController.isGrounded && _yVelocity < 0f)
            return;

        _yVelocity += _gravity * Time.deltaTime;
    }

    // 최종 이동
    private void ApplyMovement()
    {
        var currentSpeed = _isSprinting
            ? _sprintMoveSpeed
            : _moveSpeed;

        var velocity = _moveDirection * currentSpeed;

        velocity.y = _yVelocity;

        _characterController.Move(velocity * Time.deltaTime);
    }
}
using System;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
   

    [Header("점프 조작감")]
    [SerializeField, Min(0f)] private float _jumpBufferTime = 0.15f;
    [SerializeField, Min(0f)] private float _coyoteTime = 0.12f;
    [SerializeField, Min(1f)] private float _fallGravityMultiplier = 2f;
    [SerializeField, Min(1f)] private float _jumpCutGravityMultiplier = 2.5f;
    [SerializeField, Min(0f)] private float _apexGravityMultiplier = 0.7f;
    [SerializeField, Min(0f)] private float _apexVelocityThreshold = 2f;
    
    [SerializeField] private float _staminaRecovery = 15;
    [SerializeField] private float _sprintStaminaCost =20;
    [SerializeField] private float _jumpStatminaCost = 20;


    [Header("중력")]
    [SerializeField] private const float Gravity = -20f;

    private float _yVelocity = 0;
    private int _jumpCount = 0;

    private float _stamina = 0;
    public float Stamina=>_stamina;
    private float _jumpBufferCounter = 0f;
    private float _coyoteCounter = 0f;
    public event Action OnStaminaUse;
    [Header("캐릭터 컨트롤러 참조")]
    [SerializeField] private CharacterController _characterController;

    private Transform _movementCamera;

    private void Awake()
    {
        if (_characterController == null) _characterController = GetComponent<CharacterController>();
        if (Camera.main != null) _movementCamera = Camera.main.transform;
        _stamina = PlayerStat.MaxStamina;
 
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        if (GameManager.Instance.GameState != GameState.Playing)
            return;
        float deltaTime = Time.deltaTime;
        _stamina = Mathf.Min(PlayerStat.MaxStamina, _stamina + _staminaRecovery * deltaTime);

        if (Input.GetButtonDown("Jump"))
            _jumpBufferCounter = _jumpBufferTime;
        else
            _jumpBufferCounter = Mathf.Max(0f, _jumpBufferCounter - deltaTime);
        
        
        var h = Input.GetAxisRaw("Horizontal"); //-1 0 1 
        var v = Input.GetAxisRaw("Vertical");

        var input = new Vector2(h, v);
        if (input.sqrMagnitude > 1f) input.Normalize();

        var referenceForward = _movementCamera != null ? _movementCamera.forward : transform.forward;
        var forward = Vector3.ProjectOnPlane(referenceForward, Vector3.up);
        if (forward.sqrMagnitude < 0.0001f)
            forward = Vector3.ProjectOnPlane(transform.forward, Vector3.up);
        forward.Normalize();

        var right = Vector3.Cross(Vector3.up, forward).normalized;
        var direction = right * input.x + forward * input.y;
        //로컬 좌표꼐 : 한 대상을 중심
        //월드 좌표계 : 세상을 중심 

        bool isGrounded = _characterController.isGrounded;
        if (isGrounded)
        {
            _yVelocity = -1f;
            _jumpCount = 0;
            _coyoteCounter = _coyoteTime;
        }
        else
        {
            _coyoteCounter = Mathf.Max(0f, _coyoteCounter - deltaTime);
        }

        bool canGroundJump = _jumpCount == 0 && (isGrounded || _coyoteCounter > 0f);
        bool canDoubleJump = _jumpCount > 0 && _jumpCount < PlayerStat.MaxJumps;
        if (_jumpBufferCounter > 0f && (canGroundJump || canDoubleJump) && _stamina >= _jumpStatminaCost)
        {
            _yVelocity = PlayerStat.JumpPower;
            _jumpCount++;
            _stamina -= _jumpStatminaCost;
            _jumpBufferCounter = 0f;
            _coyoteCounter = 0f;
            OnStaminaUse.Invoke();
        }

        if (!isGrounded || _yVelocity > 0f)
        {
            float gravityMultiplier = 1f;
            if (_yVelocity < 0f)
                gravityMultiplier = _fallGravityMultiplier;
            else if (_yVelocity < _apexVelocityThreshold)
                gravityMultiplier = _apexGravityMultiplier;
            else if (!Input.GetButton("Jump"))
                gravityMultiplier = _jumpCutGravityMultiplier;

            _yVelocity += Gravity * gravityMultiplier * deltaTime;
        }

        float RequireStamina = _sprintStaminaCost * Time.deltaTime;
        bool wantSprint = Input.GetButton("Sprint")&&_stamina>=RequireStamina;
        float currentSpeed;
        if (wantSprint)
        {
            currentSpeed = PlayerStat.SprintSpeed;
            _stamina=Mathf.Max(0,_stamina-RequireStamina);
            OnStaminaUse.Invoke();
        }
        else
        {
            currentSpeed = PlayerStat.MoveSpeed;
        }
        

        var velocity = direction * currentSpeed;

        velocity.y = _yVelocity;
        //transform.position += direction * _moveSpeed * Time.deltaTime;
        _characterController.Move(velocity * deltaTime);
    }
}

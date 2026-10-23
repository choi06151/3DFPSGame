using System;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("이동속도")]
    [SerializeField] private float _moveSpeed = 7f;

    [SerializeField] private float _sprintSpeed = 12;
    
    [SerializeField] private const float MaxHp = 100;
    public float GetMaxHp=>MaxHp;
 
    
    
    [SerializeField] private const float MaxStamina = 100;
    public float GetMaxStamina=>MaxStamina;
    [SerializeField] private float _jumpPower = 10f;
    [SerializeField]private const int MAXjumps = 2;

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
    private float _hp=0;
    public float HP => _hp;
    private float _stamina = 0;
    public float Stamina=>_stamina;
    private float _jumpBufferCounter = 0f;
    private float _coyoteCounter = 0f;
    [Header("캐릭터 컨트롤러 참조")]
    [SerializeField] private CharacterController _characterController;

    private void Awake()
    {
        if (_characterController == null) _characterController = GetComponent<CharacterController>();
        _stamina = MaxStamina;
        _hp=MaxHp;
 
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        float deltaTime = Time.deltaTime;
        _stamina=Mathf.Min(MaxStamina, _stamina + _staminaRecovery*Time.deltaTime);
        _stamina = Mathf.Clamp(_stamina,0,MaxStamina);

        if (Input.GetButtonDown("Jump"))
            _jumpBufferCounter = _jumpBufferTime;
        else
            _jumpBufferCounter = Mathf.Max(0f, _jumpBufferCounter - deltaTime);
        
        
        var h = Input.GetAxisRaw("Horizontal"); //-1 0 1 
        var v = Input.GetAxisRaw("Vertical");

        var direction = new Vector3(h, 0, v).normalized;
        direction = transform.TransformDirection(direction);
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
        bool canDoubleJump = _jumpCount > 0 && _jumpCount < MAXjumps;
        if (_jumpBufferCounter > 0f && (canGroundJump || canDoubleJump) && _stamina >= _jumpStatminaCost)
        {
            _yVelocity = _jumpPower;
            _jumpCount++;
            _stamina -= _jumpStatminaCost;
            _jumpBufferCounter = 0f;
            _coyoteCounter = 0f;
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
            currentSpeed = _sprintSpeed;
            _stamina=Mathf.Max(0,_stamina-RequireStamina);
        }
        else
        {
            currentSpeed = _moveSpeed;
        }
        

        var velocity = direction * currentSpeed;

        velocity.y = _yVelocity;
        //transform.position += direction * _moveSpeed * Time.deltaTime;
        _characterController.Move(velocity * deltaTime);
    }
}

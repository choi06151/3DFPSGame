using System;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("이동속도")]
    [SerializeField] private float _moveSpeed = 7f;

    [SerializeField] private float _jumpPower = 10f;

    [Header("중력")]
    [SerializeField] private const float Gravity = -20f;

    private float _yVelocity = 0;

    [Header("캐릭터 컨트롤러 참조")]
    [SerializeField] private CharacterController _characterController;

    private void Awake()
    {
        if (_characterController == null) _characterController = GetComponent<CharacterController>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        var h = Input.GetAxisRaw("Horizontal"); //-1 0 1 
        var v = Input.GetAxisRaw("Vertical");

        var direction = new Vector3(h, 0, v).normalized;
        direction = transform.TransformDirection(direction);
        //로컬 좌표꼐 : 한 대상을 중심
        //월드 좌표계 : 세상을 중심 

        if (!_characterController.isGrounded)
        {
            _yVelocity += Gravity * Time.deltaTime;
        }
        else
        {
            _yVelocity = 0;
        }
        //4.만약에 점프키를 누르면 점프를 한다

        if (Input.GetButtonDown("Jump")&&_characterController.isGrounded)
        {
            _yVelocity = _jumpPower;
        }

        var velocity = direction * _moveSpeed;
        velocity.y = _yVelocity;
        //transform.position += direction * _moveSpeed * Time.deltaTime;
        _characterController.Move(velocity * Time.deltaTime);
    }
}
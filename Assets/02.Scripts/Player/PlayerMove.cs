using System;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("이동속도")]
    [SerializeField] private float _moveSpeed = 20f;

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

        //transform.position += direction * _moveSpeed * Time.deltaTime;
        _characterController.Move(direction * _moveSpeed * Time.deltaTime);

    }
}
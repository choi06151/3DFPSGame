using System;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    [Header("카메라 회전속도")]
    [SerializeField] private float _rotationSpeed = 200f;

    private float _accumulationX = 0f;

    private float _accumulationY = 0f;
    // Update is called once per frame
    void Update()
    {
        Cursor.visible = false;
        //1. 마우스 입력을 받는다
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        //2. 마우스 입력에 따라 회전할 방향을 구한다
        _accumulationX += mouseX * _rotationSpeed * Time.deltaTime;
        _accumulationY += -mouseY * _rotationSpeed * Time.deltaTime;

        //3. 해당 방향으로 회전 시킨다

        _accumulationY = Mathf.Clamp(_accumulationY, -90f, 90f);

        transform.eulerAngles = new Vector3(_accumulationY, _accumulationX, 0);

    }
}

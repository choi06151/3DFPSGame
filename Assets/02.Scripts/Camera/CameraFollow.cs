using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("따라다닐 타겟")][SerializeField] private Transform _target;

    private void Awake()
    {
        if (_target == null)
        {
            Debug.Log("설정된 타켓이 없습니다");
        }
    }

    private void LateUpdate()
    {
        transform.position = _target.position;
    }
}

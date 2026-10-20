using System;
using UnityEngine;

public class PlayerGunFire : MonoBehaviour
{
    [Header("총알 이펙트")]
    [SerializeField]private ParticleSystem _hitEffectPrefab;
    
    [Header("수치")]
    [SerializeField] private float _range = 100;
    [SerializeField]private float _damage = 10;
    [SerializeField]private float _fireInterval = 0.1f;

    private float _fireTimer=0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _fireTimer -= Time.deltaTime;
        
        if (Input.GetButton("Fire1")&&_fireTimer<0)
        {
            _fireTimer = _fireInterval;
            Fire();
            
        }
        //사용자 왼쪽 버튼 입력 받기
        // 레이를 생성하고 발사될 위치와 진행방향 설정하기
        //레이가 부딫힌 대상의 정보를 저장할 변수 생성하기
        //레이를 발사하고 부딫힌 물체가 있다면 이펙트 표시하기
    }

    private void Fire()
    {
        Transform cameraTransform = Camera.main.transform;
        
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        
        RaycastHit hitInfo=new RaycastHit();
        
        bool isHit=Physics.Raycast(ray, out hitInfo,_range);

        if (isHit)
        {
            ParticleSystem effect = Instantiate(_hitEffectPrefab);
            effect.transform.position = hitInfo.point; 
            effect.transform.forward = hitInfo.normal;
        }
    }
}

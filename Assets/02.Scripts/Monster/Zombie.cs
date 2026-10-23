using System;
using System.Collections;
using System.Net.WebSockets;
using UnityEngine;

public class Zombie : MonoBehaviour,IDamageAble
{
    public enum ZombieState
    {
        Patrol,
        Trace,Combat
        ,Attack,Hit,Comeback
    }
    
    private ZombieState _state=ZombieState.Patrol;

    [Header("스탯")]
    [SerializeField] private float _detectDistance = 8;
    [SerializeField] private float _comebackDistance = 12;
    [SerializeField] private float _attackDamage = 10f;

    [SerializeField] private float _attackRange = 2f;
    [SerializeField] private float _attackInterval = 1.2f;
    
    [Header("이펙트")]
    [SerializeField]private GameObject _bloodSprayPrefab;  
    
    [SerializeField] private float _moveSpeed=5;
    [SerializeField] private float _patrolDistance = 3f;
    [SerializeField] private float _maxHealth = 30f;
    public float MaxHealth=>_maxHealth;
    [SerializeField] private float _health = 30f;
    public float Health=>_health;
    [SerializeField]private CharacterController _controller;
    [SerializeField]private GameObject _player;

    private float _attackTimer = 0f;
    private bool _isDead;
    
    private Vector3 _startPosition;
    private Vector3 _patrolCenter;
    private Vector3 _patrolTarget;
    private void Awake()
    {
        _controller=GetComponent<CharacterController>();
    }

    void Start()
    {
        _player=GameObject.FindGameObjectWithTag("Player");
        _patrolCenter = transform.position;
        _startPosition = _patrolCenter;
        _patrolTarget = _patrolCenter + transform.forward * _patrolDistance;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (GameManager.Instance.GameState != GameState.Playing)
            return;
        
        switch (_state)
        {
            case ZombieState.Patrol:
            {
                Patrol();
                break;
            }
            case ZombieState.Trace:
                {
                    Trace();
                break;
                
                }
            case ZombieState.Combat:
            {
                break;
            }
            case ZombieState.Hit:
            {
                break;
            }
            case ZombieState.Comeback:
            {
                Comeback();
                break;
                
            }
            case ZombieState.Attack:
            {
                Attack();
                break;
                
            }
            
            
        }
        
    }

    private void Patrol()
    {
        float distance=Vector3.Distance(transform.position,_player.transform.position);
        if (distance <= _detectDistance)
        {
            Debug.Log($"{_state}->Trace");
            _startPosition = transform.position;
            _state = ZombieState.Trace;
            return;
        }

        Vector3 direction = _patrolTarget - transform.position;
        direction.y = 0f;
        if (direction.magnitude < 0.2f)
        {
            _patrolTarget = _patrolTarget == _patrolCenter
                ? _patrolCenter + transform.forward * _patrolDistance
                : _patrolCenter;
            direction = _patrolTarget - transform.position;
            direction.y = 0f;
        }

        direction.Normalize();
        transform.forward = direction;
        _controller.SimpleMove(direction * _moveSpeed);
    }

    private void Trace()
    {
        if (Vector3.Distance(transform.position, _player.transform.position) > _comebackDistance)
        {
            Debug.Log($"{_state}->Comeback");

            _state = ZombieState.Comeback;
            return;
        } else if (Vector3.Distance(transform.position, _player.transform.position) < _attackRange)
        {
            _attackTimer = 0;
            Debug.Log($"{_state}->Attack");

            _state = ZombieState.Attack;
            return;
        }
        
        Vector3 direction=_player.transform.position-transform.position;
        direction.y = 0f;
        direction.Normalize();
        
        transform.forward=direction;
        _controller.SimpleMove(direction*_moveSpeed);
    }

    public void Comeback()
    {
        if (Vector3.Distance(transform.position, _startPosition) < 1f)
        {
            Debug.Log($"{_state}->Patrol");

            _state = ZombieState.Patrol;
            return;
        }
        
        
        Vector3 direction=_startPosition-transform.position;
        direction.y = 0f;
        direction.Normalize();
        
        transform.forward=direction;
        _controller.SimpleMove(direction*_moveSpeed);
    }

    public void Attack()
    {
        //공격 사거리 밖으로 플레이어가 이동하면  다시 쫒아간다
        float distance=Vector3.Distance(transform.position,_player.transform.position);
        if (distance >_detectDistance)
        {
            Debug.Log($"{_state}->Trace");
            _startPosition=transform.position;
            _state = ZombieState.Trace;
        }
        
        
        _attackTimer+=Time.deltaTime;
        if (_attackTimer >= _attackInterval)
        {
            DamageInfo info = new DamageInfo();
            {
                info.Value = _attackDamage;
                
            }
            
            _player.GetComponent<IDamageAble>().TakeDamage(info);
            
            //플레이어 공격
            _attackTimer = 0;
            
        }
    }
    
    public void TakeDamage(DamageInfo damageInfo)
    {
        if (_isDead || damageInfo.Value <= 0f)
            return;

        _health -= damageInfo.Value;

        if (_bloodSprayPrefab != null)
        {
            GameObject spray = Instantiate(_bloodSprayPrefab, damageInfo.HitPoint,
                damageInfo.HitNormal.sqrMagnitude > 0f
                    ? Quaternion.LookRotation(damageInfo.HitNormal)
                    : Quaternion.identity);
        }
        
        if (_health <= 0)
        {
            _isDead = true;
            StartCoroutine(Death_Coroutine());
            return;
        }
        
        //잠깐 멈칫 하고싶다
        Debug.Log($"{_state}->Hit");

        _state=ZombieState.Hit;
        StartCoroutine(Hit_Coroutine());

    }

    private IEnumerator Hit_Coroutine()
    {
        yield return new WaitForSeconds(0.5f);
        if (_isDead)
            yield break;
        Debug.Log($"{_state}->Trace");
        _state = ZombieState.Trace;

    }
    private IEnumerator Death_Coroutine()
    {
        _controller.enabled = false;
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);

    }
}

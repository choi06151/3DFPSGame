using System;
using System.Collections;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    public enum ZombieState
    {
        Idle,
        Trace,Combat
        ,Attack,Hit,Comeback
    }
    
    private ZombieState _state=ZombieState.Idle;

    [Header("스탯")]
    [SerializeField] private float _detectDistance = 8;
    [SerializeField] private float _comebackDistance = 12;
    [SerializeField] private float _attackRange = 2f;
    [SerializeField] private float _attackInterval = 1.2f;
    
    [SerializeField] private float _moveSpeed=5;
    [SerializeField] private float _health = 30f;
    [SerializeField]private CharacterController _controller;
    [SerializeField]private GameObject _player;

    private float _attackTimer = 0f;
    
    private Vector3 _startPosition;
    private void Awake()
    {
        _controller=GetComponent<CharacterController>();
    }

    void Start()
    {
        _player=GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        switch (_state)
        {
            case ZombieState.Idle:
            {
                Idle();
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

    private void Idle()
    {
        float distance=Vector3.Distance(transform.position,_player.transform.position);
        if (distance <= _detectDistance)
        {
            Debug.Log($"{_state}->Trace");
                _startPosition=transform.position;
            _state = ZombieState.Trace;
        }
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
            Debug.Log($"{_state}->Idle");

            _state = ZombieState.Idle;
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
            //플레이어 공격
            _attackTimer = 0;
            
        }
    }
    
    public void TakeDamage(float damage)
    {
        _health -= damage;

        if (_health <= 0)
        {
            StartCoroutine(Death_Coroutine());
        }
        
        //잠깐 멈칫 하고싶다
        Debug.Log($"{_state}->Hit");

        _state=ZombieState.Hit;
        StartCoroutine(Hit_Coroutine());

    }

    private IEnumerator Hit_Coroutine()
    {
        yield return new WaitForSeconds(0.5f);
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

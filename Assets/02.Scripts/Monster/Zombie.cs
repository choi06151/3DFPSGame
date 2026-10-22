using System;
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
    [SerializeField] private float _moveSpeed=5;
    [SerializeField]private CharacterController _controller;
    [SerializeField]private GameObject _player;

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
}

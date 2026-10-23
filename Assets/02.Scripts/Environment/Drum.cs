using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class Drum : MonoBehaviour,IDamageAble
{
    [SerializeField] private ParticleSystem _explosionEffectPrefab;

    [Header("스탯")]
    [SerializeField] private float _health = 30f;
    [SerializeField] private float _explodedamage = 30f;

    [SerializeField] private float _explodePower = 30f;
    [SerializeField] private float _explodeRadius = 30f;

    [SerializeField] private float _spinPower = 30f;
    [SerializeField] private float _destroyDelay = 3f;
    [SerializeField] private float _explostionDelay = 3f;

    private Rigidbody _rigidbody;


    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void TakeDamage(float damage)
    {
        if (_health <= 0)
            return;
        _health -= damage;
        if (_health <= 0)
        {
            StartCoroutine(Explode());

        }
    }

    private IEnumerator Explode()
    {
        yield return new WaitForSeconds(_explostionDelay);

        Instantiate(_explosionEffectPrefab, transform.position, Quaternion.identity);
        _rigidbody.AddForce(Vector3.up * _explodePower, ForceMode.Impulse);
        _rigidbody.AddTorque(UnityEngine.Random.insideUnitSphere * _spinPower, ForceMode.Impulse);
        
        //주위에폭팔을 전달하여 공격한다
        ApplyAreaDamage();
        
        yield return new WaitForSeconds(_destroyDelay);
        Destroy(this.gameObject);
    }

    private void ApplyAreaDamage()
    {
        var overlapSphere=Physics.OverlapSphere(transform.position, _explodeRadius, LayerMask.GetMask("Drum","Monster"));

        foreach (var overlap in overlapSphere)
        {
            overlap.GetComponent<Drum>()?.TakeDamage(_explodedamage);
            overlap.GetComponent<Zombie>()?.TakeDamage(_explodedamage);

        }
    }

   

}

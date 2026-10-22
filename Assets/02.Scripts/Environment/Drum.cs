using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class Drum : MonoBehaviour
{
    [SerializeField] private ParticleSystem _explosionEffectPrefab;

    [Header("스탯")]
    [SerializeField] private float _health = 30f;

    [SerializeField] private float _explodePower = 30f;
    [SerializeField] private float _spinPower = 30f;
    [SerializeField] private float destroyDelay = 3f;
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
        Instantiate(_explosionEffectPrefab, transform.position, Quaternion.identity);
        _rigidbody.AddForce(Vector3.up * _explodePower, ForceMode.Impulse);
        _rigidbody.AddTorque(UnityEngine.Random.insideUnitSphere * _spinPower, ForceMode.Impulse);
        yield return new WaitForSeconds(destroyDelay);
        Destroy(this.gameObject);
    }

   

}

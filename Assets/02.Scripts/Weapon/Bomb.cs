using System;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private ParticleSystem _explosionEffectPrefab;

    private Action<Bomb> _returnToPool;
    private Rigidbody _rigidbody;
    private ParticleSystem _explosionEffect;
    private bool _hasExploded;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        if (_explosionEffectPrefab != null)
        {
            _explosionEffect = Instantiate(_explosionEffectPrefab);
            _explosionEffect.gameObject.SetActive(false);
        }
    }

    public void Initialize(Action<Bomb> returnToPool)
    {
        _returnToPool = returnToPool;
    }

    public void Throw(Vector3 direction, float power)
    {
        _hasExploded = false;
        if (_rigidbody == null)
            _rigidbody = GetComponent<Rigidbody>();

        _rigidbody.linearVelocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        _rigidbody.AddForce(direction * power, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_hasExploded)
            return;

        _hasExploded = true;
        if (_explosionEffect != null)
        {
            _explosionEffect.transform.position = transform.position;
            _explosionEffect.gameObject.SetActive(true);
            _explosionEffect.Play(true);
        }

        _rigidbody.linearVelocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        _returnToPool?.Invoke(this);
    }
}

using System.Collections.Generic;
using UnityEngine;

public class PlayerBombFire : MonoBehaviour
{
    private const int MaxBombCount = 10;

    [SerializeField] private Transform _firePoint;
    [SerializeField] private Bomb _bombPrefab;
    [SerializeField] private float _minThrowPower = 5f;
    [SerializeField] private float _maxThrowPower = 20f;
    [SerializeField] private float _chargeDuration = 2f;

    private readonly Queue<Bomb> _bombPool = new Queue<Bomb>();
    private int _bombsThrown;
    private float _chargeTime;
    private bool _isCharging;

    private void Awake()
    {
        for (int i = 0; i < MaxBombCount; i++)
        {
            Bomb bomb = Instantiate(_bombPrefab, transform);
            bomb.Initialize(ReturnBombToPool);
            bomb.gameObject.SetActive(false);
            _bombPool.Enqueue(bomb);
        }
    }

    private void Update()
    {
        if (_bombsThrown >= MaxBombCount)
        {
            _isCharging = false;
            return;
        }

        if (Input.GetButtonDown("Fire2"))
        {
            _isCharging = true;
            _chargeTime = 0f;
        }

        if (_isCharging && Input.GetButton("Fire2"))
            _chargeTime += Time.deltaTime;

        if (_isCharging && Input.GetButtonUp("Fire2"))
        {
            ThrowBomb();
            _isCharging = false;
        }
    }

    private void ThrowBomb()
    {
        if (_bombPool.Count == 0 || _firePoint == null || Camera.main == null)
            return;

        Bomb bomb = _bombPool.Dequeue();
        bomb.transform.SetPositionAndRotation(_firePoint.position, Quaternion.identity);
        bomb.gameObject.SetActive(true);

        float chargeRatio = _chargeDuration <= 0f ? 1f : Mathf.Clamp01(_chargeTime / _chargeDuration);
        float throwPower = Mathf.Lerp(_minThrowPower, _maxThrowPower, chargeRatio);
        bomb.Throw(Camera.main.transform.forward, throwPower);
        _bombsThrown++;
    }

    private void ReturnBombToPool(Bomb bomb)
    {
        bomb.gameObject.SetActive(false);
        bomb.transform.SetParent(transform);
        _bombPool.Enqueue(bomb);
    }
}

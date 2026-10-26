using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(HurtUIEffect))]
public class Player : MonoBehaviour, IDamageAble
{
    [SerializeField] private Slider _healthBar;

    private float _hp;
    public float HP => _hp;
    public event Action OnDamaged;
    private void Awake()
    {
        _hp = PlayerStat.MaxHealth;
        UpdateHealthBar();
    }

    public void TakeDamage(DamageInfo damageInfo)
    {
        if (_hp <= 0f || damageInfo.Value <= 0f)
            return;

        _hp -= damageInfo.Value;
        _hp = Mathf.Max(0f, _hp);
        UpdateHealthBar();
        OnDamaged.Invoke();
        if (_hp <= 0f)
        {
            Debug.Log("Player is dead");
        }
    }

    private void UpdateHealthBar()
    {
        if (_healthBar != null)
            _healthBar.value = _hp / PlayerStat.MaxHealth;
    }
}

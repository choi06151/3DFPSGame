using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(HurtUIEffect))]
public class Player : MonoBehaviour, IDamageAble
{
    [SerializeField] private Slider _healthBar;
    private WeaponMode _weaponMode = WeaponMode.Normal;
    public WeaponMode WeaponMode => _weaponMode;
    private float _hp;
    public float HP => _hp;
    public event Action OnDamaged;
    public event Action OnSniperMode;
    public event Action OnNormalMode;
    private void Awake()
    {
        _hp = PlayerStat.MaxHealth;
        UpdateHealthBar();
    }


    private void Update()
    {
        if (GameManager.Instance.GameState != GameState.Playing)
        {
            return;
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _weaponMode = WeaponMode.Sniper;
            OnSniperMode.Invoke();
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            _weaponMode = WeaponMode.Normal;
            OnNormalMode.Invoke();
        }
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

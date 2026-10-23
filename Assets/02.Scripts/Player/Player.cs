using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(HurtUIEffect))]
public class Player : MonoBehaviour, IDamageAble
{
    [SerializeField] private HurtUIEffect _hurtUIEffect;
    [SerializeField] private Slider _healthBar;

    private float _hp;
    public float HP => _hp;

    private void Awake()
    {
        _hp = PlayerStat.MaxHealth;
        if (_hurtUIEffect == null)
            _hurtUIEffect = GetComponent<HurtUIEffect>();
        UpdateHealthBar();
    }

    public void TakeDamage(DamageInfo damageInfo)
    {
        if (_hp <= 0f || damageInfo.Value <= 0f)
            return;

        _hp -= damageInfo.Value;
        _hp = Mathf.Max(0f, _hp);
        _hurtUIEffect?.Play();
        UpdateHealthBar();

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

using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoBar : MonoBehaviour
{
    [SerializeField]private Slider _healthBar;
    [SerializeField]private Slider _staminaBar;

    private PlayerMove _playerMove;

    private Player _player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _player=GetComponent<Player>();
        _player.OnDamaged += RefreshBar;
        _playerMove = GetComponent<PlayerMove>();
        _playerMove.OnStaminaUse += RefreshBar;
    }

    // Update is called once per frame
    void LateUpdate()
    {
      

        transform.forward=Camera.main.transform.forward;

    }
    
    public void RefreshBar()
    {
        _healthBar.value = _player.HP / PlayerStat.MaxHealth;
        _staminaBar.value = _playerMove.Stamina / PlayerStat.MaxStamina;

    }
}

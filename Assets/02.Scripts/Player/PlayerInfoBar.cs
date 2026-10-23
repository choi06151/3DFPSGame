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
        _playerMove = GetComponent<PlayerMove>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        _staminaBar.value = _playerMove.Stamina / PlayerStat.MaxStamina;
        _healthBar.value = _player.HP / PlayerStat.MaxHealth;

        transform.forward=Camera.main.transform.forward;

    }
}

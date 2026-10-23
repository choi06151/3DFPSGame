using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoBar : MonoBehaviour
{
    [SerializeField]private Slider _healthBar;
    [SerializeField]private Slider _staminaBar;

    private PlayerMove _playerMove;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _playerMove = GetComponent<PlayerMove>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        _staminaBar.value = _playerMove.Stamina / _playerMove.GetMaxStamina;
        _healthBar.value = _playerMove.HP / _playerMove.GetMaxHp;

        transform.forward=Camera.main.transform.forward;

    }
}

using UnityEngine;
using UnityEngine.UI;

public class ZombieHealthBar : MonoBehaviour
{
    [SerializeField]private Slider _healthBar;

    private Zombie _zombie;

    private void Awake()
    {
        
        _zombie = GetComponent<Zombie>();
        _zombie.OnDamged+=RefreshHealthBar;
    }
    private void LateUpdate()
    {
        //빌보드 : 체력바가 카메라를 쳐다보게한다
        _healthBar.transform.forward=Camera.main.transform.forward;
    }

    public void RefreshHealthBar()
    {
        _healthBar.value = _zombie.Health/_zombie.MaxHealth;
    }
}

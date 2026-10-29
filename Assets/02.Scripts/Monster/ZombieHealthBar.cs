using UnityEngine;
using UnityEngine.UI;

public class ZombieHealthBar : MonoBehaviour
{
    [SerializeField]private Slider _healthBar;

    private Zombie _zombie;
    private Transform _canvasTransform;

    private void Awake()
    {

        _zombie = GetComponent<Zombie>();
        _zombie.OnDamged+=RefreshHealthBar;
        _canvasTransform = _healthBar.GetComponentInParent<Canvas>().transform;
    }
    private void LateUpdate()
    {
        //빌보드 : 체력바가 카메라를 쳐다보게한다
        //슬라이더가 아니라 캔버스를 돌린다 (캔버스 z 스케일이 0.01이라 자식을 돌리면 찌그러짐)
        _canvasTransform.forward=Camera.main.transform.forward;
    }

    public void RefreshHealthBar()
    {
        _healthBar.value = _zombie.Health/_zombie.MaxHealth;
    }
}

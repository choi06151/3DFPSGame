using UnityEngine;
using UnityEngine.UI;

public class ZombieHealthBar : MonoBehaviour
{
    [SerializeField]private Slider _healthBar;

    private Zombie _zombie;

    private void Awake()
    {
        
        _zombie = GetComponent<Zombie>();
        
    }
    private void LateUpdate()
    {
        _healthBar.value = _zombie.Health/_zombie.MaxHealth;
        
        //빌보드 : 체력바가 카메라를 쳐다보게한다
        transform.forward=Camera.main.transform.forward;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

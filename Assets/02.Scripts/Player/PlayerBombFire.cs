using UnityEngine;

public class PlayerBombFire : MonoBehaviour
{

    [SerializeField]private Transform _firePoint;
    [SerializeField] private Bomb _bombPrefab;

    [SerializeField] private float _throwPower=10f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            Fire();
        }
    }

    private void Fire()
    {
        Bomb bomb=Instantiate(_bombPrefab,_firePoint.position,Quaternion.identity);
        Rigidbody rigidbody=bomb.GetComponent<Rigidbody>();
        rigidbody.AddForce(Camera.main.transform.forward*_throwPower,ForceMode.Impulse);
    }
}

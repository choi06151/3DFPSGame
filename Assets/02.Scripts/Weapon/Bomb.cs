using UnityEngine;

public class Bomb : MonoBehaviour
{

    [SerializeField] private ParticleSystem _explosionEffectPrefab;

    private void OnCollisionEnter(Collision collision)
    {
        ParticleSystem effect = Instantiate(_explosionEffectPrefab);
        effect.transform.position = transform.position;
        
        Destroy(gameObject);
        //풀링
    }
  
}

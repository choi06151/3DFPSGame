using UnityEngine;

public class Box : MonoBehaviour, IDamageAble
{
    [SerializeField, Min(1f)] private float _health = 10f;

    public void TakeDamage(DamageInfo damageInfo)
    {
        if (_health <= 0f || damageInfo.Value <= 0f)
            return;

        _health -= damageInfo.Value;
        if (_health <= 0f)
            Destroy(gameObject);
    }
}

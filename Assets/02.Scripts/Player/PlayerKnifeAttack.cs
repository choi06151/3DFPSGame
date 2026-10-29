using UnityEngine;

public class PlayerKnifeAttack : MonoBehaviour
{
    [SerializeField]private Animator _animator;

    [Header("수치")]
    [SerializeField] private float _range = 2f;
    [SerializeField] private float _damage = 20f;
    [SerializeField] private float _attackInterval = 0.6f;

    private float _attackTimer;

    void Update()
    {
        if (GameManager.Instance.GameState != GameState.Playing)
        {
            return;
        }

        _attackTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.F) && _attackTimer <= 0f)
        {
            _attackTimer = _attackInterval;
            KnifeAttack();
        }
    }

    private void KnifeAttack()
    {
        if (_animator != null)
            _animator.SetTrigger("Knife");

        Camera mainCamera = Camera.main;
        if (mainCamera == null)
            return;

        // 카메라 정면으로 짧은 레이를 쏴서 맞은 대상에게 데미지를 준다
        Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
        if (!Physics.Raycast(ray, out RaycastHit hitInfo, _range))
            return;

        var damageInfo = new DamageInfo
        {
            Value = _damage,
            HitPoint = hitInfo.point,
            HitNormal = hitInfo.normal
        };

        if (hitInfo.collider.GetComponentInParent<IDamageAble>() is IDamageAble damageAble)
        {
            damageAble.TakeDamage(damageInfo);
        }
    }
}

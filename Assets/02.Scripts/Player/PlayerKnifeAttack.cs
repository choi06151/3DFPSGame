using System.Collections.Generic;
using UnityEngine;

public class PlayerKnifeAttack : MonoBehaviour
{
    [SerializeField]private Animator _animator;
    [SerializeField] private CameraSwitcher _cameraSwitcher;

    [Header("수치")]
    [SerializeField] private float _range = 2f;
    [SerializeField, Range(0f, 180f)] private float _attackAngle = 100f;
    [SerializeField] private float _damage = 20f;
    [SerializeField] private float _attackInterval = 0.6f;
    [SerializeField] private GameObject _slashEffect;
    [SerializeField] private Transform _slashPoint;

    private float _attackTimer;
    private Coroutine _cameraReturnCoroutine;

    private void Awake()
    {
        if (_cameraSwitcher == null)
            _cameraSwitcher = FindFirstObjectByType<CameraSwitcher>();
    }

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
        if (_cameraSwitcher != null)
        {
            _cameraSwitcher.BeginMeleeView();
            if (_cameraReturnCoroutine != null) StopCoroutine(_cameraReturnCoroutine);
            _cameraReturnCoroutine = StartCoroutine(ReturnCameraAfterAttack());
        }

        if (_animator != null)
            _animator.SetTrigger("Knife");

        if (_slashEffect != null && _slashPoint != null)
        {
            Quaternion slashRotation = Quaternion.LookRotation(_slashPoint.forward, Vector3.up);
            GameObject slash = Instantiate(_slashEffect, _slashPoint.position, slashRotation);
            foreach (ParticleSystem particleSystem in slash.GetComponentsInChildren<ParticleSystem>(true))
                particleSystem.Play(true);
        }

        Vector3 attackOrigin = _slashPoint != null ? _slashPoint.position : transform.position;
        Vector3 attackForward = transform.forward;
        attackForward.y = 0f;
        attackForward.Normalize();

        Collider[] hitColliders = Physics.OverlapSphere(attackOrigin, _range);
        var damagedTargets = new HashSet<IDamageAble>();
        foreach (Collider hitCollider in hitColliders)
        {
            IDamageAble damageAble = hitCollider.GetComponentInParent<IDamageAble>();
            if (damageAble == null || !damagedTargets.Add(damageAble))
                continue;

            Component targetComponent = damageAble as Component;
            if (targetComponent != null &&
                (targetComponent.transform == transform || targetComponent.transform.IsChildOf(transform)))
            {
                damagedTargets.Remove(damageAble);
                continue;
            }

            Vector3 hitPoint = hitCollider.ClosestPoint(attackOrigin);
            Vector3 directionToTarget = hitPoint - attackOrigin;
            directionToTarget.y = 0f;

            if (directionToTarget.sqrMagnitude > _range * _range ||
                Vector3.Angle(attackForward, directionToTarget) > _attackAngle * 0.5f)
            {
                damagedTargets.Remove(damageAble);
                continue;
            }

            var damageInfo = new DamageInfo
            {
                Value = _damage,
                HitPoint = hitPoint,
                HitNormal = -directionToTarget.normalized
            };
            damageAble.TakeDamage(damageInfo);
        }
        
    }

    private System.Collections.IEnumerator ReturnCameraAfterAttack()
    {
        yield return new WaitForSeconds(_attackInterval);
        _cameraSwitcher.EndMeleeView();
        _cameraReturnCoroutine = null;
    }

    private void OnDisable()
    {
        if (_cameraReturnCoroutine != null)
        {
            StopCoroutine(_cameraReturnCoroutine);
            _cameraReturnCoroutine = null;
        }

        if (_cameraSwitcher != null) _cameraSwitcher.EndMeleeView();
    }
}

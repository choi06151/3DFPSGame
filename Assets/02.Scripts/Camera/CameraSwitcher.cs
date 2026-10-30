using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    private enum CameraView
    {
        FirstPerson,
        ThirdPerson,
        TopDown,
        QuarterView,
        SideView
    }

    [Header("References")]
    [SerializeField] private Transform _player;

    [SerializeField] private Transform _firstPersonPoint;
    [SerializeField] private CameraFollow _cameraFollow;
    [SerializeField] private CameraRotate _cameraRotate;

    [Header("Camera distances")]
    [SerializeField] private float _thirdPersonDistance = 4f;

    [SerializeField] private float _topDownHeight = 10f;
    [SerializeField] private float _quarterViewDistance = 9f;
    [SerializeField] private float _sideViewDistance = 8f;
    [SerializeField] private float _lookAtHeight = 1f;

    private CameraView _currentView;
    private CameraView _viewBeforeMelee;
    private bool _meleeViewActive;

    private void Awake()
    {
        if (_player == null)
        {
            var playerMove = FindFirstObjectByType<PlayerMove>();
            if (playerMove != null) _player = playerMove.transform;
        }

        if (_firstPersonPoint == null && _cameraFollow != null) _firstPersonPoint = _cameraFollow.Target;

        if (_cameraFollow == null) _cameraFollow = GetComponent<CameraFollow>();

        if (_cameraRotate == null) _cameraRotate = GetComponent<CameraRotate>();

        if (_player == null)
        {
            Debug.LogError("CameraSwitcher: 플레이어 Transform을 찾지 못했습니다.", this);
            enabled = false;
            return;
        }

        if (_firstPersonPoint == null)
        {
            Debug.LogError("CameraSwitcher: 1인칭 카메라 위치를 찾지 못했습니다.", this);
            enabled = false;
        }
    }

    private void Update()
    {
        if (!_meleeViewActive && Input.GetKeyDown(KeyCode.V))
        {
            _currentView = (CameraView)(((int)_currentView + 1) % System.Enum.GetValues(typeof(CameraView)).Length);
            ApplyView();
        }
    }

    public void BeginMeleeView()
    {
        if (!_meleeViewActive)
        {
            _viewBeforeMelee = _currentView;
            _meleeViewActive = true;
        }

        _currentView = CameraView.ThirdPerson;
        ApplyView();
    }

    public void EndMeleeView()
    {
        if (!_meleeViewActive) return;

        _currentView = _viewBeforeMelee;
        _meleeViewActive = false;
        ApplyView();
    }

    private void LateUpdate()
    {
        if (_currentView != CameraView.FirstPerson) ApplyViewTransform();
    }

    private void ApplyView()
    {
        var isFirstPerson = _currentView == CameraView.FirstPerson;

        if (_cameraFollow != null) _cameraFollow.enabled = isFirstPerson;

        if (_cameraRotate != null)
        {
            _cameraRotate.enabled = isFirstPerson;
            if (isFirstPerson) _cameraRotate.SyncYaw(_player.eulerAngles.y);
        }

        if (isFirstPerson)
            transform.position = _firstPersonPoint.position;
        else
            ApplyViewTransform();
    }

    private void ApplyViewTransform()
    {
        var lookAt = _player.position + Vector3.up * _lookAtHeight;

        switch (_currentView)
        {
            case CameraView.ThirdPerson:
                transform.position = _player.TransformPoint(new Vector3(0.65f, 1.5f, -_thirdPersonDistance));
                transform.LookAt(lookAt);
                break;

            case CameraView.TopDown:
                transform.position = _player.position + Vector3.up * _topDownHeight;
                transform.rotation = Quaternion.LookRotation(Vector3.down, _player.forward);
                break;

            case CameraView.QuarterView:
                transform.position = _player.position +
                                     _player.TransformDirection(new Vector3(1f, 0.8f, -1f).normalized *
                                                                _quarterViewDistance);
                transform.LookAt(lookAt);
                break;

            case CameraView.SideView:
                transform.position = _player.position +
                                     _player.TransformDirection(new Vector3(1f, 0.15f, 0f)) * _sideViewDistance;
                transform.LookAt(lookAt);
                break;
        }
    }
}

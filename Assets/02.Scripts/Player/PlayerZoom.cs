using System;
using UnityEngine;

public class PlayerZoom : MonoBehaviour
{
    
    private Player _player;
    [SerializeField]private GameObject _sniperZoom;
    [SerializeField] private float _normalPov = 60;
    [SerializeField] private float _sniperPov = 30;
private bool _zoomIn;
    private void Awake()
    {
        
        _player=GetComponent<Player>();
    }
    // Update is called once per frame
    void Update()
    {
        if (_player.WeaponMode == WeaponMode.Sniper && GameManager.Instance.GameState == GameState.Playing&&Input.GetMouseButton(1))
        {
            _zoomIn=true;
            Camera.main.fieldOfView = _sniperPov;
            _sniperZoom.SetActive(true);
        }
        else{
            _zoomIn=false;
            Camera.main.fieldOfView = _normalPov;
            _sniperZoom.SetActive(false);
        }
    }
}

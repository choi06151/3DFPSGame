using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerWeapons : MonoBehaviour
{
    [SerializeField]private Image _currentWeaponIndicator;
    [SerializeField]private Sprite _normalWeaponIcons;
    [SerializeField]private Sprite _sniperWeaponIcons;
  
    
    [SerializeField]private TextMeshProUGUI _remainAmmoText;
    [SerializeField]private TextMeshProUGUI _totalAmmoText;
    [SerializeField]private TextMeshProUGUI _bombCountText;

    
    private Player _player;
    private PlayerGunFire _playerGunFire;
    private PlayerBombFire _playerBombFire;

    public void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        _playerGunFire = _player.GetComponent<PlayerGunFire>();
        if (_playerGunFire == null)
            _playerGunFire = _player.GetComponentInChildren<PlayerGunFire>();
        _playerBombFire = _player.GetComponent<PlayerBombFire>();
        _player.OnSniperMode += ChangeToSniper;
        _player.OnNormalMode += ChangeToNormal;
        if (_playerGunFire != null)
        {
            _playerGunFire.OnAmmoChanged += UpdateAmmoText;
            UpdateAmmoText(_playerGunFire.AmmoInMagazine, _playerGunFire.MagazineSize);
        }

        if (_playerBombFire != null)
        {
            EnsureBombText();
            _playerBombFire.OnBombCountChanged += UpdateBombCountText;
            UpdateBombCountText(_playerBombFire.BombsRemaining, _playerBombFire.MaximumBombCount);
        }
    }

    private void OnDestroy()
    {
        if (_player != null)
        {
            _player.OnSniperMode -= ChangeToSniper;
            _player.OnNormalMode -= ChangeToNormal;
        }

        if (_playerGunFire != null)
            _playerGunFire.OnAmmoChanged -= UpdateAmmoText;
        if (_playerBombFire != null)
            _playerBombFire.OnBombCountChanged -= UpdateBombCountText;
    }

    private void UpdateAmmoText(int remainAmmo, int totalAmmo)
    {
        if (_remainAmmoText != null)
            _remainAmmoText.text = $"남은 탄약: {remainAmmo}";
        if (_totalAmmoText != null)
            _totalAmmoText.text = $"총 탄창: {totalAmmo}";
    }

    private void UpdateBombCountText(int remainBombs, int maximumBombs)
    {
        if (_bombCountText != null)
            _bombCountText.text = $"폭탄: {remainBombs}/{maximumBombs}";
    }

    private void EnsureBombText()
    {
        if (_bombCountText != null)
            return;

        var canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
            return;

        var textObject = new GameObject("BombCountText", typeof(RectTransform), typeof(TextMeshProUGUI));
        textObject.transform.SetParent(canvas.transform, false);
        var rect = textObject.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(1f, 0f);
        rect.anchorMax = new Vector2(1f, 0f);
        rect.pivot = new Vector2(1f, 0f);
        rect.anchoredPosition = new Vector2(-24f, 78f);
        rect.sizeDelta = new Vector2(220f, 42f);

        _bombCountText = textObject.GetComponent<TextMeshProUGUI>();
        _bombCountText.font = TMP_Settings.defaultFontAsset;
        _bombCountText.fontSize = 20f;
        _bombCountText.alignment = TextAlignmentOptions.Right;
        _bombCountText.color = Color.white;
    }
    
    

    public void ChangeToNormal()
    {
        _currentWeaponIndicator.sprite = _normalWeaponIcons;
    }

    public void ChangeToSniper()
    {
        _currentWeaponIndicator.sprite = _sniperWeaponIcons;
    }
}

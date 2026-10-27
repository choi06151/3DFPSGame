using System;
using TMPro;
using UnityEngine;

public class RemainEnemyText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text; 
    [SerializeField] private int _remainEnemyCount=0;


    private void Awake()
    {
        _remainEnemyCount = 0;
        var zombies=FindObjectsByType<Zombie>(FindObjectsSortMode.None);

        foreach (var zombie in zombies)
            RegisterZombie(zombie);

        RefreshText();
    }

    public void RegisterZombie(Zombie zombie)
    {
        if (zombie == null) return;

        _remainEnemyCount++;
        zombie.OnDestroy += HandleZombieDestroyed;
        RefreshText();
    }
    
    private void HandleZombieDestroyed()
    {
        _remainEnemyCount = Mathf.Max(0, _remainEnemyCount - 1);
        RefreshText();
    }

    private void RefreshText()
    {
        if (_text != null)
            _text.text = $"남은 적 : {_remainEnemyCount}";
    }
}

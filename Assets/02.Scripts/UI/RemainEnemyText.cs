using System;
using TMPro;
using UnityEngine;

public class RemainEnemyText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text; 
    [SerializeField] private int _remainEnemyCount=0;


    private void Awake()
    {

        var zombies=FindObjectsByType<Zombie>(FindObjectsSortMode.None);

        foreach (var zombie in zombies)
        {
            _remainEnemyCount++;
            zombie.OnDestroy += RefreshText;
        }
        _text.text="남은 적 : "+$"{_remainEnemyCount}";

    }
    
    private void RefreshText()
    {
        _remainEnemyCount--;
        _text.text="남은 적 : "+$"{_remainEnemyCount}";
    }
}

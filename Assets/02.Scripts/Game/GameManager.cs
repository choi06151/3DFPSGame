using System;
using System.Collections;
using UnityEngine;

public enum GameState
{
    Ready,
    Playing,
    GameOver
}
public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance => _instance;

    private GameState _gameState=GameState.Ready;
    public GameState GameState=>_gameState;
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(StartSequence_Coroutine());
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator StartSequence_Coroutine()
    {
        yield return new WaitForSeconds(0.5f);
        _gameState = GameState.Playing;
    }
}

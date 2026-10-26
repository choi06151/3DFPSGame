using System;
using System.Collections;
using TMPro;
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
    
    [SerializeField]private TextMeshProUGUI _gameText;
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
        _gameText.text = "준비";
        yield return new WaitForSeconds(0.5f);
        _gameText.text = "시작";
        yield return new WaitForSeconds(0.5f);
        _gameText.gameObject.SetActive(false);
        _gameState = GameState.Playing;
    }


    public void GameOver()
    {
        _gameState = GameState.GameOver;
        _gameText.text = "게임오버";
        _gameText.color = Color.red;
    }
}

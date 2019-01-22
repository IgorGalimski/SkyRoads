using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] 
    private int _asteroidPassedScore = 5;
    
    private int _seconds;

    private bool _boost;

    private int _score;
    
    private void Awake()
    {
        MessageSystemManager.AddListener<IMessageData>(MessageType.OnAsteroidCollision, OnAsteroidCollision);
        MessageSystemManager.AddListener<IMessageData>(MessageType.OnAsteroidPassed, OnAsteroidPassed);
        MessageSystemManager.AddListener<PlayerBoostStatus>(MessageType.OnPlayerBoostStatusChange, OnPlayerBoostStatusChange);

        StartGame();
    }

    private void OnDestroy()
    {
        MessageSystemManager.RemoveListener<IMessageData>(MessageType.OnAsteroidCollision, OnAsteroidCollision);
        MessageSystemManager.RemoveListener<IMessageData>(MessageType.OnAsteroidPassed, OnAsteroidPassed);
        MessageSystemManager.RemoveListener<PlayerBoostStatus>(MessageType.OnPlayerBoostStatusChange, OnPlayerBoostStatusChange);
    }

    private void OnAsteroidCollision(IMessageData messageData)
    {
        Debug.LogError("FAIL");
    }

    private void OnAsteroidPassed(IMessageData messageData)
    {
        _score += _asteroidPassedScore;
    }

    private void OnPlayerBoostStatusChange(PlayerBoostStatus playerBoostStatus)
    {
        _boost = playerBoostStatus.BoostStatus;
    }

    private void StartGame()
    {
        _seconds = 0;
        _score = 0;
        
        InvokeRepeating("Timer", 0.0f, 1.0f);
    }

    private void Timer()
    {
        _seconds++;
        
        MessageSystemManager.Invoke(MessageType.OnPlayingTimeUpdate, new TimeData(_seconds));
        
        if (_boost)
        {
            _score += 2;
        }
        else
        {
            _score++;
        }
        
        MessageSystemManager.Invoke(MessageType.OnScoreUpdate, new ScoreData(_score));
    }
}

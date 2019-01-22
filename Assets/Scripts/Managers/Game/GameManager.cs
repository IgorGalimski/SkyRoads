using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int _seconds;
    
    private void Awake()
    {
        MessageSystemManager.AddListener<IMessageData>(MessageType.OnAsteroidCollision, OnAsteroidCollision);
        //MessageSystemManager.AddListener<ScoreData>(MessageType.OnScoreUpdate, OnScoreUpdate);

        StartGame();
    }

    private void OnAsteroidCollision(IMessageData messageData)
    {
        Debug.LogError("FAIL");
    }

    private void StartGame()
    {
        _seconds = 0;
        
        InvokeRepeating("Timer", 0.0f, 1.0f);
    }

    private void Timer()
    {
        _seconds++;
        
        MessageSystemManager.Invoke(MessageType.OnPlayingTimeUpdate, new TimeData(_seconds));
    }
}

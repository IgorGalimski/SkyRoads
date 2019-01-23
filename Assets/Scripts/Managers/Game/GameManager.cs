﻿using UnityEngine;

public class GameManager : MonoBehaviour
{
    private const string BEST_SCORE_PREFS_KEY = "BestScore";
    
    [SerializeField] 
    private int _asteroidPassedScore = 5;
    
    private int _seconds;

    private bool _boost;

    private int _score;

    private int _asteroidPassed;

    private int _playingTime;
    
    private void Awake()
    {
        MessageSystemManager.AddListener<IMessageData>(MessageType.OnAsteroidCollision, OnAsteroidCollision);
        MessageSystemManager.AddListener<IMessageData>(MessageType.OnAsteroidPassed, OnAsteroidPassed);
        MessageSystemManager.AddListener<TimeData>(MessageType.OnPlayingTimeUpdate, OnPlayingTimeUpdate);
        MessageSystemManager.AddListener<PlayerBoostStatus>(MessageType.OnPlayerBoostStatusChange, OnPlayerBoostStatusChange);

        StartGame();
    }

    private void OnDestroy()
    {
        MessageSystemManager.RemoveListener<IMessageData>(MessageType.OnAsteroidCollision, OnAsteroidCollision);
        MessageSystemManager.RemoveListener<IMessageData>(MessageType.OnAsteroidPassed, OnAsteroidPassed);
        MessageSystemManager.RemoveListener<TimeData>(MessageType.OnPlayingTimeUpdate, OnPlayingTimeUpdate);
        MessageSystemManager.RemoveListener<PlayerBoostStatus>(MessageType.OnPlayerBoostStatusChange, OnPlayerBoostStatusChange);
    }
    
    public int BestScore
    {
        get { return PlayerPrefs.GetInt(BEST_SCORE_PREFS_KEY); }
        set { PlayerPrefs.SetInt(BEST_SCORE_PREFS_KEY, value); }
    }

    private void OnAsteroidCollision(IMessageData messageData)
    {
        TimeData timeData = new TimeData(_playingTime);
        AsteroidPassedData asteroidPassedData = new AsteroidPassedData(_asteroidPassed);
        ScoreData scoreData = new ScoreData(_score, BestScore);
        
        bool record = BestScore == _score;

        LevelFailData levelFailData = new LevelFailData(timeData, asteroidPassedData, scoreData, record);
        
        MessageSystemManager.Invoke(MessageType.OnGameFail, levelFailData);
    }

    private void OnAsteroidPassed(IMessageData messageData)
    {
        _score += _asteroidPassedScore;

        _asteroidPassed++;
    }

    private void OnPlayingTimeUpdate(TimeData timeData)
    {
        _playingTime = timeData.Seconds;
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
        
        if (BestScore < _score)
        {
            BestScore = _score;
        }
        
        MessageSystemManager.Invoke(MessageType.OnScoreUpdate, new ScoreData(_score, BestScore));
    }
}

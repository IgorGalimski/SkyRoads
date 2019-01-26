using UnityEngine;

public class GameManager : BaseSingletonManager
{
    private const string BEST_SCORE_PREFS_KEY = "BestScore";
    
    [SerializeField] 
    private int _asteroidPassedScore = 5;
    
    private int _seconds;

    private bool _boost;

    private int _score;

    private int _asteroidPassed;

    private int _playingTime;

    private bool _record;

    protected override void Init()
    {
        MessageSystemManager.AddListener(MessageType.OnAsteroidCollision, OnAsteroidCollision);
        MessageSystemManager.AddListener<AsteroidPassedData>(MessageType.OnAsteroidPassed, OnAsteroidPassed);
        MessageSystemManager.AddListener<TimeData>(MessageType.OnPlayingTimeUpdate, OnPlayingTimeUpdate);
        MessageSystemManager.AddListener<PlayerBoostStatus>(MessageType.OnPlayerBoostStatusChange, OnPlayerBoostStatusChange);
        MessageSystemManager.AddListener(MessageType.OnGameLoad, OnGameLoad);
        MessageSystemManager.AddListener(MessageType.OnGameFail, OnGameFail);
    }

    private void OnDestroy()
    {
        MessageSystemManager.RemoveListener(MessageType.OnAsteroidCollision, OnAsteroidCollision);
        MessageSystemManager.RemoveListener<AsteroidPassedData>(MessageType.OnAsteroidPassed, OnAsteroidPassed);
        MessageSystemManager.RemoveListener<TimeData>(MessageType.OnPlayingTimeUpdate, OnPlayingTimeUpdate);
        MessageSystemManager.RemoveListener<PlayerBoostStatus>(MessageType.OnPlayerBoostStatusChange, OnPlayerBoostStatusChange);
        MessageSystemManager.RemoveListener(MessageType.OnGameLoad, OnGameLoad);
        MessageSystemManager.RemoveListener(MessageType.OnGameFail, OnGameFail);
    }
    
    private int BestScore
    {
        get { return PlayerPrefs.GetInt(BEST_SCORE_PREFS_KEY); }
        set { PlayerPrefs.SetInt(BEST_SCORE_PREFS_KEY, value); }
    }

    private void OnAsteroidCollision()
    {
        TimeData timeData = new TimeData(_playingTime);
        AsteroidPassedData asteroidPassedData = new AsteroidPassedData(_asteroidPassed);
        ScoreData scoreData = new ScoreData(_score, BestScore);

        LevelFailData levelFailData = new LevelFailData(timeData, asteroidPassedData, scoreData, _record);
        
        MessageSystemManager.Invoke(MessageType.OnGameFail, levelFailData);
    }

    private void OnAsteroidPassed(AsteroidPassedData asteroidPassedData)
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

    private void OnGameLoad()
    {
        _seconds = 0;
        _score = 0;
        _record = false;
        
        InvokeRepeating("Timer", 0.0f, 1.0f);
    }

    private void OnGameFail()
    {
        CancelInvoke("Timer");
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

            _record = true;
        }
        
        MessageSystemManager.Invoke(MessageType.OnScoreUpdate, new ScoreData(_score, BestScore));
    }
}

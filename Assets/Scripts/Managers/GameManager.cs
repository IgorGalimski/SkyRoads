using SpaceShooter.MessageSystem;
using SpaceShooter.MessageSystem.Data;
using UnityEngine;

namespace SpaceShooter.Managers
{
    public class GameManager : BaseManager
    {
        private const string BEST_SCORE_PREFS_KEY = "BestScore";

        private const int ASTEROID_PASSED_SCORE = 5;

        private int _seconds;

        private bool _boost;

        private int _score;

        private AsteroidPassedData _asteroidPassedData = new AsteroidPassedData();

        private TimeData _timeData = new TimeData();

        private int _playingTime;

        private bool _record;

        protected override void Init()
        {
            MessageSystemManager.AddListener(MessageType.OnAsteroidCollision, OnAsteroidCollision);
            MessageSystemManager.AddListener<AsteroidPassedData>(MessageType.OnAsteroidPassed, OnAsteroidPassed);
            MessageSystemManager.AddListener<PlayerBoostStatus>(MessageType.OnPlayerBoostStatusChange,
                OnPlayerBoostStatusChange);
            MessageSystemManager.AddListener(MessageType.OnGameLoad, OnGameLoad);
            MessageSystemManager.AddListener(MessageType.OnGameFail, OnGameFail);
        }

        private void OnDestroy()
        {
            MessageSystemManager.RemoveListener(MessageType.OnAsteroidCollision, OnAsteroidCollision);
            MessageSystemManager.RemoveListener<AsteroidPassedData>(MessageType.OnAsteroidPassed, OnAsteroidPassed);
            MessageSystemManager.RemoveListener<PlayerBoostStatus>(MessageType.OnPlayerBoostStatusChange,
                OnPlayerBoostStatusChange);
            MessageSystemManager.RemoveListener(MessageType.OnGameLoad, OnGameLoad);
            MessageSystemManager.RemoveListener(MessageType.OnGameFail, OnGameFail);
        }

        private int BestScore
        {
            get => PlayerPrefs.GetInt(BEST_SCORE_PREFS_KEY);
            set => PlayerPrefs.SetInt(BEST_SCORE_PREFS_KEY, value);
        }

        private void OnAsteroidCollision()
        {
            ScoreData scoreData = new ScoreData(_score, BestScore);

            LevelFailData levelFailData = new LevelFailData(_timeData, _asteroidPassedData, scoreData, _record);

            MessageSystemManager.Invoke(MessageType.OnGameFail, levelFailData);
        }

        private void OnAsteroidPassed(AsteroidPassedData asteroidPassedData)
        {
            _score += ASTEROID_PASSED_SCORE;

            _asteroidPassedData = asteroidPassedData;
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

            InvokeRepeating(nameof(Timer), 0.0f, 1.0f);
        }

        private void OnGameFail()
        {
            CancelInvoke(nameof(Timer));
        }

        private void Timer()
        {
            _seconds++;
            _timeData.Seconds = _seconds;
            
            MessageSystemManager.Invoke(MessageType.OnPlayingTimeUpdate, _timeData);

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
}

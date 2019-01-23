using TMPro;
using UnityEngine;

namespace DefaultNamespace.Managers
{
    public class CountersManager : BaseSingletonManager
    {
        private const string CURRENT_SCORE_FORMAT = "Score: {0}";
        private const string BEST_SCORE_FORMAT = "Best score: {0}";
        
        [SerializeField] 
        private TextMeshProUGUI _timePlaying;

        [SerializeField] 
        private TextMeshProUGUI _asteroids;

        [SerializeField] 
        private TextMeshProUGUI _currentScore;

        [SerializeField] 
        private TextMeshProUGUI _bestScore;

        protected override void Init()
        {
            if (_timePlaying != null)
            {
                MessageSystemManager.AddListener<TimeData>(MessageType.OnPlayingTimeUpdate, OnPlayingTimeUpdate);
            }

            if (_asteroids != null)
            {
                MessageSystemManager.AddListener<AsteroidPassedData>(MessageType.OnAsteroidPassed, OnAsteroidPassed);
            }

            if (_currentScore != null && _bestScore != null)
            {
                MessageSystemManager.AddListener<ScoreData>(MessageType.OnScoreUpdate, OnScoreUpdate);
            }
        }

        private void OnDestroy()
        {
            if (_timePlaying != null)
            {
                MessageSystemManager.RemoveListener<TimeData>(MessageType.OnPlayingTimeUpdate, OnPlayingTimeUpdate);
            }

            if (_asteroids != null)
            {
                MessageSystemManager.RemoveListener<AsteroidPassedData>(MessageType.OnAsteroidPassed, OnAsteroidPassed);
            }

            if (_currentScore != null && _bestScore != null)
            {
                MessageSystemManager.RemoveListener<ScoreData>(MessageType.OnScoreUpdate, OnScoreUpdate);
            }
        }

        private void OnPlayingTimeUpdate(TimeData timeData)
        {
            _timePlaying.text = timeData.GetFormattedTime();
        }

        private void OnAsteroidPassed(AsteroidPassedData asteroidPassedData)
        {
            _asteroids.text = asteroidPassedData.GetFormattedCount();
        }

        private void OnScoreUpdate(ScoreData scoreData)
        {
            _currentScore.text = string.Format(CURRENT_SCORE_FORMAT, scoreData.CurrentScore);
            _bestScore.text = string.Format(BEST_SCORE_FORMAT, scoreData.BestScore);
        }
    }
}
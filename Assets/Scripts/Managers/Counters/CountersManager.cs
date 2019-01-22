using TMPro;
using UnityEngine;

namespace DefaultNamespace.Managers
{
    public class CountersManager : MonoBehaviour
    {
        private const string BEST_SCORE_PREFS_KEY = "BestScore";
        
        private const string TIME_PLAYING_FORMAT = "{0}:{1}:{2}";
        private const string ASTEROIDS_PASSED_FORMAT = "Asteroids: {0}";
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

        public int BestScore
        {
            get { return PlayerPrefs.GetInt(BEST_SCORE_PREFS_KEY); }
            set { PlayerPrefs.SetInt(BEST_SCORE_PREFS_KEY, value); }
        }

        private void Awake()
        {
            if (_timePlaying != null)
            {
                MessageSystemManager.AddListener<TimeData>(MessageType.OnPlayingTimeUpdate, OnPlayingTimeUpdate);
            }

            if (_asteroids != null)
            {
                MessageSystemManager.AddListener<AsteroidPassedData>(MessageType.OnAsteroidPassed, OnAsteroidPassed);
            }

            if (_currentScore != null)
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

            if (_currentScore != null)
            {
                MessageSystemManager.RemoveListener<ScoreData>(MessageType.OnScoreUpdate, OnScoreUpdate);
            }
        }

        private void OnPlayingTimeUpdate(TimeData timeData)
        {
            int seconds = timeData.Seconds % 60; 
            int minutes = (timeData.Seconds / 60) % 60; 
            int hours = timeData.Seconds / 3600;

            _timePlaying.text = string.Format(TIME_PLAYING_FORMAT, hours, minutes, seconds);
        }

        private void OnAsteroidPassed(AsteroidPassedData asteroidPassedData)
        {
            _asteroids.text = string.Format(ASTEROIDS_PASSED_FORMAT, asteroidPassedData.Count);
        }

        private void OnScoreUpdate(ScoreData scoreData)
        {
            int currentScore = scoreData.Score;
            
            _currentScore.text = string.Format(CURRENT_SCORE_FORMAT, currentScore);

            if (BestScore < currentScore)
            {
                BestScore = currentScore;
            }
            
            if (_bestScore != null)
            {
                _bestScore.text = string.Format(BEST_SCORE_FORMAT, BestScore);
            }
        }
    }
}
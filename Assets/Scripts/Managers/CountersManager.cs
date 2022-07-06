using UnityEngine;

namespace DefaultNamespace.Managers
{
    [RequireComponent(typeof(CounterView))]
    public class CountersManager : MonoBehaviour
    {
        [SerializeField] 
        private CounterView _counterView;

        private void Awake()
        {
            MessageSystemManager.AddListener<TimeData>(MessageType.OnPlayingTimeUpdate, OnPlayingTimeUpdate);
            MessageSystemManager.AddListener<AsteroidPassedData>(MessageType.OnAsteroidPassed, OnAsteroidPassed);
            MessageSystemManager.AddListener<ScoreData>(MessageType.OnScoreUpdate, OnScoreUpdate);

            MessageSystemManager.AddListener(MessageType.OnGameFail, OnGameFail);
            
            _counterView.ResetValues();
        }

        private void OnDestroy()
        {
            MessageSystemManager.RemoveListener<TimeData>(MessageType.OnPlayingTimeUpdate, OnPlayingTimeUpdate);
            MessageSystemManager.RemoveListener<AsteroidPassedData>(MessageType.OnAsteroidPassed, OnAsteroidPassed);
            MessageSystemManager.RemoveListener<ScoreData>(MessageType.OnScoreUpdate, OnScoreUpdate);

            MessageSystemManager.RemoveListener(MessageType.OnGameFail, OnGameFail);
        }

        private void OnPlayingTimeUpdate(TimeData timeData)
        {
            _counterView.UpdatePlayingTime(timeData.GetFormattedTime());
        }

        private void OnAsteroidPassed(AsteroidPassedData asteroidPassedData)
        {
            _counterView.UpdateAsteroidsCount(asteroidPassedData.GetFormattedCount());
        }

        private void OnScoreUpdate(ScoreData scoreData)
        {
            _counterView.UpdateScore(scoreData.GetFormattedCurrentScore(), scoreData.GetFormattedBestScore());
        }

        private void OnGameFail()
        {
            gameObject.SetActive(false);
        }
    }
}
using TMPro;
using UnityEngine;

namespace DefaultNamespace.Managers
{
    public class Counters : MonoBehaviour
    {
        private const string TIME_PLAYING_FORMAT = "{0}:{1}:{2}";
        
        [SerializeField] 
        private TextMeshProUGUI _timePlaying;

        [SerializeField] 
        private TextMeshProUGUI _asteroids;

        [SerializeField] 
        private TextMeshProUGUI _currentScore;

        [SerializeField] 
        private TextMeshProUGUI _bestScore;

        private void Awake()
        {
            if (_timePlaying != null)
            {
                MessageSystemManager.AddListener<TimeData>(MessageType.OnPlayingTimeUpdate, OnPlayingTimeUpdate);
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
        }

        private void OnPlayingTimeUpdate(TimeData timeData)
        {
            int seconds = timeData.Seconds % 60; 
            int minutes = (timeData.Seconds / 60) % 60; 
            int hours = timeData.Seconds / 3600;

            _timePlaying.text = string.Format(TIME_PLAYING_FORMAT, hours, minutes, seconds);
        }

        private void OnScoreUpdate(ScoreData scoreData)
        {
            
        }
    }
}
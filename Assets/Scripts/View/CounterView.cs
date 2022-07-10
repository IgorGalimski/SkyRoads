using TMPro;
using UnityEngine;

namespace SpaceShooter.View
{
    public class CounterView : MonoBehaviour
    {
        private const string DEFAULT_TEXT_VALUE = "-";
        
        [SerializeField] 
        private TextMeshProUGUI _timePlaying;

        [SerializeField] 
        private TextMeshProUGUI _asteroids;

        [SerializeField] 
        private TextMeshProUGUI _currentScore;

        [SerializeField] 
        private TextMeshProUGUI _bestScore;

        public void ResetValues()
        {
            _timePlaying.text = DEFAULT_TEXT_VALUE;
            _asteroids.text = DEFAULT_TEXT_VALUE;
            _currentScore.text = DEFAULT_TEXT_VALUE;
            _bestScore.text = DEFAULT_TEXT_VALUE;
        }

        public void UpdatePlayingTime(string formattedTime)
        {
            _timePlaying.text = formattedTime;
        }

        public void UpdateAsteroidsCount(string formattedAsteroids)
        {
            _asteroids.text = formattedAsteroids;
        }

        public void UpdateScore(string formattedCurrentScore, string formattedBestScore)
        {
            _currentScore.text = formattedCurrentScore;
            _bestScore.text = formattedBestScore;
        }
    }
}
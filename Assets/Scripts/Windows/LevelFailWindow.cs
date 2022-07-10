using SpaceShooter.MessageSystem;
using SpaceShooter.MessageSystem.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceShooter.Windows
{
    [RequireComponent(typeof(Animator))]
    public class LevelFailWindow : MonoBehaviour
    {
        private static readonly int ShowTriggerName = Animator.StringToHash(SHOW_TRIGGER_NAME);
        private const string SHOW_TRIGGER_NAME = "Show";

        [SerializeField] private TextMeshProUGUI _playingTime;

        [SerializeField] private TextMeshProUGUI _asteroidsCount;

        [SerializeField] private TextMeshProUGUI _finalScore;

        [SerializeField] private TextMeshProUGUI _congratulations;

        [SerializeField] private Button _replayButton;

        private void Awake()
        {
            MessageSystemManager.AddListener<LevelFailData>(MessageType.OnGameFail, OnGameFail);

            if (_replayButton != null)
            {
                _replayButton.onClick.AddListener(OnReplayButtonClick);
            }
            else
            {
                Debug.LogWarning("ReplayButton is null");
            }
        }

        private void OnDestroy()
        {
            MessageSystemManager.RemoveListener<LevelFailData>(MessageType.OnGameFail, OnGameFail);

            if (_replayButton != null)
            {
                _replayButton.onClick.RemoveListener(OnReplayButtonClick);
            }
        }

        private void OnGameFail(LevelFailData levelFailData)
        {
            if (_playingTime != null)
            {
                _playingTime.text = levelFailData.Duration.GetFormattedTime();
            }
            else
            {
                Debug.LogWarning("Playing time is null");
            }

            if (_asteroidsCount != null)
            {
                _asteroidsCount.text = levelFailData.Asteroids.GetFormattedCount();
            }
            else
            {
                Debug.LogWarning("Asteroids count is null");
            }

            if (_finalScore != null)
            {
                _finalScore.text = levelFailData.Score.GetFormattedFinalScore();
            }
            else
            {
                Debug.LogWarning("Current score is null");
            }

            if (_congratulations != null)
            {
                _congratulations.enabled = levelFailData.Record;
            }
            else
            {
                Debug.LogWarning("Congratulations is null");
            }

            GetComponent<Animator>().SetTrigger(ShowTriggerName);
        }

        private void OnReplayButtonClick()
        {
            _replayButton.onClick.RemoveListener(OnReplayButtonClick);

            MessageSystemManager.Invoke(MessageType.OnGameReplay);
        }
    }
}

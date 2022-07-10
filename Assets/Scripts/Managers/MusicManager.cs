using SpaceShooter.Data;
using SpaceShooter.MessageSystem;
using SpaceShooter.MessageSystem.Data;
using UnityEngine;

namespace SpaceShooter.Managers
{
    public class MusicManager : BaseManager
    {
        [SerializeField] 
        private AudioSource _backgroundAudioSource;

        [SerializeField] 
        private AudioSource _effectsAudioSource;

        [SerializeField] 
        private SoundsScriptableObject _sounds;
        
        protected override void Init()
        {
            _backgroundAudioSource.clip = _sounds.BackgroundSound;
            _backgroundAudioSource.Play();
            
            MessageSystemManager.AddListener(MessageType.OnPlayerBoostCollide, OnPlayerBoost);
            MessageSystemManager.AddListener(MessageType.OnGameFail, OnGameFail);
        }

        public void OnDestroy()
        {
            MessageSystemManager.RemoveListener(MessageType.OnPlayerBoostCollide, OnPlayerBoost);
            MessageSystemManager.RemoveListener(MessageType.OnGameFail, OnGameFail);
        }

        private void OnPlayerBoost()
        {
            SetEffectSoundAndPlay(_sounds.BoosterActivatedSound);
        }

        private void OnGameFail()
        {
            SetEffectSoundAndPlay(_sounds.GameFailSound);
        }

        private void SetEffectSoundAndPlay(AudioClip audioClip)
        {
            _effectsAudioSource.clip = audioClip;
            _effectsAudioSource.Play();
        }
    }
}
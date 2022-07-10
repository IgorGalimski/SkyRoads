using UnityEngine;

namespace SpaceShooter.Data
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SoundsScriptableObject", order = 1)]
    public class SoundsScriptableObject : ScriptableObject
    {
        [SerializeField] 
        private AudioClip _backgroundSound;

        [SerializeField] 
        private AudioClip _gameFailSound;

        [SerializeField] 
        private AudioClip _boosterActivatedSound;

        public AudioClip BackgroundSound => _backgroundSound;

        public AudioClip GameFailSound => _gameFailSound;
        
        public AudioClip BoosterActivatedSound => _boosterActivatedSound;
    }
}
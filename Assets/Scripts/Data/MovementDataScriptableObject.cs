using System;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/MovementDataScriptableObject", order = 1)]
    public class MovementDataScriptableObject : ScriptableObject
    {
        [SerializeField] 
        private float _xMoveSpeed = 5f;
    
        [SerializeField]
        private float _yMoveSpeed = 5f;

        [SerializeField]
        private float _xMin = -5f;
    
        [SerializeField]
        private float _xMax = 5f;

        [SerializeField] 
        private float _maxYDeflectionAngle = 60f;

        [SerializeField]
        private float _boostMultiplier = 2f;

        [SerializeField] 
        private ParticleEffect[] _particleEffects;
        
        [Serializable]
        public struct ParticleEffect
        {
            [SerializeField]
            private ParticleSystem _particleSystem;

            [SerializeField]
            private bool _play;
        
            public ParticleSystem ParticleSystem => _particleSystem;
            public bool Play => _play;
        }

        public float XMoveSpeed => _xMoveSpeed;

        public float YMoveSpeed => _yMoveSpeed;

        public float XMin => _xMin;

        public float XMax => _xMax;

        public float MaxYDeflectionAngle => _maxYDeflectionAngle;

        public float BoostMultiplier => _boostMultiplier;

        public ParticleEffect[] ParticleEffects => _particleEffects;
    }
}
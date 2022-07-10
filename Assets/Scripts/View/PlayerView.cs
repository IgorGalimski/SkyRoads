using System;
using UnityEngine;

namespace SpaceShooter.View
{
    [RequireComponent(typeof(Collider))]
    public class PlayerView : MonoBehaviour
    {
        [SerializeField] 
        private ParticleEffect[] _particleEffects;
        
        [Serializable]
        private struct ParticleEffect
        {
            [SerializeField]
            private ParticleSystem _particleSystem;

            [SerializeField]
            private bool _play;
        
            public ParticleSystem ParticleSystem => _particleSystem;
            
            public bool Play => _play;
        }

        public Vector3 Position => transform.position;

        public Quaternion Rotation => transform.rotation;

        public void AddPosition(Vector3 delta)
        {
            transform.position += delta;
        }

        public void UpdatePosition(Vector3 position)
        {
            transform.position = position;
        }

        public void UpdateRotation(Quaternion rotation)
        {
            transform.rotation = rotation;
        }

        public void PlayDestroyEffect()
        {
            if (_particleEffects == null)
            {
                return;
            }

            foreach (ParticleEffect particleEffect in _particleEffects)
            {
                if (particleEffect.ParticleSystem == null)
                {
                    Debug.LogWarning("ParticleEffect is null");
                
                    continue;
                }

                if (particleEffect.Play)
                {
                    particleEffect.ParticleSystem.Play();
                }
                else
                {
                    particleEffect.ParticleSystem.Stop();
                }
            }
        }
    }
}
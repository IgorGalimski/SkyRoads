using UnityEngine;

namespace SpaceShooter.Components
{
    public class Booster : BaseCollisionComponent
    {
        [SerializeField] 
        private ParticleSystem _destroyEffect;

        [SerializeField] 
        private MeshRenderer _mesh;

        public void Awake()
        {
            _destroyEffect.gameObject.SetActive(false);
        }

        protected override void OnCollisionHandler()
        {
            _destroyEffect.gameObject.SetActive(true);
            
            Invoke(nameof(DisableEffect), 0.2f);

            _mesh.enabled = false;
        }

        private void DisableEffect()
        {
            _mesh.enabled = true;
            
            _destroyEffect.Stop();
        }
    }
}
using SpaceShooter.Data;
using UnityEngine;

namespace SpaceShooter.Components
{
    [RequireComponent(typeof(Collider))]
    public class Asteroid : BaseCollisionComponent
    {
        [SerializeField] 
        private Vector3 _rotationDirection = Vector3.up;

        [SerializeField] 
        private float _speed = 1f;

        [SerializeField] 
        private MeshRenderer _meshRenderer;

        [SerializeField] 
        private AsteroidMaterialsScriptableObject _materials;

        private Collider _collider;

        public void Awake()
        {
            _collider = GetComponent<Collider>();
        }

        public void Rotate()
        {
            transform.Rotate(_rotationDirection * _speed * Time.deltaTime);
        }

        public void SetBoostStatus(bool boostEnabled)
        {
            _collider.enabled = !boostEnabled;

            _meshRenderer.material = boostEnabled 
                ? _materials.BoosterEnabledMaterial 
                : _materials.StandardMaterial;
        }
    }
}

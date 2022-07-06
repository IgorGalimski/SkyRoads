using System;
using UnityEngine;

namespace Components
{
    [RequireComponent(typeof(Collider))]
    public abstract class BaseCollisionComponent : MonoBehaviour
    {
        public event Action OnCollision;
        
        private void OnTriggerEnter(Collider other)
        {
            OnCollision?.Invoke();
        }
    }
}
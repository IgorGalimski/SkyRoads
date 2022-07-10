using System;
using UnityEngine;

namespace SpaceShooter.Components
{
    [RequireComponent(typeof(Collider))]
    public abstract class BaseCollisionComponent : MonoBehaviour
    {
        public event Action OnCollision;

        protected virtual void OnCollisionHandler()
        {
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            OnCollisionHandler();
            
            OnCollision?.Invoke();
        }
    }
}
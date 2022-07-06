using System.Collections.Generic;
using Components;
using UnityEngine;

namespace DefaultNamespace.Managers
{
    [RequireComponent(typeof(ObjectPool))]
    public class BoosterManager : MonoBehaviour
    {
        private ObjectPool _objectPool;

        private List<Booster> _boosters = new List<Booster>();
        
        public void Awake()
        {
            _objectPool = GetComponent<ObjectPool>();
            if (_objectPool.IsInitialized)
            {
                OnInit();
            }
            else
            {
                _objectPool.OnInit += OnInit;
            }
        }

        private void OnInit()
        {
            foreach (var gameObject in _objectPool.Instances)
            {
                var booster = gameObject?.GetComponent<Booster>();
                if (booster != null)
                {
                    _boosters.Add(booster);
                    booster.OnCollision += OnCollision;
                }
            }
        }

        private void OnCollision()
        {
            MessageSystemManager.Invoke(MessageType.OnPlayerBoostCollide);
        }

        private void OnDestroy()
        {
            foreach (var booster in _boosters)
            {
                booster.OnCollision -= OnCollision;
            }
        }
    }
}
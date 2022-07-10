using System.Collections.Generic;
using SpaceShooter.Components;
using SpaceShooter.MessageSystem;
using SpaceShooter.MessageSystem.Data;
using SpaceShooter.View;
using UnityEngine;

namespace SpaceShooter.Managers
{
    [RequireComponent(typeof(ObjectPool.ObjectPool))]
    public class BoosterManager : MonoBehaviour
    {
        [SerializeField] 
        private BoosterView _boosterView;
        
        private ObjectPool.ObjectPool _objectPool;

        private List<Booster> _boosters = new List<Booster>();
        
        public void Awake()
        {
            _objectPool = GetComponent<ObjectPool.ObjectPool>();
            if (_objectPool.IsInitialized)
            {
                OnInit();
            }
            else
            {
                _objectPool.OnInit += OnInit;
            }
            
            MessageSystemManager.AddListener<BoostFillData>(MessageType.OnPlayerBoostFillChange, OnPlayerBoostFillChange);
            MessageSystemManager.AddListener(MessageType.OnGameFail, OnGameFail);
        }

        private void OnGameFail()
        {
            _boosterView.SetBoosterViewFill(0f);
        }

        private void OnPlayerBoostFillChange(BoostFillData boostFillData)
        {
            _boosterView.SetBoosterViewFill(boostFillData.Fill);
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
            
            MessageSystemManager.RemoveListener<BoostFillData>(MessageType.OnPlayerBoostFillChange, OnPlayerBoostFillChange);
            MessageSystemManager.RemoveListener(MessageType.OnGameFail, OnGameFail);
        }
    }
}
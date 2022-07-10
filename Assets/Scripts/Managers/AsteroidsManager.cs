using System.Collections.Generic;
using System.Linq;
using SpaceShooter.Components;
using SpaceShooter.MessageSystem;
using SpaceShooter.MessageSystem.Data;
using UnityEngine;

namespace SpaceShooter.Managers
{
	[RequireComponent(typeof(ObjectPool.ObjectPool))]
	public class AsteroidsManager : MonoBehaviour
	{
		[SerializeField] private int _stepYDistanceInterval = 10;
		private List<Asteroid> _asteroids;

		private int _asteroidsPassed;

		private ObjectPool.ObjectPool _objectPool;

		private AsteroidPassedData _asteroidPassedData = new AsteroidPassedData();

		public void Awake()
		{
			_objectPool = GetComponent<ObjectPool.ObjectPool>();
			_objectPool.OnSetNewPosition += OnSetNewPosition;

			if (_objectPool.IsInitialized)
			{
				OnInit();
			}
			else
			{
				_objectPool.OnInit += OnInit;
			}

			MessageSystemManager.AddListener<TimeData>(MessageType.OnPlayingTimeUpdate, OnPlayingTimeUpdate);
			MessageSystemManager.AddListener<PlayerBoostStatus>(MessageType.OnPlayerBoostStatusChange,
				OnPlayerBoostStatusChange);
		}

		private void Update()
		{
			foreach (Asteroid asteroid in _asteroids)
			{
				asteroid.Rotate();
			}
		}

		private void OnDestroy()
		{
			_objectPool.OnSetNewPosition -= OnSetNewPosition;
			_objectPool.OnInit -= OnInit;

			foreach (var asteroid in _asteroids)
			{
				asteroid.OnCollision -= OnCollision;
			}

			MessageSystemManager.RemoveListener<TimeData>(MessageType.OnPlayingTimeUpdate, OnPlayingTimeUpdate);
			MessageSystemManager.RemoveListener<PlayerBoostStatus>(MessageType.OnPlayerBoostStatusChange,
				OnPlayerBoostStatusChange);
		}

		private void OnPlayerBoostStatusChange(PlayerBoostStatus playerBoostStatus)
		{
			foreach (var asteroid in _asteroids)
			{
				asteroid.SetBoostStatus(playerBoostStatus.BoostStatus);
			}
		}

		private void OnSetNewPosition(GameObject instance)
		{
			_asteroidsPassed++;
			_asteroidPassedData.Count = _asteroidsPassed;

			MessageSystemManager.Invoke(MessageType.OnAsteroidPassed, _asteroidPassedData);
		}

		private void OnInit()
		{
			_objectPool.OnInit -= OnInit;

			int instanceCount = _objectPool.Instances.Count();

			_asteroids = new List<Asteroid>(instanceCount);

			for (int i = 0; i < instanceCount; i++)
			{
				GameObject instance = _objectPool.Instances.ElementAt(i);

				Asteroid asteroid = instance.GetComponent<Asteroid>();
				if (asteroid != null)
				{
					_asteroids.Add(asteroid);
					asteroid.OnCollision += OnCollision;
				}
				else
				{
					Debug.LogWarning("Asteroid is null");
				}
			}
		}

		private void OnCollision()
		{
			MessageSystemManager.Invoke(MessageType.OnAsteroidCollision);
		}

		private void OnPlayingTimeUpdate(TimeData timeData)
		{
			if (timeData.Seconds != 0 && (timeData.Seconds % _stepYDistanceInterval) == 0)
			{
				_objectPool.StepYDistance();
			}
		}
	}
}

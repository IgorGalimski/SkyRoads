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

		private List<Asteroid> _passedAsteroids = new List<Asteroid>();
		private List<Asteroid> _asteroids;

		private int _asteroidsPassed;

		private ObjectPool.ObjectPool _objectPool;

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

			MessageSystemManager.AddListener<PositionData>(MessageType.OnPlayerPositionUpdate, OnPlayerPositionUpdate);
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

			MessageSystemManager.RemoveListener<PositionData>(MessageType.OnPlayerPositionUpdate,
				OnPlayerPositionUpdate);
			MessageSystemManager.RemoveListener<TimeData>(MessageType.OnPlayingTimeUpdate, OnPlayingTimeUpdate);
			MessageSystemManager.RemoveListener<PlayerBoostStatus>(MessageType.OnPlayerBoostStatusChange,
				OnPlayerBoostStatusChange);
		}

		private void OnPlayerBoostStatusChange(PlayerBoostStatus playerBoostStatus)
		{
			foreach (var asteroid in _asteroids)
			{
				Debug.LogError(asteroid.name);
				asteroid.SetBoostStatus(playerBoostStatus.BoostStatus);
			}
		}

		private void OnSetNewPosition(GameObject instance)
		{
			Asteroid asteroid = instance.GetComponent<Asteroid>();
			if (asteroid != null)
			{
				if (_passedAsteroids.Contains(asteroid))
				{
					_passedAsteroids.Remove(asteroid);
				}
				else
				{
					Debug.LogWarning(string.Format("PassedAsteroid doesn't contain asteroid {0}", asteroid.name));
				}
			}
			else
			{
				Debug.LogWarning("asteroid is null");
			}
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

		private void OnPlayerPositionUpdate(PositionData positionData)
		{
			foreach (Asteroid asteroid in _asteroids)
			{
				if (asteroid.transform.position.y < positionData.Position.y)
				{
					if (!_passedAsteroids.Contains(asteroid))
					{
						_passedAsteroids.Add(asteroid);

						_asteroidsPassed++;

						MessageSystemManager.Invoke(MessageType.OnAsteroidPassed,
							new AsteroidPassedData(_asteroidsPassed));
					}
				}
			}
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

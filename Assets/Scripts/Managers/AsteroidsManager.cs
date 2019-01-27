using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(ObjectPool))]
public class AsteroidsManager : MonoBehaviour
{
	[SerializeField] 
	private int _stepYDistanceInterval = 10;
	
	private List<Asteroid> _passedAsteroids = new List<Asteroid>();
	private Asteroid[] _asteroids = new Asteroid[0];

	private int _asteroidsPassed;

	private ObjectPool _objectPool; 
	
	private void Awake()
	{
		_objectPool = GetComponent<ObjectPool>();
		_objectPool.OnSetNewPosition += OnSetNewPosition;
		_objectPool.OnInit += OnInit;
		
		MessageSystemManager.AddListener<PositionData>(MessageType.OnPlayerPositionUpdate, OnPlayerPositionUpdate);
		MessageSystemManager.AddListener<TimeData>(MessageType.OnPlayingTimeUpdate, OnPlayingTimeUpdate);
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
		
		MessageSystemManager.RemoveListener<PositionData>(MessageType.OnPlayerPositionUpdate, OnPlayerPositionUpdate);
		MessageSystemManager.RemoveListener<TimeData>(MessageType.OnPlayingTimeUpdate, OnPlayingTimeUpdate);
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
		
		_asteroids = new Asteroid[instanceCount];

		for(int i = 0; i < instanceCount; i++)
		{
			GameObject instance = _objectPool.Instances.ElementAt(i);
			
			Asteroid asteroid = instance.GetComponent<Asteroid>();
			if (asteroid != null)
			{
				_asteroids[i] = asteroid;
			}
			else
			{
				Debug.LogWarning("Asteroid is null");
			}
		}
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
				
					MessageSystemManager.Invoke(MessageType.OnAsteroidPassed, new AsteroidPassedData(_asteroidsPassed));
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

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class AsteroidsManager : MonoBehaviour
{
	private const int INIT_NUMBER_ASTEROIDS = 10;

	private const string ASTEROID_NAME = "Asteroid: {0}";
	
	[SerializeField]
	private Vector2 _minPosition;
    
	[SerializeField]
	private Vector2 _maxPosition;

	[SerializeField]
	private Vector2 _distMin;
	
	[SerializeField]
	private Vector2 _distMax;

	[SerializeField]
	private Asteroid[] _asteroids;

	[SerializeField] 
	private float _playerYDistanceTolerance = 0.0001f;

	private List<Asteroid> _asteroidInstances = new List<Asteroid>();

	private Vector3 _previousPosition = Vector3.zero;
	
	void Awake ()
	{
		InstantiateAsteroids();
		
		MessageSystemManager.AddListener<PositionData>(MessageType.OnPlayerPositionUpdate, OnPlayerPositionUpdate);
	}

	private void InstantiateAsteroids()
	{
		if (_asteroids == null || !_asteroids.Any())
		{
			Debug.LogError("Asteroids are null or empty!");
			
			return;
		}
		
		for (int i = 0; i < INIT_NUMBER_ASTEROIDS; i++)
		{
			Asteroid asteroid = _asteroids[Random.Range(0, _asteroids.Length - 1)];

			Asteroid asteroidInstance = Instantiate(asteroid, GetNewPosition(), Quaternion.identity, transform);
			asteroidInstance.name = string.Format(ASTEROID_NAME, i);
			
			_asteroidInstances.Add(asteroidInstance);
		}
	}

	private Vector3 GetNewPosition()
	{
		float distanceX = Random.Range(_distMin.x, _distMax.x);
		float distanceY = Random.Range(_distMin.y, _distMax.y);

		float x = Mathf.Clamp(distanceX + _previousPosition.x, _minPosition.x, _maxPosition.x);
		float y = Mathf.Clamp(distanceY + _previousPosition.y, _minPosition.y, _maxPosition.y);
		
		Vector3 position = new Vector3(x, y, 0);

		_previousPosition = position;

		return position;
	}

	private void OnPlayerPositionUpdate(PositionData positionData)
	{
		foreach (Asteroid asteroid in _asteroidInstances)
		{
			if (Math.Abs(asteroid.transform.position.y - positionData.Position.y) < _playerYDistanceTolerance)
			{
				Debug.Log(asteroid.name + " " + asteroid.transform.position.y + " -> " + positionData.Position.y + "  " );
			}
		}
	}
}

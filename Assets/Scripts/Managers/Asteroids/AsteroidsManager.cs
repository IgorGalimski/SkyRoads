using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using Random = UnityEngine.Random;

public class AsteroidsManager : MonoBehaviour
{
	private const int INIT_NUMBER_ASTEROIDS = 10;

	private const string ASTEROID_NAME = "Asteroid: {0}";
	
	[SerializeField]
	private float _minXPosition;
    
	[SerializeField]
	private float _maxXPosition;

	[SerializeField]
	private Vector2 _distMin;
	
	[SerializeField]
	private Vector2 _distMax;

	[SerializeField]
	private Asteroid[] _asteroids;

	private List<Asteroid> _asteroidInstances = new List<Asteroid>();
	
	private List<Asteroid> _passedAsteroids = new List<Asteroid>();

	private Vector3 _previousPosition = Vector3.zero;

	private int _asteroidsPassed;
	
	void Awake ()
	{
		InstantiateAsteroids();
		
		MessageSystemManager.AddListener<PositionData>(MessageType.OnPlayerPositionUpdate, OnPlayerPositionUpdate);
	}

	private void Update()
	{
		Vector3 screenBottomWorldCoordinate = Camera.main.ViewportToWorldPoint(Vector3.zero);
		
		foreach (Asteroid asteroid in _asteroidInstances)
		{
			if (asteroid.transform.position.y < screenBottomWorldCoordinate.y)
			{
				_passedAsteroids.Remove(asteroid);
				
				asteroid.transform.position = GetNewPosition();
			}
		}
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
		float distanceX = Random.Range(_distMin.x, _distMax.x) * Mathf.Sign(Random.Range(-1f, 1f));
		float distanceY = Random.Range(_distMin.y, _distMax.y);

		float x = Mathf.Clamp(distanceX + _previousPosition.x, _minXPosition, _maxXPosition);
		float y = distanceY + _previousPosition.y;
		
		Vector3 position = new Vector3(x, y, 0);

		_previousPosition = position;

		return position;
	}

	private void OnPlayerPositionUpdate(PositionData positionData)
	{
		foreach (Asteroid asteroid in _asteroidInstances)
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
}

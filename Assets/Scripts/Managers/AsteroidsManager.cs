using System.Collections.Generic;

using UnityEngine;

[RequireComponent(typeof(ObjectPool))]
public class AsteroidsManager : MonoBehaviour
{
	private List<Asteroid> _passedAsteroids = new List<Asteroid>();

	private int _asteroidsPassed;

	private ObjectPool _objectPool; 
	
	private void Awake()
	{
		_objectPool = GetComponent<ObjectPool>();
		_objectPool.OnSetNewPosition += OnSetNewPosition;
		
		MessageSystemManager.AddListener<PositionData>(MessageType.OnPlayerPositionUpdate, OnPlayerPositionUpdate);
	}

	private void OnDestroy()
	{
		_objectPool.OnSetNewPosition -= OnSetNewPosition;
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

	private void OnPlayerPositionUpdate(PositionData positionData)
	{
		foreach (GameObject instance in _objectPool.Instances)
		{
			Asteroid asteroid = instance.GetComponent<Asteroid>();
			if (asteroid == null)
			{
				Debug.LogWarning("Asteroid is null");
				
				continue;
			}
			
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

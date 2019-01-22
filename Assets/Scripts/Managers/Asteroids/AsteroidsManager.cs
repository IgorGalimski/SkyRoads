using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AsteroidsManager : MonoBehaviour
{
	[SerializeField] 
	private Vector2 _min;
	
	[SerializeField]
	private Vector2 _max;

	[SerializeField]
	private Vector2 _distMin;
	
	[SerializeField]
	private Vector2 _distMax;

	private Asteroid[] _asteroids;
	
	void Start ()
	{
		return;
		
		Vector3 previousPosition = Vector3.zero;

		float distanceX = 0;
		float distanceY = 0;

		float x = 0;
		float y = 0;
		
		for (int i = 0; i < 100; i++)
		{
			distanceX = Random.Range(_distMin.x, _distMax.y);
			distanceY = Random.Range(_distMin.y, _distMax.y);

			x = Mathf.Clamp(distanceX + previousPosition.x, _min.x, _max.x);
			y = Mathf.Clamp(distanceY + previousPosition.y, _min.y, _max.y);
			
			Vector3 position = new Vector3(x, y, 0);

			//del
			GameObject ojb = GameObject.CreatePrimitive(PrimitiveType.Cube);
			ojb.transform.position = position;

			previousPosition = ojb.transform.position;
		}
	}
}

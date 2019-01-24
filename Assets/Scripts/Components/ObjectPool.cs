using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using Random = UnityEngine.Random;

public class ObjectPool : MonoBehaviour
{
    private enum PositionType
    {
        RandomDistance,
        Offset
    }

    [SerializeField] 
    private PositionType _positionType;

    [SerializeField]
    private float _minXPosition;
    
    [SerializeField]
    private float _maxXPosition;

    [SerializeField]
    private Vector2 _distMin;
	
    [SerializeField]
    private Vector2 _distMax;

    [SerializeField] 
    private Vector3 _offset;
    
    [SerializeField] 
    private int _initCount;

    [SerializeField] 
    private GameObject[] _prefabs;
    
    private List<GameObject> _instances = new List<GameObject>();
    
    private Vector3 _previousPosition = Vector3.zero;

    public event Action<GameObject> OnSetNewPosition;

    public List<GameObject> Instances
    {
        get
        {
            return _instances;
        }
    }

    private void Awake()
    {
        Init();
    }
    
    private void Update()
    {
        Vector3 screenBottomWorldCoordinate = Camera.main.ViewportToWorldPoint(Vector3.zero);
		
        foreach (GameObject instance in _instances)
        {
            if (instance.transform.position.y < screenBottomWorldCoordinate.y)
            {
                if (OnSetNewPosition != null)
                {
                    OnSetNewPosition(instance);
                }
				
                instance.transform.position = GetNewPosition();
            }
        }
    }
    
    private void Init()
    {
        if (_prefabs == null || !_prefabs.Any())
        {
            Debug.LogError("Prefabs are null or empty!");
			
            return;
        }
		
        for (int i = 0; i < _initCount; i++)
        {
            GameObject prefab = _prefabs[Random.Range(0, _prefabs.Length - 1)];

            GameObject instance = Instantiate(prefab, GetNewPosition(), prefab.transform.rotation, transform);
            instance.name = string.Concat(prefab.name, " ", i);
			
            _instances.Add(instance);
        }
    }
    
    private Vector3 GetNewPosition()
    {
        Vector3 newPosition = Vector3.zero;
        
        switch (_positionType)
        {
            case PositionType.RandomDistance:
            {
                float distanceX = Random.Range(_distMin.x, _distMax.x) * Mathf.Sign(Random.Range(-1f, 1f));
                float distanceY = Random.Range(_distMin.y, _distMax.y);

                float x = Mathf.Clamp(distanceX + _previousPosition.x, _minXPosition, _maxXPosition);
                float y = distanceY + _previousPosition.y;
		
                newPosition = new Vector3(x, y, 0);

                break;
            }

            case PositionType.Offset:
            {
                newPosition = _previousPosition + _offset;
                
                break;
            }
        }
        
        _previousPosition = newPosition;

        return newPosition;
    }
    
    private void OnDestroy()
    {
        foreach (GameObject instance in _instances)
        {
            Destroy(instance);
        }
		
        _instances.Clear();
    }
}
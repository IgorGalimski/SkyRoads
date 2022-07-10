using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace SpaceShooter.ObjectPool
{
    public class ObjectPool : MonoBehaviour
    {
        public enum PositionType
        {
            RandomDistance,
            Offset
        }

        [SerializeField] private PositionType _positionType;

        public PositionType positionType
        {
            get { return _positionType; }
        }

        [SerializeField] private List<float> _xValues;

        [SerializeField] private float _distYMin;

        [SerializeField] private float _distYMax;

        [SerializeField] private float _distYStep;

        [SerializeField] private Vector3 _offset;

        [SerializeField] private Vector3 _startPosition = Vector3.zero;

        [SerializeField] [Range(1f, 100f)] private int _initCount;

        [SerializeField] private PoolableObject[] _prefabs;

        private Dictionary<PoolableObject, GameObject> _instances = new Dictionary<PoolableObject, GameObject>();
        private Vector3 _previousPosition = Vector3.negativeInfinity;

        public event Action OnInit;
        public event Action<GameObject> OnSetNewPosition;

        private float _distY;

        public IEnumerable<GameObject> Instances
        {
            get { return _instances.Values.Where(item => item != null); }
        }

        public bool IsInitialized { get; private set; }

        public void StepYDistance()
        {
            _distY = Mathf.Clamp(_distY + _distYStep, _distYMin, _distYMax);
        }

        private void Awake()
        {
            _distY = _distYMax;

            IsInitialized = false;

            Init();
        }

        private void Update()
        {
            Vector2 screenBottomPosition = Camera.main.ViewportToWorldPoint(Vector3.zero);

            foreach (PoolableObject poolableObject in _instances.Keys)
            {
                if (poolableObject.GetTopBorder().y < screenBottomPosition.y)
                {
                    if (OnSetNewPosition != null)
                    {
                        OnSetNewPosition(poolableObject.gameObject);
                    }

                    poolableObject.transform.position = GetNewPosition();
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

            _previousPosition = _startPosition;

            for (int i = 0; i < _initCount; i++)
            {
                PoolableObject prefab = _prefabs.GetRandomElement<PoolableObject>();

                PoolableObject poolableObject =
                    Instantiate(prefab, GetNewPosition(), prefab.transform.rotation, transform);
                poolableObject.name = string.Concat(prefab.name, " ", i);

                _instances.Add(poolableObject, poolableObject.gameObject);
            }

            IsInitialized = true;

            OnInit?.Invoke();
        }

        private Vector3 GetNewPosition()
        {
            Vector3 newPosition = Vector3.zero;

            if (_previousPosition != Vector3.negativeInfinity)
            {
                switch (_positionType)
                {
                    case PositionType.RandomDistance:
                    {
                        float x = _xValues
                            .Where(item => !Mathf.Approximately(item, _previousPosition.x))
                            .ToArray()
                            .GetRandomElement<float>();
                        float y = _distY + _previousPosition.y;

                        newPosition = new Vector3(x, y, _previousPosition.z);

                        break;
                    }

                    case PositionType.Offset:
                    {
                        newPosition = _previousPosition + _offset;

                        break;
                    }
                }
            }
            else
            {
                newPosition = _startPosition;
            }

            _previousPosition = newPosition;

            return newPosition;
        }

        private void OnDestroy()
        {
            foreach (GameObject instance in _instances.Values)
            {
                Destroy(instance);
            }

            _instances.Clear();
        }
    }
}
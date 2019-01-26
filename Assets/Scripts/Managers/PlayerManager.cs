using System;

using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PlayerManager : MonoBehaviour
{
    [Serializable]
    private struct ParticleEffect
    {
        [SerializeField]
        private ParticleSystem _particleSystem;
        public ParticleSystem ParticleSystem
        {
            get { return _particleSystem; }
        }

        [SerializeField]
        private bool _play;
        public bool Play
        {
            get { return _play; }
        }
    }
    
    [SerializeField] 
    private float _xMoveSpeed = 5f;
    
    [SerializeField]
    private float _yMoveSpeed = 5f;

    [SerializeField]
    private float _xMin = -5f;
    
    [SerializeField]
    private float _xMax = 5f;

    [SerializeField] 
    private float _maxYDeflectionAngle = 60f;

    [SerializeField]
    private float _boostMultiplier = 2f;

    [SerializeField] 
    private ParticleEffect[] _particleEffects;

    private bool _boost;
    
    private void Awake()
    {
        MessageSystemManager.AddListener<LevelFailData>(MessageType.OnGameFail, OnGameFail);
        MessageSystemManager.AddListener(MessageType.OnAsteroidCollision, OnAsteroidCollision);
        MessageSystemManager.AddListener<AxisData>(MessageType.OnAxisInput, OnInputAxis);
        MessageSystemManager.AddListener<KeyData>(MessageType.OnKeyDown, OnKeyDown);
        MessageSystemManager.AddListener<KeyData>(MessageType.OnKeyUp, OnKeyUp);
    }

    private void Update()
    {
        transform.position += Vector3.up * Time.deltaTime * _yMoveSpeed * (_boost ? _boostMultiplier : 1f);
        
        MessageSystemManager.Invoke(MessageType.OnPlayerPositionUpdate, new PositionData(transform.position));
    }

    private void OnDestroy()
    {
        MessageSystemManager.RemoveListener<LevelFailData>(MessageType.OnGameFail, OnGameFail);
        MessageSystemManager.RemoveListener(MessageType.OnAsteroidCollision, OnAsteroidCollision);
        MessageSystemManager.RemoveListener<AxisData>(MessageType.OnAxisInput, OnInputAxis);
        MessageSystemManager.RemoveListener<KeyData>(MessageType.OnKeyDown, OnKeyDown);
        MessageSystemManager.RemoveListener<KeyData>(MessageType.OnKeyUp, OnKeyUp);
    }
    
    private void OnInputAxis(AxisData axisData)
    {   
        Vector3 movementVector = axisData.HorizontalAxis > 0f ? Vector3.right : Vector3.left;

        Vector3 position = (transform.position += movementVector * Time.deltaTime * _xMoveSpeed);
        position.x = Mathf.Clamp(position.x, _xMin, _xMax);
        
        transform.position = position;

        Vector3 rotation = transform.rotation.eulerAngles;
        rotation.y = _maxYDeflectionAngle * axisData.HorizontalAxis;

        transform.rotation = Quaternion.Euler(rotation);
    }

    private void OnKeyDown(KeyData keyData)
    {
        if (keyData.KeyCode.Equals(KeyCode.Space))
        {
            _boost = true;
            
            MessageSystemManager.Invoke(MessageType.OnPlayerBoostStatusChange, new PlayerBoostStatus(_boost));
        }
    }
    
    private void OnKeyUp(KeyData keyData)
    {
        if (keyData.KeyCode.Equals(KeyCode.Space))
        {
            _boost = false;
            
            MessageSystemManager.Invoke(MessageType.OnPlayerBoostStatusChange, new PlayerBoostStatus(_boost));
        }
    }

    private void OnGameFail(LevelFailData levelFailData)
    {
        _yMoveSpeed = 0f;
    }

    private void OnAsteroidCollision()
    {
        if (_particleEffects == null)
        {
            return;
        }

        foreach (ParticleEffect particleEffect in _particleEffects)
        {
            if (particleEffect.ParticleSystem == null)
            {
                Debug.LogWarning("ParticleEffect is null");
                
                continue;
            }

            if (particleEffect.Play)
            {
                particleEffect.ParticleSystem.Play();
            }
            else
            {
                particleEffect.ParticleSystem.Stop();
            }
        }
    }
}
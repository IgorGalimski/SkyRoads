using System;
using Data;
using MessageSystem.Data;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PlayerManager : MonoBehaviour
{
    [SerializeField] 
    private MovementDataScriptableObject _data;
    
    [SerializeField] 
    private ParticleEffect[] _particleEffects;
        
    [Serializable]
    public struct ParticleEffect
    {
        [SerializeField]
        private ParticleSystem _particleSystem;

        [SerializeField]
        private bool _play;
        
        public ParticleSystem ParticleSystem => _particleSystem;
        public bool Play => _play;
    }

    private bool _boost;

    private bool _fail;

    private float? _boostTime;
    
    private void Awake()
    {
        _boost = false;
        _fail = false;
        
        MessageSystemManager.AddListener<LevelFailData>(MessageType.OnGameFail, OnGameFail);
        MessageSystemManager.AddListener(MessageType.OnAsteroidCollision, OnAsteroidCollision);
        MessageSystemManager.AddListener<AxisData>(MessageType.OnAxisInput, OnInputAxis);
        MessageSystemManager.AddListener(MessageType.OnPlayerBoostCollide, OnPlayerBoost);
    }

    private void Update()
    {
        if (_fail)
        {
            return;
        }
        
        transform.position += Vector3.up 
                              * Time.deltaTime 
                              * _data.YMoveSpeed 
                              * (_boost ? _data.BoostMultiplier : 1f);
        
        MessageSystemManager.Invoke(MessageType.OnPlayerPositionUpdate, new PositionData(transform.position));

        if (_boostTime.HasValue)
        {
            if (_boostTime.Value > 0)
            {
                _boostTime -= Time.deltaTime;
                
                MessageSystemManager.Invoke(MessageType.OnPlayerBoostFillChange, 
                    new BoostFillData(_boostTime.Value / _data.BoostTime));
            }
            else
            {
                _boost = false;
                _boostTime = null;
                
                MessageSystemManager.Invoke(MessageType.OnPlayerBoostStatusChange, 
                    new PlayerBoostStatus(_boost));
            }
        }
    }

    private void OnDestroy()
    {
        MessageSystemManager.RemoveListener<LevelFailData>(MessageType.OnGameFail, OnGameFail);
        MessageSystemManager.RemoveListener(MessageType.OnAsteroidCollision, OnAsteroidCollision);
        MessageSystemManager.RemoveListener<AxisData>(MessageType.OnAxisInput, OnInputAxis);
        MessageSystemManager.RemoveListener(MessageType.OnPlayerBoostCollide, OnPlayerBoost);
    }
    
    private void OnInputAxis(AxisData axisData)
    {   
        Vector3 movementVector = axisData.HorizontalAxis > 0f ? Vector3.right : Vector3.left;

        Vector3 position = (transform.position += movementVector * Time.deltaTime * _data.XMoveSpeed);
        position.x = Mathf.Clamp(position.x, _data.XMin, _data.XMax);
        
        transform.position = position;

        Vector3 rotation = transform.rotation.eulerAngles;
        rotation.y = _data.MaxYDeflectionAngle * axisData.HorizontalAxis;

        transform.rotation = Quaternion.Euler(rotation);
    }

    private void OnPlayerBoost()
    {
        _boost = true;
        
        _boostTime = _data.BoostTime;
            
        MessageSystemManager.Invoke(MessageType.OnPlayerBoostStatusChange, new PlayerBoostStatus(_boost));
    }

    private void OnGameFail(LevelFailData levelFailData)
    {
        _fail = true;
        
        MessageSystemManager.RemoveListener<AxisData>(MessageType.OnAxisInput, OnInputAxis);
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
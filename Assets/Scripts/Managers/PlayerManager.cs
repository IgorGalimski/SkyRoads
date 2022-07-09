using System;
using Data;
using DefaultNamespace;
using MessageSystem.Data;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] 
    private MovementDataScriptableObject _data;

    [SerializeField] 
    private PlayerView _playerView;

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

        _playerView.AddPosition(Vector3.up 
                                * Time.deltaTime 
                                * _data.YMoveSpeed 
                                * (_boost ? _data.BoostMultiplier : 1f));
        
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

        Vector3 position = _playerView.Position + (movementVector * Time.deltaTime * _data.XMoveSpeed);
        position.x = Mathf.Clamp(position.x, _data.XMin, _data.XMax);

        _playerView.UpdatePosition(position);

        Vector3 rotation = _playerView.Rotation.eulerAngles;
        rotation.y = _data.MaxYDeflectionAngle * axisData.HorizontalAxis;

        _playerView.UpdateRotation(Quaternion.Euler(rotation));
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
        _playerView.PlayDestroyEffect();
    }
}
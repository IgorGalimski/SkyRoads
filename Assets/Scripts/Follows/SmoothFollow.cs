using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
    [SerializeField]
    private float _distance = 3.0f;
    [SerializeField] 
    private float _distanceBoost;
    
    [SerializeField]
    private float _height = -2.0f;
    [SerializeField] 
    private float _heightBoost;
    
    [SerializeField]
    private float _heightDamping = 4.0f;
    
    [SerializeField]
    private Transform _target;

    private float Distance
    {
        get { return _playerBoost ? _distanceBoost : _distance; }
    }

    private float Height
    {
        get { return _playerBoost ? _heightBoost : _height; }
    }

    private bool _playerBoost;
    
    private void Awake()
    {
        MessageSystemManager.AddListener<PlayerBoostStatus>(MessageType.OnPlayerBoostStatusChange, OnPlayerBoostStatusChange);        
        MessageSystemManager.AddListener(MessageType.OnAsteroidCollision, OnAsteroidCollision);
    }

    private void LateUpdate()
    {
        // Early out if we don't have a target
        if (!_target)
        {
            return;
        }

        float wantedHeight = _target.position.y + Height;

        float currentHeight = transform.position.y;

        // Damp the height
        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, _heightDamping * Time.deltaTime);

        // Set the position of the camera on the x-z plane to:
        // distance meters behind the target
        var pos = transform.position;
        pos = _target.position -  Vector3.forward * Distance;
        pos.y = currentHeight;
        transform.position = pos;

        // Always look at the target
        transform.LookAt(_target);
    }

    private void OnDestroy()
    {
        MessageSystemManager.RemoveListener<PlayerBoostStatus>(MessageType.OnPlayerBoostStatusChange, OnPlayerBoostStatusChange);
        MessageSystemManager.RemoveListener(MessageType.OnAsteroidCollision, OnAsteroidCollision);
    }

    private void OnPlayerBoostStatusChange(PlayerBoostStatus playerBoostStatus)
    {
        _playerBoost = playerBoostStatus.BoostStatus;
    }

    private void OnAsteroidCollision()
    {
        enabled = false;
    }
}
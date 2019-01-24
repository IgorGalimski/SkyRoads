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
    private float _heightDamping = 2.0f;
    
    [SerializeField]
    private float _rotationDamping = 3.0f;
    
    [SerializeField]
    private Transform _target;

    private float Distance
    {
        get { return _playerBoost ? _distanceBoost : _distance; }
    }

    public float Height
    {
        get { return _playerBoost ? _heightBoost : _height; }
    }

    private bool _playerBoost;
    
    private void Awake()
    {
        MessageSystemManager.AddListener<PlayerBoostStatus>(MessageType.OnPlayerBoostStatusChange, OnPlayerBoostStatusChange);
    }

    private void LateUpdate()
    {
        // Early out if we don't have a target
        if (!_target)
        {
            return;
        }

        // Calculate the current rotation angles
        float wantedRotationAngle = _target.eulerAngles.y;
        float wantedHeight = _target.position.y + Height;

        float currentRotationAngle = transform.eulerAngles.y;
        float currentHeight = transform.position.y;

        // Damp the rotation around the y-axis
        currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, _rotationDamping * Time.deltaTime);

        // Damp the height
        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, _heightDamping * Time.deltaTime);

        // Convert the angle into a rotation
        Quaternion currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

        // Set the position of the camera on the x-z plane to:
        // distance meters behind the target
        var pos = transform.position;
        pos = _target.position - currentRotation * Vector3.forward * Distance;
        pos.y = currentHeight;
        transform.position = pos;

        // Always look at the target
        transform.LookAt(_target);
    }

    private void OnDestroy()
    {
        MessageSystemManager.RemoveListener<PlayerBoostStatus>(MessageType.OnPlayerBoostStatusChange, OnPlayerBoostStatusChange);
    }

    private void OnPlayerBoostStatusChange(PlayerBoostStatus playerBoostStatus)
    {
        _playerBoost = playerBoostStatus.BoostStatus;
    }
}
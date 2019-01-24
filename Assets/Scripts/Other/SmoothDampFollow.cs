using UnityEngine;

public class SmoothDampFollow : MonoBehaviour
{
    [SerializeField]
    private Transform _target;
    
    [SerializeField] [Range(0f, 10f)]
    private float _smoothTime = 0.3F;

    [SerializeField]
    private Vector3 _offset;

    private Vector3 _velocity;

    private Vector3 _targetPosition;

    private void LateUpdate()
    {
        if (_target == null)
        {
            Debug.LogWarning("Target is null");
            
            return;
        }
        
        _targetPosition = _target.position + _offset;

        transform.position = Vector3.SmoothDamp(transform.position, _targetPosition, ref _velocity, _smoothTime);
    }
}
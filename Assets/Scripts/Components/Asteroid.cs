using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Asteroid : MonoBehaviour
{
    [SerializeField] 
    private Vector3 _rotationDirection = Vector3.up;

    [SerializeField] 
    private float _speed = 1f;
    
    public void Rotate()
    {
        transform.Rotate(_rotationDirection * _speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        MessageSystemManager.Invoke(MessageType.OnAsteroidCollision);
    }
}

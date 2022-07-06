using Data;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PlayerManager : MonoBehaviour
{
    [SerializeField] 
    private MovementDataScriptableObject _data;

    private bool _boost;

    private bool _fail;
    
    private void Awake()
    {
        _boost = false;
        _fail = false;
        
        MessageSystemManager.AddListener<LevelFailData>(MessageType.OnGameFail, OnGameFail);
        MessageSystemManager.AddListener(MessageType.OnAsteroidCollision, OnAsteroidCollision);
        MessageSystemManager.AddListener<AxisData>(MessageType.OnAxisInput, OnInputAxis);
        MessageSystemManager.AddListener<KeyData>(MessageType.OnKeyDown, OnKeyDown);
        MessageSystemManager.AddListener<KeyData>(MessageType.OnKeyUp, OnKeyUp);
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

        Vector3 position = (transform.position += movementVector * Time.deltaTime * _data.XMoveSpeed);
        position.x = Mathf.Clamp(position.x, _data.XMin, _data.XMax);
        
        transform.position = position;

        Vector3 rotation = transform.rotation.eulerAngles;
        rotation.y = _data.MaxYDeflectionAngle * axisData.HorizontalAxis;

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
        _fail = true;
        
        MessageSystemManager.RemoveListener<AxisData>(MessageType.OnAxisInput, OnInputAxis);
    }

    private void OnAsteroidCollision()
    {
        if (_data.ParticleEffects == null)
        {
            return;
        }

        foreach (MovementDataScriptableObject.ParticleEffect particleEffect in _data.ParticleEffects)
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
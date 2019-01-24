using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PlayerManager : MonoBehaviour
{
    [SerializeField] 
    private float _xMoveSpeed = 5f;
    
    [SerializeField]
    private float _yMoveSpeed = 5f;

    [SerializeField]
    private float _xMin = -5f;
    
    [SerializeField]
    private float _xMax = 5f;

    [SerializeField]
    private float _boostMultiplier = 2f;

    private bool _boost;

    private float _rotat;
    
    private void Awake()
    {
        //MessageSystemManager.AddListener<LevelFailData>(MessageType.OnGameFail, OnGameFail);
        MessageSystemManager.AddListener<AxisData>(MessageType.OnAxisInput, OnInputAxis);
        MessageSystemManager.AddListener<KeyData>(MessageType.OnKeyDown, OnKeyDown);
        MessageSystemManager.AddListener<KeyData>(MessageType.OnKeyUp, OnKeyUp);
    }

    private void Update()
    {
        transform.position += Vector3.up * Time.deltaTime * _yMoveSpeed * (_boost ? _boostMultiplier : 1f);
        
        MessageSystemManager.Invoke(MessageType.OnPlayerPositionUpdate, new PositionData(transform.position));

        /*if (_rotat > 0f)
        {
            Vector3 rotation = transform.rotation.eulerAngles;
            rotation.y = 90 * _rotat;

            transform.rotation = Quaternion.Euler(rotation);

            _rotat -= Time.deltaTime;
        }*/
    }

    private void OnDestroy()
    {
        MessageSystemManager.RemoveListener<LevelFailData>(MessageType.OnGameFail, OnGameFail);
        MessageSystemManager.RemoveListener<AxisData>(MessageType.OnAxisInput, OnInputAxis);
        MessageSystemManager.RemoveListener<KeyData>(MessageType.OnKeyDown, OnKeyDown);
        MessageSystemManager.RemoveListener<KeyData>(MessageType.OnKeyUp, OnKeyUp);
    }
    
    private void OnInputAxis(AxisData axisData)
    {       
        Vector3 movementVector = axisData.HorizontalAxis > 0f ? Vector3.right : Vector3.left;
        transform.position += movementVector * Time.deltaTime * _xMoveSpeed;

        Vector3 position = transform.position;
        position.x = Mathf.Clamp(position.x, _xMin, _xMax);
        
        transform.position = position;
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
}
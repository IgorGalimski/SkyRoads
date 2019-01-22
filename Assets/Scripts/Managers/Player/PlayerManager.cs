using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] 
    private float _xMoveSpeed = 5f;
    
    [SerializeField]
    private float _yMoveSpeed = 5f;

    [SerializeField] [Range(0f, 1f)]
    private float _xMinBorder = 0.1f;
    
    [SerializeField] [Range(0f, 1f)]
    private float _xMaxBorder = 0.2f;

    [SerializeField]
    private float _boostMultiplier = 2f;

    private bool _boost;
    
    private void Awake()
    {
        MessageSystemManager.AddListener<AxisData>(MessageType.OnAxisInput, OnInputAxis);
        MessageSystemManager.AddListener<KeyData>(MessageType.OnKeyDown, OnKeyDown);
        MessageSystemManager.AddListener<KeyData>(MessageType.OnKeyUp, OnKeyUp);
    }

    private void Update()
    {
        transform.Translate(Vector3.up  * Time.deltaTime * _yMoveSpeed * (_boost ? _boostMultiplier : 1f));
    }

    private void OnDestroy()
    {
        MessageSystemManager.RemoveListener<AxisData>(MessageType.OnAxisInput, OnInputAxis);
        MessageSystemManager.RemoveListener<KeyData>(MessageType.OnKeyDown, OnKeyDown);
        MessageSystemManager.RemoveListener<KeyData>(MessageType.OnKeyUp, OnKeyUp);
    }
    
    private void OnInputAxis(AxisData axisData)
    {
        //rotatitiom
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        pos.x = (axisData.HorizontalAxis + 1f)/2;
        //transform.position = Camera.main.ViewportToWorldPoint(pos);

        Vector3 movementVector = axisData.HorizontalAxis > 0f ? Vector3.right : Vector3.left;
        transform.Translate(movementVector * Time.deltaTime * _xMoveSpeed);

        NormalizePosition();
    }

    private void NormalizePosition()
    {
        Vector3 position = Camera.main.WorldToViewportPoint(transform.position);

        position.x = Mathf.Clamp(position.x, _xMinBorder, _xMaxBorder);
        
        transform.position = Camera.main.ViewportToWorldPoint(position);
    }

    private void OnKeyDown(KeyData keyData)
    {
        if (keyData.KeyCode.Equals(KeyCode.Space))
        {
            _boost = true;
        }
    }
    
    private void OnKeyUp(KeyData keyData)
    {
        if (keyData.KeyCode.Equals(KeyCode.Space))
        {
            _boost = false;
        }
    }
}
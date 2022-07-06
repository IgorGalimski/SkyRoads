using System;
using UnityEngine;

public class InputManager : BaseManager
{
    [SerializeField] 
    private float _tolerance = 0.001f;

    private float _verticalAxis;
    private float _horizontalAxis;

    private KeyCode[] _keyCodes;
    
    protected override void Init()
    {
        _keyCodes = (KeyCode[])Enum.GetValues(typeof(KeyCode));
    }

    private void Update()
    {
        _verticalAxis = Input.GetAxis("Vertical");
        _horizontalAxis = Input.GetAxis("Horizontal");
        
        if (Math.Abs(_verticalAxis) > _tolerance || Math.Abs(_horizontalAxis) > _tolerance)
        {
            MessageSystemManager.Invoke(MessageType.OnAxisInput, new AxisData(_horizontalAxis, _verticalAxis));
        }

        foreach (KeyCode keyCode in _keyCodes)
        {
            if (Input.GetKeyDown(keyCode))
            {
                MessageSystemManager.Invoke(MessageType.OnKeyDown, new KeyData(keyCode));
            }
            
            if (Input.GetKeyUp(keyCode))
            {
                MessageSystemManager.Invoke(MessageType.OnKeyUp, new KeyData(keyCode));
            }
        }
    }
}
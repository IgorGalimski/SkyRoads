using System;
using UnityEngine;

public class InputManager : BaseManager
{
    [SerializeField] 
    private float _tolerance = 0.001f;

    private float _verticalAxis;
    private float _horizontalAxis;

    private KeyCode[] _keyCodes;

    private AxisData _axisData;
    
    protected override void Init()
    {
        _keyCodes = (KeyCode[])Enum.GetValues(typeof(KeyCode));
        _axisData = new AxisData();
    }

    private void Update()
    {
        _verticalAxis = Input.GetAxis("Vertical");
        _horizontalAxis = Input.GetAxis("Horizontal");
        
        if (Math.Abs(_verticalAxis) > _tolerance || Math.Abs(_horizontalAxis) > _tolerance)
        {
            _axisData.HorizontalAxis = _horizontalAxis;
            _axisData.VerticalAxis = _verticalAxis;
            MessageSystemManager.Invoke(MessageType.OnAxisInput, _axisData);
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
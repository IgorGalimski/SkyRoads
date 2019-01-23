using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] 
    private float _tolerance = 0.001f;

    private float _vecticalAxis;
    private float _horizontalAxis;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        _vecticalAxis = Input.GetAxis("Vertical");
        _horizontalAxis = Input.GetAxis("Horizontal");

        if (Math.Abs(_vecticalAxis) > _tolerance || Math.Abs(_horizontalAxis) > _tolerance)
        {
            MessageSystemManager.Invoke(MessageType.OnAxisInput, new AxisData(_horizontalAxis, _vecticalAxis));
        }

        foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
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
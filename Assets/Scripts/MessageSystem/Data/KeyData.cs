using UnityEngine;

public class KeyData : IMessageData
{
    public KeyCode KeyCode { get; }

    public KeyData(KeyCode keyCode)
    {
        KeyCode = keyCode;
    }
}
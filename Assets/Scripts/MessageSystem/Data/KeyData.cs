using UnityEngine;

public class KeyData : IMessageData
{
    public KeyCode KeyCode { get; private set; }

    public KeyData(KeyCode keyCode)
    {
        KeyCode = keyCode;
    }
}
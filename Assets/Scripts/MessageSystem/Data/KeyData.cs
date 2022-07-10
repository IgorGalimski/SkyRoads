using UnityEngine;

namespace SpaceShooter.MessageSystem.Data
{
    public class KeyData : IMessageData
    {
        public KeyCode KeyCode { get; }

        public KeyData(KeyCode keyCode)
        {
            KeyCode = keyCode;
        }
    }
}
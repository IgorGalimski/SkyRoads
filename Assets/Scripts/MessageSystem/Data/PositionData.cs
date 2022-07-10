using UnityEngine;

namespace SpaceShooter.MessageSystem.Data
{
    public class PositionData : IMessageData
    {
        public Vector3 Position { get; private set; }

        public PositionData(Vector3 position)
        {
            Position = position;
        }
    }
}
namespace SpaceShooter.MessageSystem.Data
{
    public class PlayerBoostStatus : IMessageData
    {
        public bool BoostStatus { get; }

        public PlayerBoostStatus(bool boostStatus)
        {
            BoostStatus = boostStatus;
        }
    }
}
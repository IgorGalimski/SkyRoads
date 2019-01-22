public class PlayerBoostStatus : IMessageData
{
    public bool BoostStatus { get; private set; }

    public PlayerBoostStatus(bool boostStatus)
    {
        BoostStatus = boostStatus;
    }
}
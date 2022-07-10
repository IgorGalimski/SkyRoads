namespace SpaceShooter.MessageSystem.Data
{
    public class BoostFillData : IMessageData
    {
        public float Fill { get; }
        
        public BoostFillData(float fill)
        {
            Fill = fill;
        }
    }
}
namespace SpaceShooter.MessageSystem.Data
{
    public class AsteroidPassedData : IMessageData
    {
        private const string ASTEROIDS_PASSED_FORMAT = "Asteroids: {0}";

        public int Count { get; set; }

        public string GetFormattedCount()
        {
            return string.Format(ASTEROIDS_PASSED_FORMAT, Count);
        }
    }
}
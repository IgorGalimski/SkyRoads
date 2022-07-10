namespace SpaceShooter.MessageSystem.Data
{
    public class TimeData : IMessageData
    {
        private const string TIME_PLAYING_FORMAT = "Time: {0:D2}:{1:D2}:{2:D2}";

        public int Seconds { get; set; }

        public string GetFormattedTime()
        {
            int seconds = Seconds % 60;
            int minutes = (Seconds / 60) % 60;
            int hours = Seconds / 3600;

            return string.Format(TIME_PLAYING_FORMAT, hours, minutes, seconds);
        }
    }
}
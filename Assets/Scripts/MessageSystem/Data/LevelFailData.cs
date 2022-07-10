namespace SpaceShooter.MessageSystem.Data
{
    public class LevelFailData : IMessageData
    {
        public TimeData Duration { get; }
        public AsteroidPassedData Asteroids { get; }
        public ScoreData Score { get; }
        public bool Record { get; }

        public LevelFailData(TimeData duration, AsteroidPassedData asteroids, ScoreData score, bool record)
        {
            Duration = duration;
            Asteroids = asteroids;
            Score = score;
            Record = record;
        }
    }
}
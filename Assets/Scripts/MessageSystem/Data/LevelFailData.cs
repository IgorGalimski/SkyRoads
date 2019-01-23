public class LevelFailData : IMessageData
{
    public TimeData Duration { get; private set; }
    public AsteroidPassedData Asteroids { get; private set; }
    public ScoreData Score { get; private set; }
    public bool Record { get; private set; }

    public LevelFailData(TimeData duration, AsteroidPassedData asteroids, ScoreData score, bool record)
    {
        Duration = duration;
        Asteroids = asteroids;
        Score = score;
        Record = record;
    }
}
public class ScoreData : IMessageData
{
    public int Score { get; private set; }

    public ScoreData(int score)
    {
        Score = score;
    }
}

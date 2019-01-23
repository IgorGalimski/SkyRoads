public class ScoreData : IMessageData
{
    private const string FINAL_SCORE_FORMAT = "Final score: {0}";
    
    public int CurrentScore { get; private set; }
    public int BestScore { get; private set; }

    public ScoreData(int currentScore, int bestScore)
    {
        CurrentScore = currentScore;
        BestScore = bestScore;
    }

    public string GetFormattedFinalScore()
    {
        return string.Format(FINAL_SCORE_FORMAT, CurrentScore);
    }
}

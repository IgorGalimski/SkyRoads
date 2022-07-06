public class ScoreData : IMessageData
{
    private const string FINAL_SCORE_FORMAT = "Final score: {0}";
    private const string CURRENT_SCORE_FORMAT = "Score: {0}";
    private const string BEST_SCORE_FORMAT = "Best score: {0}";
    
    public int CurrentScore { get; }
    public int BestScore { get; }

    public ScoreData(int currentScore, int bestScore)
    {
        CurrentScore = currentScore;
        BestScore = bestScore;
    }

    public string GetFormattedFinalScore()
    {
        return string.Format(FINAL_SCORE_FORMAT, CurrentScore);
    }

    public string GetFormattedCurrentScore()
    {
        return string.Format(CURRENT_SCORE_FORMAT, CurrentScore);
    }

    public string GetFormattedBestScore()
    {
        return string.Format(BEST_SCORE_FORMAT, BestScore);
    }
}

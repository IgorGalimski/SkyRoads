public class AsteroidPassedData : IMessageData
{
    private const string ASTEROIDS_PASSED_FORMAT = "Asteroids: {0}";
    
    public int Count { get; private set; }

    public AsteroidPassedData(int count)
    {
        Count = count;
    }

    public string GetFormattedCount()
    {
        return string.Format(ASTEROIDS_PASSED_FORMAT, Count);
    }
}

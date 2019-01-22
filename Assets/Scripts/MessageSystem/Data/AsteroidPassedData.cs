public class AsteroidPassedData : IMessageData
{
    public int Count { get; private set; }

    public AsteroidPassedData(int count)
    {
        Count = count;
    }
}

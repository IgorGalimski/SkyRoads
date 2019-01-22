public class TimeData : IMessageData
{
    public int Seconds { get; private set; }

    public TimeData(int seconds)
    {
        Seconds = seconds;
    }
}
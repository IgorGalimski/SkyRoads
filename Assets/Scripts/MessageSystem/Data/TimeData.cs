public class TimeData : IMessageData
{
    private const string TIME_PLAYING_FORMAT = "{0}:{1}:{2}";
    
    public int Seconds { get; private set; }

    public TimeData(int seconds)
    {
        Seconds = seconds;
    }

    public string GetFormattedTime()
    {
        int seconds = Seconds % 60; 
        int minutes = (Seconds / 60) % 60; 
        int hours = Seconds / 3600;
        
        return string.Format(TIME_PLAYING_FORMAT, hours, minutes, seconds);
    }
}
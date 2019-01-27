public class AxisData : IMessageData
{
    public float HorizontalAxis { get; private set; }
    public float VerticalAxis{ get; private set; }

    public AxisData(float horizontalAxis, float verticalAxis)
    {
        HorizontalAxis = horizontalAxis;
        VerticalAxis = verticalAxis;
    }
}

﻿public class AxisData : IMessageData
{
    public float HorizontalAxis { get; private set; }
    public float VecticalAxis{ get; private set; }

    public AxisData(float horizontalAxis, float vecticalAxis)
    {
        HorizontalAxis = horizontalAxis;
        VecticalAxis = vecticalAxis;
    }
}

using UnityEngine;

public class Player : MonoBehaviour
{
    private void Awake()
    {
        MessageSystemManager.AddListener<AxisData>(MessageType.OnAxisInput, OnInputAxis);
    }

    private void OnDestroy()
    {
        MessageSystemManager.RemoveListener<AxisData>(MessageType.OnAxisInput, OnInputAxis);
    }
    
    private void OnInputAxis(AxisData axisData)
    {
        Debug.LogWarning(axisData.VecticalAxis + " " + axisData.HorizontalAxis);
    }
}
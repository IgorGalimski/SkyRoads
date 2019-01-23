using UnityEngine;

public class LevelStartWindow : MonoBehaviour 
{
    private void Awake()
    {
        MessageSystemManager.AddListener<KeyData>(MessageType.OnKeyDown, OnKeyDown);
    }

    private void OnDestroy()
    {
        MessageSystemManager.RemoveListener<KeyData>(MessageType.OnKeyDown, OnKeyDown);
    }

    private void OnKeyDown(KeyData keyData)
    {
        MessageSystemManager.RemoveListener<KeyData>(MessageType.OnKeyDown, OnKeyDown);
        
        Application.LoadLevel(1);
    }
}

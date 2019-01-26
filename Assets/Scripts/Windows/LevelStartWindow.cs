using TMPro;

using UnityEngine;

public class LevelStartWindow : MonoBehaviour
{
    [SerializeField] 
    private TextMeshProUGUI _levelStart;
    
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

        if (_levelStart != null)
        {
            _levelStart.enabled = false;
        }
        else
        {
            Debug.LogWarning("Level start is null");
        }
        
        MessageSystemManager.Invoke(MessageType.OnGameStart);
    }
}

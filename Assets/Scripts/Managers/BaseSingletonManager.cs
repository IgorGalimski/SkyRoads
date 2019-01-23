using UnityEngine;

public abstract class BaseSingletonManager : MonoBehaviour
{
    private static BaseSingletonManager _instance;

    protected void Awake()
    {
        if (_instance == null) 
        {
            _instance = this;
        } 
        else 
        {
            if(_instance == this)
            {
                Destroy(gameObject);
            }
        }

        DontDestroyOnLoad(gameObject);
        
        Init();
    }

    protected abstract void Init();
}
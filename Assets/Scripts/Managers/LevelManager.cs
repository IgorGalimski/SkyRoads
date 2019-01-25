using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : BaseSingletonManager
{
    private Texture _colorTexture;
    
    private Color _fadeColor = Color.black;
    
    protected override void Init()
    {
        Texture2D nullTexture = new Texture2D(1,1);
        nullTexture.SetPixel(0,0, Color.black);
        nullTexture.Apply();

        _colorTexture = (Texture)nullTexture;

        MessageSystemManager.AddListener<IMessageData>(MessageType.OnGameStart, OnGameStart);
        MessageSystemManager.AddListener<IMessageData>(MessageType.OnGameReplay, OnGameReplay);
        
        StartCoroutine(FadeOut());
    }
    
    private void OnGUI() 
    {
        GUI.depth = -2;
        GUI.color = _fadeColor;
        GUI.DrawTexture(new Rect(0,0,Screen.width, Screen.height), _colorTexture, ScaleMode.StretchToFill, true);
    }

    private void OnDestroy()
    {
        MessageSystemManager.RemoveListener<IMessageData>(MessageType.OnGameStart, OnGameStart);
        MessageSystemManager.RemoveListener<IMessageData>(MessageType.OnGameReplay, OnGameReplay);
    }

    private void OnGameStart(IMessageData messageData)
    {
        StartCoroutine(LoadScene(1));
    }

    private void OnGameReplay(IMessageData messageData)
    {
        StartCoroutine(LoadScene(1));
    }

    private IEnumerator LoadScene(int sceneIndex)
    {
        yield return StartCoroutine(FadeIn());
        
        SceneManager.LoadScene(sceneIndex);

        yield return StartCoroutine(FadeOut());
        
        MessageSystemManager.Invoke(MessageType.OnGameLoad);
    }

    private IEnumerator FadeIn()
    {
        while (_fadeColor.a < 1f)
        {
            _fadeColor.a += Time.deltaTime;

            yield return null;
        }
    }

    private IEnumerator FadeOut()
    {
        while (_fadeColor.a > 0f)
        {
            _fadeColor.a -= Time.deltaTime;

            yield return null;
        }
    }
}

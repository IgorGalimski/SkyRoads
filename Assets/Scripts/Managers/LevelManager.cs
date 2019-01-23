﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : BaseSingletonManager
{
    private float _alpha;
    
    private Texture _colorTexture;
    
    private Color _fadeColor = Color.black;
    
    protected override void Init()
    {
        Texture2D nullTexture = new Texture2D(1,1);
        nullTexture.SetPixel(0,0, Color.black);
        nullTexture.Apply();

        _colorTexture = (Texture)nullTexture;

        _fadeColor.a = 0f;
        
        MessageSystemManager.AddListener<IMessageData>(MessageType.OnGameStart, OnGameStart);
        MessageSystemManager.AddListener<IMessageData>(MessageType.OnGameReplay, OnGameReplay);
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
        Debug.LogError("STARt");
        
        StartCoroutine(LoadScene(1));
    }

    private void OnGameReplay(IMessageData messageData)
    {
        Debug.LogError("Replay");
        //StartCoroutine(LoadScene(1));
    }

    private IEnumerator LoadScene(int sceneIndex)
    {
        yield return StartCoroutine(FadeIn());
        
        SceneManager.LoadScene(sceneIndex);

        yield return StartCoroutine(FadeOut());
    }

    private IEnumerator FadeIn()
    {
        while (_alpha < 1f)
        {
            _alpha += Time.deltaTime;

            _fadeColor.a = _alpha;

            yield return null;
        }
    }

    private IEnumerator FadeOut()
    {
        while (_alpha > 0f)
        {
            _alpha -= Time.deltaTime;
            
            _fadeColor.a = _alpha;

            yield return null;
        }
    }
}

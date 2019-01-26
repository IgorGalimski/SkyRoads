using System;
using System.Collections.Generic;

using UnityEngine;

public static class MessageSystemManager
{
    public delegate void EventDelegate<in T> (T e) where T : IMessageData;

    public delegate void EventDelegate ();
    
    private static Dictionary<MessageType, MultitypeDelegate> _multitypeDelegates = new Dictionary<MessageType, MultitypeDelegate>();
    public static void AddListener<T>(MessageType messageType, EventDelegate<T> eventDelegate) where T : IMessageData
    {
        AddListener(messageType, eventDelegate as Delegate);
    }
    
    public static void AddListener(MessageType messageType, EventDelegate eventDelegate)
    {
        AddListener(messageType, eventDelegate as Delegate);
    }

    public static void RemoveListener<T>(MessageType messageType, EventDelegate<T> eventDelegate) where T : IMessageData
    {
        RemoveListener(messageType, eventDelegate as Delegate);
    }
    
    public static void RemoveListener(MessageType messageType, EventDelegate eventDelegate)
    {
        RemoveListener(messageType, eventDelegate as Delegate);
    }

    public static void Invoke(MessageType messageType, IMessageData messageData = null)
    {
        if (!_multitypeDelegates.ContainsKey(messageType))
        {
            return;
        }
        
        _multitypeDelegates[messageType].Invoke(messageData);
    }
    
    private static void AddListener(MessageType messageType, Delegate action)
    {
        if (action == null)
        {
            Debug.LogWarning("Action is null");
        }

        if (!_multitypeDelegates.ContainsKey(messageType))
        {
            _multitypeDelegates.Add(messageType, new MultitypeDelegate());
        }
        
        _multitypeDelegates[messageType].AddDelegate(action);
    }
    
    private static void RemoveListener(MessageType messageType, Delegate action)
    {
        if (action == null)
        {
            Debug.LogWarning("Action is null");
        }

        if (!_multitypeDelegates.ContainsKey(messageType))
        {
            return;
        }
        
        _multitypeDelegates[messageType].RemoveDelegate(action);
    }
}

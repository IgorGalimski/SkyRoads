using System;
using System.Collections.Generic;

using UnityEngine;

public static class MessageSystemManager
{
    public delegate void EventDelegate<in T> (T e) where T : IMessageData;

    public delegate void EventDelegate ();
    
    private static Dictionary<MessageType, Delegate> _actions = new Dictionary<MessageType, Delegate>();
    public static void AddListener<T>(MessageType messageType, EventDelegate<T> action) where T : IMessageData
    {
        if (action == null)
        {
            Debug.LogWarning("Action is null");
        }

        if (!_actions.ContainsKey(messageType))
        {
            _actions.Add(messageType, action);
            
            return;
        }

        _actions[messageType] = Delegate.Combine(_actions[messageType], action);
    }

    public static void AddListener(MessageType messageType, EventDelegate action)
    {
        AddListener<IMessageData>(messageType, (e) => action());
    }

    public static void RemoveListener<T>(MessageType messageType, EventDelegate<T> action) where T : IMessageData
    {
        if (action == null)
        {
            Debug.LogWarning("Action is null");
        }

        if (!_actions.ContainsKey(messageType))
        {
            return;
        }

        _actions[messageType] = Delegate.Remove(_actions[messageType], action);
    }
    
    public static void RemoveListener(MessageType messageType, EventDelegate action)
    {
        RemoveListener<IMessageData>(messageType, (e) => action());
    }

    public static void Invoke(MessageType messageType, IMessageData messageData = null)
    {
        if (!_actions.ContainsKey(messageType))
        {
            Debug.LogWarning(string.Format("Action doesn't contain key:{0}", messageType));
            
            return;
        }

        foreach (Delegate del in _actions[messageType].GetInvocationList())
        {
            del.DynamicInvoke(messageData);
        }
    }
}

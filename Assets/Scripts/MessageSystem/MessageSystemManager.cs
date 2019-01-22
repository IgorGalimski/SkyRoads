using System.Collections.Generic;

using UnityEngine;

public static class MessageSystemManager
{
    public delegate void EventDelegate<in T> (T e) where T : IMessageData;

    private delegate void EventDelegate (IMessageData e);
    
    private static Dictionary<MessageType, EventDelegate> _actions = new Dictionary<MessageType, EventDelegate>();
    public static void AddListener<T>(MessageType messageType, EventDelegate<T> action) where T : IMessageData
    {
        if (action == null)
        {
            Debug.LogWarning("Action is null");
        }
        
        EventDelegate currentEventDelegate = (e) => action((T)e);

        if (!_actions.ContainsKey(messageType))
        {
            _actions.Add(messageType, currentEventDelegate);
            
            return;
        }

        _actions[messageType] += currentEventDelegate;
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
        
        EventDelegate currentEventDelegate = (e) => action((T)e);
        
        _actions[messageType] -= currentEventDelegate;
    }

    public static void Invoke(MessageType messageType, IMessageData messageData = null)
    {
        if (!_actions.ContainsKey(messageType))
        {
            Debug.LogWarning(string.Format("Action doesn't contain key:{0}", messageType));
            
            return;
        }

        _actions[messageType].Invoke(messageData);
    }
    
}

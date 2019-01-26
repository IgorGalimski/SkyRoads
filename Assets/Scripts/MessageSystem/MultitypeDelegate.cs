using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public class MultitypeDelegate
{
    private Dictionary<Type, Delegate> _delegates = new Dictionary<Type, Delegate>();

    public void AddDelegate(Delegate addedDelegate)
    {
        Type delegateType = addedDelegate.GetType();
        
        if (!_delegates.ContainsKey(delegateType))
        {
            _delegates[delegateType] = addedDelegate;
            
            return;
        }
        
        _delegates[delegateType] = Delegate.Combine(_delegates[delegateType], addedDelegate);
    }

    public void RemoveDelegate(Delegate removedDelegate)
    {
        Type delegateType = removedDelegate.GetType();
        
        if (!_delegates.ContainsKey(delegateType))
        {
            _delegates[delegateType] = removedDelegate;
            
            return;
        }
        
        _delegates[delegateType] = Delegate.Remove(_delegates[delegateType], removedDelegate);
    }

    public void Invoke(IMessageData messageData)
    {
        Delegate[] invokeDelegates = _delegates.Values.ToArray();
        foreach (Delegate currentDelegate in invokeDelegates)
        {
            ParameterInfo[] parameters = currentDelegate.Method.GetParameters();
            if (!parameters.Any())
            {
                currentDelegate.DynamicInvoke();
            }
            else
            {
                currentDelegate.DynamicInvoke(messageData);
            }            
        }
    }
}
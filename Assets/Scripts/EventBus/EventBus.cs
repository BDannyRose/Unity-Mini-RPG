using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EventBus : IService
{
    private Dictionary<string, List<ActionCallbackWithPriority>> signalCallbacks = new Dictionary<string, List<ActionCallbackWithPriority>>();

    public void Subscribe<T>(Action<T> callback, int priority = 0)
    {
        string key = typeof(T).Name;
        if (signalCallbacks.ContainsKey(key))
        {
            signalCallbacks[key].Add(new ActionCallbackWithPriority(priority, callback));
        }
        else
        {
            signalCallbacks.Add(key, new List<ActionCallbackWithPriority>() {new ActionCallbackWithPriority(priority, callback) });
        }

        signalCallbacks[key] = signalCallbacks[key].OrderByDescending(x => x.Priority).ToList();
    }

    public void Unsubscribe<T>(Action<T> callback)
    {
        string key = typeof(T).Name;
        if (signalCallbacks.ContainsKey(key))
        {
            var callbackToDelete = signalCallbacks[key].FirstOrDefault(x => x.Callback.Equals(callback));
            if (callbackToDelete != null)
            {
                signalCallbacks[key].Remove(callbackToDelete);
            }
        }
        else
        {
            Debug.LogError($"Trying to unsubscribe from nonexistant key {key}");
        }
    }

    public void Invoke<T>(T signal)
    {
        string key = typeof(T).Name;
        if (signalCallbacks.ContainsKey(key))
        {
            foreach (var obj in signalCallbacks[key])
            {
                var callback = obj.Callback as Action<T>;
                callback?.Invoke(signal);
            }
        }
    }
}

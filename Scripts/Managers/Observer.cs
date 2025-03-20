using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum GameEvent
{
    OnWatchingAd,
    OnResetGame
}
public abstract class Observer : MonoBehaviour
{
    public static Dictionary<GameEvent, List<Action<object[]>>> Listeners = new Dictionary<GameEvent, List<Action<object[]>>>();
    public static void AddListener(GameEvent eventName, Action<object[]> callback)
    {
        if (!Listeners.ContainsKey(eventName))
            Listeners.Add(eventName, new List<Action<object[]>>());
        Listeners[eventName].Add(callback);
    }
    public static void RemoveListener(GameEvent eventName, Action<object[]> callback)
    {
        if (!Listeners.ContainsKey(eventName))
            return;
        Listeners[eventName].Remove(callback);
    }
    public static void Notify(GameEvent eventName, params object[] datas)
    {
        if (!Listeners.ContainsKey(eventName))
        {
            Debug.LogError("Dont have event: " + eventName.ToString());
            return;
        }
        foreach(var item in Listeners[eventName])
        {
            try
            {
                item?.Invoke(datas);
            } catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }
    }
}

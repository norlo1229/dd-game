using System;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    private static EventManager _instance { get; set; }
    private Dictionary<string, Action<GameEventArgs>> eventDictionary;

    public static EventManager Instance
    {
        get
        {
            if(_instance != null)
            {
                return _instance;
            }
            else
            {
                _instance = GameObject.FindObjectOfType<EventManager>();

                if(_instance == null)
                {
                    var instance_object = new GameObject();
                    instance_object.name = "EventManager";
                    _instance = instance_object.AddComponent<EventManager>();
                    _instance.Initialize();
                }

                return _instance;
            }
        }
    }

    public void Initialize()
    {
        eventDictionary = new Dictionary<string, Action<GameEventArgs>>();
    }

    public void StartListening(string eventName, Action<GameEventArgs> listener)
    {
        Action<GameEventArgs> thisEvent;

        if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent += listener;
            Instance.eventDictionary[eventName] = thisEvent;
        }
        else
        {
            thisEvent += listener;
            Instance.eventDictionary.Add(eventName, thisEvent);
        }
    }

    public void StopListening(string eventName, Action<GameEventArgs> listener)
    {

        Action<GameEventArgs> thisEvent;

        if (_instance == null) return;

        if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent -= listener;
            Instance.eventDictionary[eventName] = thisEvent;
        }
    }

    public void TriggerEvent(string eventName, GameEventArgs eventArgs)
    {
        Action<GameEventArgs> thisEvent = null;

        if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(eventArgs);
        }
    }
}

public class GameEventArgs : EventArgs
{
    public class LoggingEventArgs : GameEventArgs
    {
        public string Message { get; set; }
    }

    public class CombatEventArgs : GameEventArgs
    {

    }
}
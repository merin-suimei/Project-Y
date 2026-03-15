using System;
using System.Collections.Generic;
using UnityEngine;

public enum EventType { ResetGameState, UpdateGameState, ResetSettings, UpdateSettings,
    LoadScene, UpdatePosition, OnObjectToggle,
    OnEnemyDetect, OnEnemyLoseAim, OnEnemyCatchPlayer, IsEnemyOnPatrolPoint,
    TurnOnEnemyPattern, TurnOffEnemyPattern,
    PlayPlayerFootStepSound, StopPlayerFootStepSound, PlayEnemyMoveSound, StopEnemyMoveSound, PlayEnemyDetectSound, OnClickPlay, OnClickAuthors };
public static class EventBus
{
    private static Dictionary<EventType, Delegate> typedActions = new();
    private static Dictionary<EventType, Action> simpleActions = new();

    public static void Raise<T>(EventType eventType, T data)
    {
        if (typedActions.TryGetValue(eventType, out Delegate existingAction)) { 
            (existingAction as Action<T>)?.Invoke(data);
        }
    }

    public static void Subscribe<T>(EventType eventType, Action<T> action)
    {
        if (typedActions.ContainsKey(eventType)) { 
            typedActions[eventType] = (Action<T>)typedActions[eventType] + action;
        }
        else
        {
            typedActions[eventType] = action;
        }
    }
    public static void Unsubscribe<T>(EventType eventType, Action<T> action)
    {
        if (typedActions.ContainsKey(eventType))
        {
            typedActions[eventType] = (Action<T>)typedActions[eventType] - action;
        }
    }

    public static void Raise(EventType type)
    {
        if (simpleActions.TryGetValue(type, out Action existingAction))
        {
            existingAction?.Invoke();
        }
    }

    public static void Subscribe(EventType type, Action action)
    {
        if (simpleActions.ContainsKey(type))
        {
            simpleActions[type] += action;
        }
        else
        {
            simpleActions[type] = action;
        }
    }
    public static void Unsubscribe(EventType type, Action action)
    {
        if (simpleActions.ContainsKey(type))
        {
            simpleActions[type] -= action;
        }
    }
}

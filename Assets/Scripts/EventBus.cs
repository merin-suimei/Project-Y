using System;
using System.Collections.Generic;

public enum EventType { ResetGameState, UpdateGameState, ResetSettings, UpdateSettings,
    OnObjectToggle, EnableEnemyPattern, UpdateResetPoint,
    OnMove, OnMoveTo, OnMoveToArrived, OnInterruptMoveTo, OnRotateTo, OnRotateToArrived, OnTeleport, OnAnimationStart, OnAnimationStop,
    OnEnemyDetect, OnEnemyLoseAim, OnEnemyCatchPlayer, OnPlayerVisible, EnemyEnableChaseSpeed,
    PlayPlayerFootStepSound, StopPlayerFootStepSound, PlayEnemyMoveSound, StopEnemyMoveSound, PlayEnemyDetectSound, OnTimerIsGoing,OnTimerIsUP };

public static class EventBus
{
    private static Dictionary<EventType, Delegate> typedActions = new();
    private static Dictionary<EventType, Action> simpleActions = new();

    public static void Raise<T1, T2>(EventType eventType, T1 data1, T2 data2)
    {
        if (typedActions.TryGetValue(eventType, out Delegate existingAction))
            (existingAction as Action<T1, T2>)?.Invoke(data1, data2);
    }

    public static void Subscribe<T1, T2>(EventType eventType, Action<T1, T2> action)
    {
        if (typedActions.ContainsKey(eventType))
            typedActions[eventType] = (Action<T1, T2>)typedActions[eventType] + action;
        else
            typedActions[eventType] = action;
    }
    public static void Unsubscribe<T1, T2>(EventType eventType, Action<T1, T2> action)
    {
        if (typedActions.ContainsKey(eventType))
            typedActions[eventType] = (Action<T1, T2>)typedActions[eventType] - action;
    }

    public static void Raise<T>(EventType eventType, T data)
    {
        if (typedActions.TryGetValue(eventType, out Delegate existingAction))
            (existingAction as Action<T>)?.Invoke(data);
    }

    public static void Subscribe<T>(EventType eventType, Action<T> action)
    {
        if (typedActions.ContainsKey(eventType))
            typedActions[eventType] = (Action<T>)typedActions[eventType] + action;
        else
            typedActions[eventType] = action;
    }
    public static void Unsubscribe<T>(EventType eventType, Action<T> action)
    {
        if (typedActions.ContainsKey(eventType))
            typedActions[eventType] = (Action<T>)typedActions[eventType] - action;
    }

    public static void Raise(EventType type)
    {
        if (simpleActions.TryGetValue(type, out Action existingAction))
            existingAction?.Invoke();
    }

    public static void Subscribe(EventType type, Action action)
    {
        if (simpleActions.ContainsKey(type))
            simpleActions[type] += action;
        else
            simpleActions[type] = action;
    }
    public static void Unsubscribe(EventType type, Action action)
    {
        if (simpleActions.ContainsKey(type))
            simpleActions[type] -= action;
    }
}

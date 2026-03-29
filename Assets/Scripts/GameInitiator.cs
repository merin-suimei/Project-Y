using UnityEngine;

public static class GameInitiator
{
    private static GameState currentGameState = Resources.Load<GameState>("CurrentGameState"); // AssetDatabase.LoadAssetAtPath<GameState>("Assets/ScriptableObjects/CurrentGameState.asset");
    private static Settings currentSettings = Resources.Load<Settings>("CurrentSettings");
    private static GameState defaultGameState = Resources.Load<GameState>("DefaultGameState");
    private static Settings defaultSettings = Resources.Load<Settings>("DefaultSettings");

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void BeforeSceneLoad()
    {
        ObjectResolver.RegisterInstance<IPlayerInput>(new InputSystemListener());
        ObjectResolver.RegisterInstance(new InputsTypes());

        ObjectResolver.RegisterInstance(currentGameState);
        ObjectResolver.RegisterInstance(currentSettings);

        EventBus.Subscribe(EventType.ResetGameState, ResetGameState);
        EventBus.Subscribe(EventType.UpdateGameState, UpdateGameState);
        EventBus.Subscribe(EventType.ResetSettings, ResetSettings);
        EventBus.Subscribe(EventType.UpdateSettings, UpdateSettings);
    }

    private static void ResetGameState()
    {
        //TODO
    }

    private static void UpdateGameState()
    {
        //TODO
    }

    private static void ResetSettings()
    {
        //TODO
    }

    private static void UpdateSettings()
    {
        //TODO
    }
}
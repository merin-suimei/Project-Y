using System.Collections.Generic;
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
        EventBus.Subscribe<List<SettingsPair>>(EventType.UpdateSettings, UpdateSettings);

        if (currentSettings.isFirstLaunch)
        {
            ResetSettings();

            // Load initial setup scene here
        }

        ApplySettings();
    }

    private static void ResetGameState()
    {
        currentGameState.currentLevel = defaultGameState.currentLevel;
        currentGameState.isHardMode = defaultGameState.isHardMode;
    }

    private static void UpdateGameState()
    {
        //TODO
    }

    private static void ResetSettings()
    {
        currentSettings.isFirstLaunch = defaultSettings.isFirstLaunch;

        currentSettings.screenResolution = new Vector2Int(Display.main.systemWidth, Display.main.systemHeight);
        currentSettings.fullScreen = defaultSettings.fullScreen;
        currentSettings.VSync = defaultSettings.VSync;

        currentSettings.soundVolume = defaultSettings.soundVolume;
        currentSettings.musicVolume = defaultSettings.musicVolume;
        currentSettings.effectsVolume = defaultSettings.effectsVolume;

        ApplySettings();
    }

    private static void UpdateSettings(List<SettingsPair> list)
    {
        foreach (SettingsPair pair in list)
        {
            switch (pair.Name)
            {
                case "screenResolution":
                    currentSettings.screenResolution = pair.Value.vector2IntValue;
                    break;
                case "fullScreen":
                    currentSettings.fullScreen = pair.Value.boolValue;
                    break;
                case "VSync":
                    currentSettings.VSync = pair.Value.boolValue;
                    break;
                case "soundVolume":
                    currentSettings.soundVolume = pair.Value.floatValue;
                    break;
                case "musicVolume":
                    currentSettings.musicVolume = pair.Value.floatValue;
                    break;
                case "effectsVolume":
                    currentSettings.effectsVolume = pair.Value.floatValue;
                    break;

                default:
                    Debug.LogError("Unknown setting name: " + pair.Name);
                    break;
            }
        }

        ApplySettings();
    }

    private static void ApplySettings()
    {
        Screen.SetResolution(currentSettings.screenResolution.x, currentSettings.screenResolution.y, currentSettings.fullScreen);
        QualitySettings.vSyncCount = currentSettings.VSync ? 1 : 0;

        // TODO: Double check audio settings application
    }
}

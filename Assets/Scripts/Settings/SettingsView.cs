using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public enum SettingsValueKind { Bool, Float, Vector2Int }
public struct SettingsValue
{
    public SettingsValueKind kind;
    public Vector2Int vector2IntValue;
    public bool boolValue;
    public float floatValue;
}

public struct SettingsPair
{
    public string Name;
    public SettingsValue Value;
}

public class SettingsView : MonoBehaviour
{

    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private TMP_Dropdown displayModeDropdown;
    [SerializeField] private Toggle vsyncToggle;
    [SerializeField] private Slider soundVolume;
    [SerializeField] private Slider musicVolume;
    [SerializeField] private Slider effectsVolume;
    [SerializeField] public GameObject SettingsPopupUI;

    private Settings _settings;
    private List<Vector2Int> _resolutions;
    private Dictionary<string, SettingsValue> _newSettings;
    private bool _initialized;

    private void Init()
    {
        _initialized = true;
        _newSettings = new Dictionary<string, SettingsValue>();
        CollectSettingsOption();
        Subscribe();
    }


    private void OnEnable()
    {
        if (!_initialized) Init();
        _newSettings.Clear();
        LoadSettings();
        BindSettings();
    }

    void Subscribe()
    {
        resolutionDropdown.onValueChanged.AddListener(OnResolutionChanged);
        displayModeDropdown.onValueChanged.AddListener(OnModeChanged);
        vsyncToggle.onValueChanged.AddListener(OnVsyncChanged);
        soundVolume.onValueChanged.AddListener(OnSoundChanged);
        musicVolume.onValueChanged.AddListener(OnMusicChanged);
        effectsVolume.onValueChanged.AddListener(OnEffectsChanged);
    }

    void LoadSettings()
    {
        _settings = ObjectResolver.Resolve<Settings>();
    }

    void CollectSettingsOption()
    {
        CollectResolutions();
    }

    void BindSettings()
    {
        BindResolutions();
        BindDisplayMode();
        BindVSync();
        BindVolumes();
    }

    public void ResetSettings()
    {
        EventBus.Raise(EventType.ResetSettings);
        SettingsPopupUI.SetActive(false);
    }

    public void UpdateSettings()
    {
        var list = _newSettings
            .Select(kv => new SettingsPair { Name = kv.Key, Value = kv.Value })
            .ToList();
        EventBus.Raise(EventType.UpdateSettings, list);
        SettingsPopupUI.SetActive(false);
    }

    public void Exit()
    {
        SettingsPopupUI.SetActive(false);
    }

    // collect settings options
    void CollectResolutions()
    {
        _resolutions = Screen.resolutions
            .Select(r => new Vector2Int(r.width, r.height))
            .Distinct()
            .ToList();
        resolutionDropdown.ClearOptions();
        var options = _resolutions.Select(v => $"{v.x}x{v.y}").ToList();
        resolutionDropdown.AddOptions(options);
    }

    // binding
    void BindResolutions()
    {
        var currentResolution = _settings.screenResolution;
        int resIndex = _resolutions.FindIndex(v => v == currentResolution);
        if (resIndex < 0) resIndex = _resolutions.Count - 1;
        resolutionDropdown.SetValueWithoutNotify(resIndex);
    }

    void BindDisplayMode()
    {
        displayModeDropdown.ClearOptions();
        var options = new List<string> { "Window", "Fullscreen"};
        displayModeDropdown.AddOptions(options);

        var currentMode = _settings.fullScreen;
        int index = currentMode ? 1 : 0;
        displayModeDropdown.SetValueWithoutNotify(index);
    }

    void BindVSync()
    {
        bool vsync = _settings.VSync;
        vsyncToggle.SetIsOnWithoutNotify(vsync);
    }

    void BindVolumes()
    {
        var sound = _settings.soundVolume;
        soundVolume.SetValueWithoutNotify(sound);

        var music = _settings.musicVolume;
        musicVolume.SetValueWithoutNotify(music);

        var effects = _settings.effectsVolume;
        effectsVolume.SetValueWithoutNotify(effects);
    }

    // change value events

    void OnResolutionChanged(int index)
    {
 ;
        _newSettings["screenResolution"] = new SettingsValue
        {
            kind = SettingsValueKind.Vector2Int,
            vector2IntValue = _resolutions[index]
        };
    }

    void OnModeChanged(int index)
    {
        _newSettings["fullScreen"] = new SettingsValue
        {
            kind = SettingsValueKind.Bool,
            boolValue = (index == 1)
        };
    }

    void OnVsyncChanged(bool isOn)
    {
        _newSettings["VSync"] = new SettingsValue
        {
            kind = SettingsValueKind.Bool,
            boolValue = isOn
        };
    }

    void OnSoundChanged(float newValue)
    {
        _newSettings["soundVolume"] = new SettingsValue
        {
            kind = SettingsValueKind.Float,
            floatValue = newValue
        };
    }

    void OnMusicChanged(float newValue)
    {
        _newSettings["musicVolume"] = new SettingsValue
        {
            kind = SettingsValueKind.Float,
            floatValue = newValue
        };
    }


    void OnEffectsChanged(float newValue)
    {
        _newSettings["effectsVolume"] = new SettingsValue
        {
            kind = SettingsValueKind.Float,
            floatValue = newValue
        };
    }
}

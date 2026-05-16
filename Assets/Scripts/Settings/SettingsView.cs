using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
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

    [SerializeField] public GameObject SettingsPopupUI;

    private Settings _settings;
    private List<Vector2Int> _resolutions;
    private Dictionary<string, SettingsValue> _newSettings;
    private bool _initialized;
    private InputsTypes _input;

    private void Init()
    {
        _initialized = true;
        _input = new InputsTypes();
        _newSettings = new Dictionary<string, SettingsValue>();
        CollectSettingsOption();
        Subscribe();
    }


    private void OnEnable()
    {
        if (!_initialized) Init();
        _input.Enable();
        _input.UI.Exit.performed += OnExit;
        _newSettings.Clear();
        LoadSettings();
        BindSettings();
    }

    private void OnDisable()
    {
        _input.UI.Exit.performed -= OnExit;
        _input.Disable();
    }

    void Subscribe()
    {
        resolutionDropdown.onValueChanged.AddListener(OnResolutionChanged);
        displayModeDropdown.onValueChanged.AddListener(OnModeChanged);
        vsyncToggle.onValueChanged.AddListener(OnVsyncChanged);
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
        EventBus.Raise(EventType.CancelSettingsChanges);
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

    private void OnExit(InputAction.CallbackContext ctx)
    {
        Exit();
    }
}

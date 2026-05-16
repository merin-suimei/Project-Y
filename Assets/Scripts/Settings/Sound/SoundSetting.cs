using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class SoundSetting : MonoBehaviour
{
    [SerializeField] Slider allSoundsSlider;
    [SerializeField] Slider sfxSlider;
    [SerializeField] Slider musicSlider;

    private SoundVolumeSO volumeData;

    private float oldAllSoundsVolume;
    private float oldSFXVolume;
    private float oldMusicVolume;

    private float initAllSoundsVolume;
    private float initSFXVolume;
    private float initMusicVolume;

    private void Awake()
    {
        volumeData = SoundManager.Instance.VolumeData;
    }
    private void Start()
    {
        EventBus.Subscribe(EventType.ResetSettings, ResetToInit);
        EventBus.Subscribe(EventType.CancelSettingsChanges, CancelChanges);

        InitializeVariables();

        allSoundsSlider.onValueChanged.AddListener(SoundManager.Instance.OnAllSoundsVolumeChanged);
        sfxSlider.onValueChanged.AddListener(SoundManager.Instance.OnSFXVolumeChanged);
        musicSlider.onValueChanged.AddListener(SoundManager.Instance.OnMusicVolumeChanged);

        allSoundsSlider.SetValueWithoutNotify(volumeData.AllSoundsVolumeView);
        sfxSlider.SetValueWithoutNotify(volumeData.SFXVolumeView);
        musicSlider.SetValueWithoutNotify(volumeData.MusicVolumeView);

    }

    private void OnEnable()
    {
        InitOldVolumes();
    }
    private void CancelChanges()
    {
        ChangeAllVolumes(oldAllSoundsVolume, oldSFXVolume, oldMusicVolume);
    }

    private void ResetToInit()
    {
        ChangeAllVolumes(initAllSoundsVolume, initSFXVolume, initMusicVolume);
    }

    private void ChangeAllVolumes(float allSoundsVol, float sfxVol, float musicVol) 
    {
        allSoundsSlider.value = allSoundsVol;
        sfxSlider.value = sfxVol;
        musicSlider.value = musicVol; 
    }

    private void InitializeVariables()
    {
        InitOldVolumes();
        initAllSoundsVolume = volumeData.AllSoundsVolumeView;
        initSFXVolume = volumeData.SFXVolumeView;
        initMusicVolume = volumeData.MusicVolumeView;
    }

    private void InitOldVolumes()
    {
        oldAllSoundsVolume = volumeData.AllSoundsVolumeView;
        oldSFXVolume = volumeData.SFXVolumeView;
        oldMusicVolume = volumeData.MusicVolumeView;
    }
}

using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class SoundManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool collectionCheck = true;
    [SerializeField] private int defaultCapacity = 10;
    [SerializeField] private int maxPoolSize = 100;
    [SerializeField] private int maxSoundInstance = 100;

    private IObjectPool<SoundEmitter> soundEmitterPool;
    private List<SoundEmitter> avaliableSoundEmittes;

    [SerializeField] private SoundEmitter soundEmitterPrefab;

    [field:SerializeField] public SoundVolumeSO VolumeData { get; private set; }

    public static SoundManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        avaliableSoundEmittes = new List<SoundEmitter>();

        DontDestroyOnLoad(gameObject);
        InitializePool();
    }

    private void Start()
    {
        UpdateAllVolumes();
    }
    private void InitializePool()
    {
        soundEmitterPool = new ObjectPool<SoundEmitter>
            (
                CreateSoundEmitter,
                OnTakeSoundEmitter,
                OnReleaseSoundEmitter,
                OnDestroySoundEmitter,
                collectionCheck,
                defaultCapacity,
                maxPoolSize
            );
    }

    public SoundEmitter Get() { return soundEmitterPool.Get(); }
    public void Release(SoundEmitter soundEmitter) 
    {
        if (soundEmitter != null)
            soundEmitterPool.Release(soundEmitter); 
    }

    public void StopAllSounds()
    {
        for (int i = avaliableSoundEmittes.Count - 1; i >= 0; i--)
        {
            if (avaliableSoundEmittes[i] != null)
                avaliableSoundEmittes[i].Stop();
        }
    }

    public void PauseAllSounds()
    {
        for (int i = avaliableSoundEmittes.Count - 1; i >= 0; i--)
        {
            if (avaliableSoundEmittes[i] != null)
                avaliableSoundEmittes[i].Pause();
        }
    }

    public void UnPauseAllSounds()
    {
        for (int i = avaliableSoundEmittes.Count - 1; i >= 0; i--)
        {
            if (avaliableSoundEmittes[i] != null)
                avaliableSoundEmittes[i].UnPause();
        }
    }
    public void OnAllSoundsVolumeChanged(float value)
    {
        VolumeData.AllSoundsVolumeView = value;
        UpdateAllVolumes();
    }

    public void OnSFXVolumeChanged(float value)
    {
        VolumeData.SFXVolumeView = value;
        UpdateAllVolumes();
    }

    public void OnMusicVolumeChanged(float value)
    {
        VolumeData.MusicVolumeView = value;
        UpdateAllVolumes();
    }

    private void UpdateAllVolumes()
    {

        float actualSFXVolume = VolumeData.AllSoundsVolumeView * VolumeData.SFXVolumeView;
        float actualMusicVolume = VolumeData.AllSoundsVolumeView * VolumeData.MusicVolumeView;

        VolumeData.SFXVolume = actualSFXVolume;
        VolumeData.MusicVolume = actualMusicVolume;

        for (int i = avaliableSoundEmittes.Count - 1; i >= 0; i--)
        {
            if (avaliableSoundEmittes[i] != null)
            {
                if (avaliableSoundEmittes[i].soundData.IsSFX)
                {
                    avaliableSoundEmittes[i].ChangeVolume(actualSFXVolume);
                }
                else
                {
                    avaliableSoundEmittes[i].ChangeVolume(actualMusicVolume);
                }

            }
        }
    }
    private SoundEmitter CreateSoundEmitter()
    {
        SoundEmitter soundEmitter = Instantiate(soundEmitterPrefab, transform);
        soundEmitter.gameObject.SetActive(false);
        return soundEmitter;
    }

    private void OnTakeSoundEmitter(SoundEmitter soundEmitter)
    {
        soundEmitter.gameObject.SetActive(true);
        avaliableSoundEmittes.Add(soundEmitter);
    }

    private void OnReleaseSoundEmitter(SoundEmitter soundEmitter)
    {
        soundEmitter.gameObject.SetActive(false);
        soundEmitter.transform.SetParent(transform);
        avaliableSoundEmittes.Remove(soundEmitter);
    }

    private void OnDestroySoundEmitter(SoundEmitter soundEmitter)
    {
        Destroy(soundEmitter.gameObject);
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}

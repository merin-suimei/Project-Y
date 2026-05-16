using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundEmitter : MonoBehaviour
{
    public AudioSource audioSource {  get; private set; }
    private Coroutine playingCoroutine;
    public SoundDataSO soundData {  get; private set; }

    private bool isPaused;
    private bool hasFocus; //does game window has focus (we shoud not destroy sounds)

    private float baseVolume;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnApplicationFocus(bool focus)
    {
        hasFocus = focus;
    }
    public SoundEmitter Initialize(SoundDataSO soundDataSO)
    {
        soundData = soundDataSO;
        audioSource.resource = soundData.AudioResource;
        audioSource.loop = soundData.IsLooping;
        audioSource.playOnAwake = soundData.PlayOnAwake;

        baseVolume = soundData.volume;
        float globalVolumeModifier = soundData.IsSFX
                ? SoundManager.Instance.VolumeData.SFXVolume
                : SoundManager.Instance.VolumeData.MusicVolume;

        audioSource.volume = baseVolume * globalVolumeModifier;
        return this;
    }

    public void Play()
    {
        isPaused = false;
        if (playingCoroutine != null)
        {
            StopCoroutine(playingCoroutine);
        }
        audioSource.Play();
        playingCoroutine = StartCoroutine(WaitForPlay());
    }

    public void Stop()
    {
        isPaused = false;
        if (playingCoroutine != null)
        {
            StopCoroutine(playingCoroutine);
        }
        playingCoroutine = null;
        SoundManager.Instance.Release(this);
    }

    public void Pause()
    {
        isPaused=true;
        audioSource.Pause();
    }
    public void UnPause()
    {
        audioSource.UnPause();
        isPaused=false;
    }
    public void ChangeVolume(float value)
    {
        audioSource.volume = baseVolume * value;
    }
    private IEnumerator WaitForPlay()
    {
        yield return new WaitWhile(() => audioSource.isPlaying || isPaused || !hasFocus);
        SoundManager.Instance.Release(this);
    }

    private void OnDestroy()
    {
        playingCoroutine = null;
    }
}


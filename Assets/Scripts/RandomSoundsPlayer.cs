using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSoundsPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip[] sounds;
    [SerializeField] private float volume;
    [SerializeField] private float delayBetweenSounds = 0.2f;
    private List<SoundDataSO> soundsData;
    private Coroutine coroutine;
    private SoundEmitter soundEmitter;
    private int previousIndex = 0;
    private void Start()
    {
        soundsData = new List<SoundDataSO>();
        for (int i = 0; i < sounds.Length; i++)
        {
            SoundDataSO newSoundData = ScriptableObject.CreateInstance<SoundDataSO>();
            newSoundData.AudioResource = sounds[i];
            newSoundData.volume = volume;
            newSoundData.PlayOnAwake = false;
            newSoundData.IsSFX = false;
            newSoundData.IsLooping = false;

            soundsData.Add(newSoundData);
        }
    }

    public void LaunchRandomSound()
    {
        if (coroutine == null)
            coroutine = StartCoroutine(Play());
    }
    
    private IEnumerator Play()
    {
        int currentIndex = Random.Range(0, soundsData.Count);
        while(previousIndex == currentIndex)
        {
            currentIndex = Random.Range(0, soundsData.Count);
        }
        previousIndex = currentIndex;
        Debug.Log(currentIndex);
        SoundDataSO currentSoundData = soundsData[currentIndex];
        soundEmitter = SoundManager.Instance.Get().Initialize(currentSoundData);
        soundEmitter.transform.SetParent(transform);
        soundEmitter.transform.localPosition = Vector3.zero;
        soundEmitter.Play();
        yield return new WaitForSeconds(sounds[currentIndex].length + delayBetweenSounds);
        coroutine = null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStepsSound : MonoBehaviour
{
    [SerializeField] private AudioClip[] sounds;
    [SerializeField] private float volume;
    [SerializeField] private float distanceBetweenSounds = 2f;
    [SerializeField] private float delayBetweenSounds = 0.2f;
    
    private List<SoundDataSO> soundsData;
    private Coroutine coroutine;
    private SoundEmitter soundEmitter;
    private int previousIndex = 0;
    private Vector3 previousPos;
    private void Start()
    {
        previousPos = transform.position;

        soundsData = new List<SoundDataSO>();
        for (int i = 0; i < sounds.Length; i++)
        {
            SoundDataSO newSoundData = ScriptableObject.CreateInstance<SoundDataSO>();
            newSoundData.AudioResource = sounds[i];
            newSoundData.volume = volume;
            newSoundData.PlayOnAwake = false;
            newSoundData.IsSFX = true;
            newSoundData.IsLooping = false;

            soundsData.Add(newSoundData);
        }

    }

    private void Update()
    {
        if (Vector3.Distance(previousPos, transform.position) >= distanceBetweenSounds)
        {
            LaunchRandomSound();
        }
    }
    public void LaunchRandomSound()
    {
        if (coroutine == null)
        {
            coroutine = StartCoroutine(Play());
            previousPos = transform.position;
        }
    }

    private IEnumerator Play()
    {

        int currentIndex = Random.Range(0, soundsData.Count);
        while (previousIndex == currentIndex && soundsData.Count > 1)
        {
            currentIndex = Random.Range(0, soundsData.Count);
        }
        previousIndex = currentIndex;
        SoundDataSO currentSoundData = soundsData[currentIndex];
        soundEmitter = SoundManager.Instance.Get().Initialize(currentSoundData);
        soundEmitter.audioSource.spatialBlend = 1f;
        soundEmitter.audioSource.rolloffMode = AudioRolloffMode.Linear;
        soundEmitter.transform.SetParent(transform);
        soundEmitter.transform.localPosition = Vector3.zero;
        soundEmitter.audioSource.maxDistance = 12;
        soundEmitter.Play();
        yield return new WaitForSeconds(
            sounds[currentIndex].length + delayBetweenSounds
        );

        coroutine = null;
    }
}

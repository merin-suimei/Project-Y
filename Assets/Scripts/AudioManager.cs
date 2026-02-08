using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private float timeBetweenSteps = 10f;
    [SerializeField] private AudioClip[] playerFootSteps;
    [SerializeField] private AudioClip enemyMove;
    [SerializeField] private AudioClip enemyDetect;
    private AudioSource enemyMoveSource;
    private float playerFootStepsVolume = 0.02f;
    private float enemyMoveVolume = 0.09f;
    private float enemyDetectVolume = 0.2f;
    private Coroutine footstepsCoroutine;
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        EventBus.Subscribe(EventType.PlayPlayerFootStepSound, PlayPlayerFootSteps);
        EventBus.Subscribe(EventType.StopPlayerFootStepSound, StopPlayerFootStep);
        EventBus.Subscribe(EventType.PlayEnemyMoveSound, PlayEnemyMoveSound);
        EventBus.Subscribe(EventType.StopEnemyMoveSound, StopEnemyMoveSound);
        EventBus.Subscribe(EventType.PlayEnemyDetectSound, PlayEnemyDetectSound);
    }

    
    private void PlaySFX(AudioClip clip, float volume = 1f)
    {
        StartCoroutine(PlaySFXCoroutine(clip, volume)); 
    }
    
    private IEnumerator PlaySFXCoroutine(AudioClip clip, float volume = 1f)
    {
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.Play();

        yield return new WaitForSeconds(audioSource.clip.length);
        Destroy(audioSource);
    }


    private void PlayPlayerFootSteps()
    {
        if (footstepsCoroutine != null) return;
        footstepsCoroutine = StartCoroutine(PlayPlayerFootStepsCoroutine());
    }
    private void StopPlayerFootStep()
    {
        if (footstepsCoroutine != null)
        {
            StopCoroutine(footstepsCoroutine);
            footstepsCoroutine = null;
        }
    }
    private IEnumerator PlayPlayerFootStepsCoroutine()
    {
        while (true)
        {
            PlaySFX(playerFootSteps[Random.Range(0, playerFootSteps.Length)], playerFootStepsVolume);

            yield return new WaitForSeconds(timeBetweenSteps);
        }
    }


    private void PlayEnemyMoveSound()
    {
        if (enemyMoveSource == null)
        {
            enemyMoveSource = gameObject.AddComponent<AudioSource>();   
            enemyMoveSource.clip = enemyMove;
            enemyMoveSource.volume = enemyMoveVolume;
            enemyMoveSource.loop = true;
            enemyMoveSource.Play();
        }
        if (!enemyMoveSource.isPlaying)
        {
            enemyMoveSource.Play();
        }
    }

    private void StopEnemyMoveSound()
    {
        if (enemyMoveSource != null && enemyMoveSource.isPlaying)
        {
            enemyMoveSource.Stop();
        }
    }

    private void PlayEnemyDetectSound()
    {
        PlaySFX(enemyDetect, enemyDetectVolume);
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class SoundPlayer : MonoBehaviour
{
    [SerializeField] private SoundDataSO soundData;
    [SerializeField] private float minDistance = 1;
    [SerializeField] private float maxDistance = 500;
    [SerializeField] private bool is3DSound;

    [SerializeField] private float waitTimeToLoad;
    private void Start()
    {
        SoundEmitter emitter = SoundManager.Instance.Get().Initialize(soundData);
        emitter.transform.SetParent(transform);
        emitter.transform.localPosition = Vector3.zero;
        emitter.audioSource.spatialBlend = is3DSound ? 1f : 0f;
        emitter.audioSource.rolloffMode = AudioRolloffMode.Linear;
        emitter.audioSource.minDistance = minDistance;
        emitter.audioSource.maxDistance = maxDistance;
        StartCoroutine(PlaySound(emitter));
    }

    private IEnumerator PlaySound(SoundEmitter emitter)
    {
        yield return new WaitForSeconds(waitTimeToLoad);
        emitter.Play();

    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, minDistance);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, maxDistance);
    }
}

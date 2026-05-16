using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [SerializeField] private SoundDataSO soundData;
    [SerializeField] private float minDistance = 1;
    [SerializeField] private float maxDistance = 500;
    private void Start()
    {
        SoundEmitter emitter = SoundManager.Instance.Get().Initialize(soundData);
        emitter.transform.position = transform.position;
        emitter.audioSource.spatialBlend = 1;
        emitter.audioSource.rolloffMode = AudioRolloffMode.Linear;
        emitter.audioSource.minDistance = minDistance;
        emitter.audioSource.maxDistance = maxDistance;
        emitter.Play();
        Debug.Log(minDistance);
        Debug.Log(maxDistance);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, minDistance);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, maxDistance);
    }
}

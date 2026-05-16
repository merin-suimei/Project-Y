using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "SoundDataSO", menuName = "Scriptable Objects/SoundDataSO")]
public class SoundDataSO : ScriptableObject
{
    public AudioResource AudioResource;
    public float volume;
    public bool IsLooping;
    public bool PlayOnAwake;
    public bool IsSFX; //if is not sfx, then music
}

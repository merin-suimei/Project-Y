using UnityEngine;

[CreateAssetMenu(fileName = "SoundVolumeSO", menuName = "Scriptable Objects/SoundVolumeSO")]
public class SoundVolumeSO : ScriptableObject
{
    [Header("For UI")]
    public float AllSoundsVolumeView;
    public float SFXVolumeView;
    public float MusicVolumeView;

    [Header("For system")]
    public float SFXVolume;
    public float MusicVolume;
}

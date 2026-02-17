using UnityEngine;

[CreateAssetMenu(fileName = "Settings", menuName = "Scriptable Objects/Settings")]
public class Settings : ScriptableObject
{
    // video
    public Vector2Int screenResolution;
    public bool fullScreen;
    public bool VSync;

    //audio
    [Range(0f, 1f)] public float soundVolume;
    [Range(0f, 1f)] public float musicVolume;
    [Range(0f, 1f)] public float effectsVolume;
}

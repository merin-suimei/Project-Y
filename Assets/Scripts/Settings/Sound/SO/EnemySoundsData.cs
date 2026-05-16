using UnityEngine;

[CreateAssetMenu(fileName = "EnemySoundsData", menuName = "Scriptable Objects/EnemySoundsData")]
public class EnemySoundsData : ScriptableObject
{
    public SoundDataSO walkSoundData;
    public SoundDataSO detectSounddata;
    public SoundDataSO catchSoundData;
}

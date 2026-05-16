using UnityEngine;

[CreateAssetMenu(fileName = "ScreamerSO", menuName = "Scriptable Objects/ScreamerSO")]
public class ScreamerSO : ScriptableObject
{
    public AudioClip audioClip;
    public Sprite[] sprites;
}

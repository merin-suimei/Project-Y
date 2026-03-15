using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "BrightnessData", menuName = "Scripts/Settings/BrightnessData.cs")]
public class BrightnessData : ScriptableObject
{
    public float gammaIntensity;
    public UnityAction<float> OnGammaIntensityChanged;

    public void SetGammaIntensity(float value)
    {
        gammaIntensity = value;
        PlayerPrefs.SetFloat("GammaIntensity", gammaIntensity);
        PlayerPrefs.Save();
        OnGammaIntensityChanged?.Invoke(value);
    }

    public void Load()
    {
        gammaIntensity = PlayerPrefs.GetFloat("GammaIntensity", 0.0f);
    }
}

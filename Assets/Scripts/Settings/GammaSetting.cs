using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class GammaSetting : MonoBehaviour
{
    [SerializeField] BrightnessData brightnessData;
    [SerializeField] Slider gammaSlider;

    private void Start()
    {
        brightnessData.Load();
        gammaSlider.SetValueWithoutNotify(brightnessData.gammaIntensity);
        gammaSlider.onValueChanged.AddListener(brightnessData.SetGammaIntensity);
    }
}

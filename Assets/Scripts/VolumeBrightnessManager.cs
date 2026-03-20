using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VolumeBrightnessManager : MonoBehaviour
{
    [SerializeField] private BrightnessData brightnessData;
    private Volume volume;
    private static VolumeBrightnessManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        volume = GetComponent<Volume>();
        brightnessData.Load();

        Apply(brightnessData.gammaIntensity);
        
    }

    private void OnEnable()
    {
        brightnessData.OnGammaIntensityChanged += Apply;
    }

    private void OnDisable()
    {
        brightnessData.OnGammaIntensityChanged -= Apply;
    }

    private void Apply(float value)
    {
        if (volume.profile.TryGet<LiftGammaGain>(out var lgg))
        {
            lgg.gamma.overrideState = true;
            lgg.gamma.value = new Vector4(lgg.gamma.value.x, lgg.gamma.value.y, lgg.gamma.value.z, value);
        }
    }
}